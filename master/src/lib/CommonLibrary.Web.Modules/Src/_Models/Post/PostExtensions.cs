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
using System.Text;
using System.Data;

using ComLib.Extensions;
using ComLib.Entities;
using ComLib.Caching;
using ComLib.LocationSupport;
using ComLib.Data;
using ComLib.Feeds;
using ComLib.Web.Lib.Core;
using ComLib.Configuration;
using ComLib.ValidationSupport;

using ComLib.Web.Modules.Tags;
using ComLib.Web.Modules.Categorys;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Core;

namespace ComLib.Web.Modules.Posts
{
    /// <summary>
    /// Settings specific for blog posts.
    /// </summary>
    public class PostSettings : EntitySettings<Post>
    {
        /// <summary>
        /// Set default values.
        /// </summary>
        public PostSettings()
        {
            CacheTime = 300;
            CacheOnArchives = true;
            CacheOnRecent = true;
            IndexPageSize = 15;
            CommentPageSize = 20;            
        }


        public bool CacheOnArchives { get; set; }
        public bool CacheOnRecent { get; set; }
        public int  CacheTime { get; set; }
        public int NumberDaysCommentsEnabled { get; set; }
        public int IndexPageSize { get; set; }
        public int CommentPageSize { get; set; }
    }



    /// <summary>
    /// BlogPost entity.
    /// </summary>
    [Model(Id = 6, DisplayName = "Post", Description = "BlogPost", IsPagable = true,
        IsExportable = true, IsImportable = true, FormatsForExport = "xml,ini", FormatsForImport = "xml,ini",
        RolesForCreate = "${Admin}", RolesForView = "?", RolesForIndex = "?", RolesForManage = "${Admin}",
        RolesForDelete = "${Admin}", RolesForImport = "${Admin}", RolesForExport = "${Admin}")]
    public partial class Post : ActiveRecordBaseEntity<Post>, IEntity, IPublishable, IEntityActivatable, IEntityClonable, IEntityMediaSupport
    {
        /// <summary>
        /// Singleton settings.
        /// </summary>
        private static PostSettings _settings = new PostSettings();


        #region Properties
        /// <summary>
        /// Year that the blogpost was published.
        /// </summary>
        public int Year { get { return PublishDate.Year; } }


        /// <summary>
        /// Month that the blogpost was published.
        /// </summary>
        public int Month { get { return PublishDate.Month; } }


        /// <summary>
        /// Activate.
        /// </summary>
        public bool IsActive { get { return IsPublished; } set { IsPublished = value; } }


        /// <summary>
        /// Get / set the category name used for import.
        /// </summary>
        public string CategoryName { get; set; }


        public Category Category { get; set; }



        /// <summary>
        /// Url for the slug. e.g. /my-latest-blog-post-with-different-url-than-default-title-48
        /// </summary>
        public string SlugUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Slug))
                    return ComLib.Web.UrlSeoUtils.BuildValidUrl(Title) + "-" + Id;
             
                return ComLib.Web.UrlSeoUtils.BuildValidUrl(Slug) + "-" + Id;
            }
        }
        #endregion


        #region Validation
        /// <summary>
        /// Gets the validator to use for validating this entity.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Post entity = (Post)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Title, false, true, true, 1, 150, results, "Title");
                Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 200, results, "Description");
                Validation.IsStringLengthMatch(entity.Content, false, true, false, 1, -1, results, "Content");                
                Validation.IsStringLengthMatch(entity.Tags, true, false, true, -1, 80, results, "Tags");
                Validation.IsStringLengthMatch(entity.Slug, true, false, true, -1, 150, results, "Slug");
                if (!IsPersistant())
                {
                    Validation.IsDateWithinRange(entity.PublishDate, true, true, DateTime.Today, DateTime.Today.AddDays(180), results, "PublishDate");
                }

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }
        #endregion


        #region IPublishable
        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get { return UpdateUser; } }


        /// <summary>
        /// Gets the relative link.
        /// </summary>
        /// <value>The relative link.</value>
        public string UrlRelative
        {
            get
            {
                return SlugUrl;
            }
        }


        /// <summary>
        /// Gets the absolute link.
        /// </summary>
        /// <value>The relative link.</value>
        public string UrlAbsolute
        {
            get
            {
                return "/" + SlugUrl;
            }
        }


        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public string GuidId
        {
            get { return UrlAbsolute; }
        }
        #endregion


        #region Life-Cycle Events
        /// <summary>
        /// Handle life-cycle event.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeValidate(object ctx)
        {
            if (this.PublishDate == DateTime.MinValue)
                PublishDate = DateTime.Now;

            if (this.PublishDate.TimeOfDay.IsMidnightExactly())
                this.PublishDate = this.PublishDate.GetDateWithCurrentTime();

            return true;
        }


        /// <summary>
        /// On before save callback
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            // Set the category id.
            if (!string.IsNullOrEmpty(CategoryName))
            {
                var lookup = Categorys.Category.LookupFor<Post>();
                this.CategoryId = lookup != null && lookup.ContainsKey(CategoryName) ? lookup[CategoryName].Id : 0;
            }
            this.Description = this.Content.Truncate(200);

            return true;
        }

        
        /// <summary>
        /// After this is created/updated, handle the tags.
        /// </summary>
        public override void OnAfterSave()
        {
            // Queue up the processing of tags.
            int groupId = ModuleMap.Instance.GetId(typeof(Post));
            Queue.Queues.Enqueue<TagsEntry>(new TagsEntry() { GroupId = groupId, RefId = Id, Tags = Tags });
        }
        #endregion


        #region Static Methods
        /// <summary>
        /// Settings for this entity.
        /// </summary>
        public static new PostSettings Settings
        {
            get { return _settings; }
        }


        /// <summary>
        /// Get recent blogposts.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static IList<Post> GetRecent(int count)
        {
            var recents = Cacher.Get<IList<Post>>("Posts_Recent", Settings.CacheOnRecent, Settings.CacheTime.Seconds(), () =>
                                                     Find(Query<Post>.New().Limit(count).Where(p => p.IsPublished).Is(true)
                                                                            .OrderByDescending(p => p.PublishDate)));
            return recents;
        }


        /// <summary>
        /// Return a datatable of the year/month count groupings.
        /// </summary>
        /// <example>
        /// e.g. 
        /// 2009, January,  4
        /// 2009, February, 2
        /// 2009, May,      6
        /// </example>
        /// <returns></returns>
        public static DataTable GetArchives()
        {
            var archives = Cacher.Get<DataTable>("Posts_Archive", Settings.CacheOnArchives, Settings.CacheTime.Seconds(), () =>
            {
                var repo = Post.Repository;
                DataTable groups = repo.Group(Query<Post>.New().Where(p => p.IsPublished).Is(true)
                                                               .OrderByDescending(p => p.Year).OrderBy(p => p.Month), "Year", "Month");
                return groups;
            });
            return archives;
        }


        /// <summary>
        /// Gets the by tags.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static IList<Post> GetByTags(string tag)
        {   
            var posts = TagHelper.GetByTags<Post>(tag, (ids) => Query<Post>.New().Where(p => p.Id).In<int>(ids).And(p => p.IsPublished).Is(true));
            return posts;
        }
        #endregion
    }
}
