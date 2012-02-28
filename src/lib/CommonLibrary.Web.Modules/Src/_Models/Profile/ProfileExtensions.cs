/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: ? 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using ComLib.Entities;
using ComLib.Authentication;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;
using ComLib.Account;
using ComLib.Data;
using ComLib.Web.Services.GravatarSupport;
using ComLib.Web.Lib.Core;

namespace ComLib.Web.Modules.Profiles
{
    /// <summary>
    /// Profile entity.
    /// </summary>
    public partial class Profile : ActiveRecordBaseEntity<Profile>, IEntity, IEntityMediaSupport
    {
        public Profile()
        {
            _address = new Address();
        }


        /// <summary>
        /// Returns either the alias( if available ) or the username otherwise.
        /// </summary>
        public string Name
        {
            get
            {
                string alias = Alias;
                if (string.IsNullOrEmpty(alias))
                    return UserName;

                return alias;
            }
        }


        private string _imageUrl;
        /// <summary>
        /// Get/Set ImageUrl
        /// </summary>
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }            
        }



        /// <summary>
        /// Get/Set HasImage
        /// </summary>
        public bool HasImage
        {
            get { return ImageRefId > 0; }            
        }


        /// <summary>
        /// Gets the image url for profile which can be either the gravatar which is used 1st and then the internal image photo.
        /// </summary>
        /// <param name="wrapInImageTag"></param>
        /// <param name="size"></param>
        /// <param name="align"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        public string GetImageUrl(bool wrapInImageTag = true, int size = 80, bool useSameSizeForHeight = false, string align = "top", string style = "padding:2px 5px 5px 2px", bool removeImageIfNotAvailable = true)
        {
            string url = string.Empty;
            if (IsGravatarEnabled)
            {
                Gravatar gravatar = new Gravatar(Email, size, Rating.g, IconType.none, string.Empty);
                url = gravatar.Url;
            }
            else if (HasImage)
            {
                url = _imageUrl;
            }

            if (string.IsNullOrEmpty(url)) return string.Empty;
            if (wrapInImageTag)
            {
                string attributes = "alt=\"user photo\" align=\"" + align + "\" style=\"" + style + "\"";
                if (HasImage)
                {
                    attributes += " width=\"" + size + "px\"";
                    if (useSameSizeForHeight)
                        attributes += " height=\"" + size + "px\" ";
                }
                // <img src="<%= gravatar.Url %>" alt="gravatar" align="top" style=" padding: 2px 5px 5px 2px" />
                url = "<img src=\"" + url + "\" " + attributes + " />";
            }
            return url;
        }


        /// <summary>
        /// On before save callback
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            if (string.IsNullOrEmpty(CreateUser))
                CreateUser = this.UserName;
            if (string.IsNullOrEmpty(UpdateUser))
                UpdateUser = this.UserName;

            return true;
        }


        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Profile entity = (Profile)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.UserName, false, false, true, 1, 150, results, "UserName");
                Validation.IsStringLengthMatch(entity.About, true, true, true, 1, 2500, results, "About");
                Validation.IsStringLengthMatch(entity.FirstName, true, true, true, 1, 20, results, "FirstName");
                Validation.IsStringLengthMatch(entity.LastName, true, true, true, 1, 20, results, "LastName");
                Validation.IsStringRegExMatch(entity.Email, false, RegexPatterns.Email, results, "Email");
                Validation.IsStringRegExMatch(entity.WebSite, true, RegexPatterns.Url, results, "WebSite");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Creates a new profile.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="email"></param>
        /// <param name="imageUrl"></param>
        /// <param name="isGravatarEnabled"></param>
        /// <param name="websiteUrl"></param>
        public static void Create(string username, int userId, string first, string last, string email, string about, bool isGravatarEnabled, string websiteUrl)
        {
            Profile profile = new Profile()
            {
                UserId = userId,
                UserName = username,
                About = about,
                Email = email,
                FirstName = first,
                LastName = last,
                IsGravatarEnabled = isGravatarEnabled,
                WebSite = websiteUrl
            };
            Create(profile);
        }


        /// <summary>
        /// Creates a new profile.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="email"></param>
        /// <param name="imageUrl"></param>
        /// <param name="isGravatarEnabled"></param>
        /// <param name="websiteUrl"></param>
        public static BoolMessageItem<Profile> Create(User user)
        {
            var profile = new Profile();
            profile.EnableDisplayOfName = false;
            profile.About = string.Empty;
            profile.IsAddressEnabled = false;
            profile.Email = user.Email;
            profile.UserId = user.Id;
            profile.UserName = user.UserName;            
            Create(profile);
            return new BoolMessageItem<Profile>(profile, !profile.Errors.HasAny, profile.Errors.Message("<br/>"));
        }


        /// <summary>
        /// Create the profile conditionally based on whether the profile exists already based on the checkField expression values.
        /// Also gets the user id from the users and applies it to the profiles before conditionally creating the profile.
        /// </summary>
        /// <param name="profiles"></param>
        /// <param name="getUserId"></param>
        /// <param name="checkFields"></param>
        public static void Create(IList<Profile> profiles, bool getUserId, params Expression<Func<Profile, object>>[] checkFields)
        {
            // Set the user id on the profile by querying the users repo w/ the profile's username.
            foreach (var profile in profiles)
            {
                if (profile.UserId <= 0 || getUserId)
                {
                    var user = User.Find(Query<User>.New().Where(u => u.UserNameLowered).Is(profile.UserName.ToLower())).First();
                    if (user != null)
                    {
                        profile.UserId = user.Id;
                    }
                }
            }
            // Now create the conditionally.         
            Create(profiles, checkFields);
        }


        /// <summary>
        /// Changes the email.
        /// </summary>
        /// <param name="newEmail">The new email.</param>
        /// <param name="profile">The profile.</param>
        public static BoolMessage ChangeEmail(string newEmail, Profile profile)
        {
            return User.ChangeEmail(profile.UserId, newEmail);
        }


        /// <summary>
        /// Find by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Profile FindUser(string username)
        {
            var profile = Profile.Repository.First(Query<Profile>.New().Where( p => p.UserName).Is(username));
            return profile;
        }
    }


    public static class ProfileExtensions
    {
        /// <summary>
        /// Checks whether or not the current logged in user is the same user represented by the profile based on the ID.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="checkById"></param>
        /// <returns></returns>
        public static bool IsLoggedInUser(this Profile profile)
        {
            if (!Auth.IsAuthenticated()) return false;
            if (profile == null) return false;
            if (Auth.UserShortName != profile.UserName) return false;

            return true;
        }


        /// <summary>
        /// Delete the photo.
        /// </summary>
        /// <param name="profile"></param>
        public static void DeletePhoto(this Profile profile)
        {
            var svc = EntityRegistration.GetService<Profile>();
            var mservice = EntityRegistration.GetService<ComLib.Web.Modules.Media.MediaFile>();
            mservice.Delete(profile.ImageRefId);
            profile.ImageRefId = 0;
            profile.ImageUrl = string.Empty;
            profile.Save();
        }
    }
}
