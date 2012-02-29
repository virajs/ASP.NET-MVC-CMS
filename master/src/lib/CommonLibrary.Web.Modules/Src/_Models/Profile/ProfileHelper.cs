using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;
using ComLib.Web.Lib.Core;
using ComLib.Data;
using ComLib.Web.Modules.Media;

namespace ComLib.Web.Modules.Profiles
{
    public class ProfileHelper
    {
        /// <summary>
        /// Applies the profile image by setting it to the first one.
        /// </summary>
        /// <param name="profile"></param>
        public static void ApplyProfileImage(Profile profile)
        {
            var groupId = ComLib.Web.Lib.Core.ModuleMap.Instance.GetId(typeof(Profile));
            var files = MediaFile.Find(Query<MediaFile>.New().Where(m => m.RefId).Is(profile.Id).And(m => m.RefGroupId).Is(groupId).OrderBy(m => m.SortIndex));
            profile.ImageRefId = (files != null && files.Count > 0) ? files[0].Id : 0;
            profile.ImageUrl = files[0].AbsoluteUrl;
        }


        /// <summary>
        /// Changes the profile email after checking if it was first changed by comparing the original.
        /// </summary>
        /// <param name="updated"></param>
        /// <param name="original"></param>
        public static void ChangeEmail(Profile updated, Profile original)
        {
            string originalEmail = original.Email;
            string newEmail = updated.Email;
            if (string.Compare(originalEmail, newEmail, true) != 0)
            {
                var emailResult = Profile.ChangeEmail(newEmail, updated);
                if (!emailResult.Success)
                    updated.Errors.Add(emailResult.Message);
            }
        }
    }
}
