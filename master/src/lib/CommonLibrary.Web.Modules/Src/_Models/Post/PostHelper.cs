using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ComLib.Web.Modules.Posts
{
    /// <summary>
    /// Helper class for events.
    /// </summary>
    public class PostHelper
    {
        public static PostActionsViewModel BuildActionsFor<T>(bool hasComments, int commentCount, int id, string title,
            bool enableComments, bool enableFavorites, bool enableFlagging, HttpContext ctx, string permaLink = null)
        {
            // Build  http://localhost:49739/post/Details/14
            // from : http://localhost:49739/post/index
            string actualPermaLink = permaLink == null ? HttpContext.Current.Request.Url.AbsolutePath : permaLink;
            if (permaLink == null)
            {
                actualPermaLink = actualPermaLink.ToLower().Replace("/index", "/details/" + id);
            }
            else
                actualPermaLink = "http://" + ctx.Request.Url.Authority + actualPermaLink;

            string emailBody = "Thought you might like this : " + title + ".";
            emailBody += " " + ctx.Request.Url.AbsoluteUri;
            PostActionsViewModel model = new PostActionsViewModel(hasComments, commentCount, actualPermaLink, actualPermaLink, title, emailBody);
            model.EntityName = typeof(T).Name;
            model.EntityId = id;
            model.EnableComments = enableComments;
            model.EnableFavorites = enableFavorites;
            model.EnableFlaging = enableFlagging;
            model.EntityDetailsUrl = "/" + typeof(T).Name + "/details/" + id;
            return model;
        }


        /// <summary>
        /// Applies cateog
        /// </summary>
        /// <param name="events"></param>
        public static void ApplyCategories(IList<Post> posts)
        {
            if (posts == null || posts.Count == 0) return;

            var lookup = Categorys.Category.LookupFor<Post>();
            foreach (var post in posts)
            {
                post.Category = lookup[post.CategoryId];
                if (post.CategoryName == null && post.Category != null)
                    post.CategoryName = post.Category.Name;
            }
        }
    }
}
