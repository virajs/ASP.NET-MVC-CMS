using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Linq.Expressions;

using ComLib.Data;
using ComLib.Authentication;
using ComLib.Extensions;
using ComLib.Web.Modules;
using ComLib.Web.Modules.Tags;
using ComLib.Web.Modules.Comments;
using ComLib.Web.Modules.Categorys;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.CaptchaSupport;


namespace ComLib.Web.Modules.Posts
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [ValidateInput(false)]
    [HandleError]
    public class PostController : JsonController<Post>
    {
        /// <summary>
        /// Initialize the index page size.
        /// </summary>
        protected override void Init()
        {
            _titleFetcher = (post) => post.SlugUrl;
            InitColumns(
               columnNames: new List<string>() {"Id", "Published", "Public", "Updated", "Title", "Category" },
               columnProps: new List<Expression<Func<Post, object>>>() { a => a.Id, a => a.IsPublished, a => a.IsPublic,  a => a.UpdateDate, a => a.Title, a => a.CategoryName },
               columnWidths: null);
        }


        /// <summary>
        /// Get a list of all the posts by the page number.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public override ActionResult Index(int page = 1, int pageSize = 15)
        {
            SetPageTitle("Posts");
            IQuery<Post> query = Query<Post>.New().Where(p => p.IsPublished).Is(true).OrderByDescending(p => p.PublishDate);
            return base.FindPaged(page: page, query: query);
        }


        /// <summary>
        /// Find posts by tags.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult Tags(string tag)
        {
            SetPageTitle(tag);
            IList<Post> posts = Post.GetByTags(tag);
            var pagedItems = new PagedList<Post>(1, 10000, posts.Count, posts);
            return BuildActionResult(new EntityActionResult(true, "", null, pagedItems, true, true), "Pages/List");
        }


        /// <summary>
        /// Find posts by year/month.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult Archives(int year, int month)
        {
            SetPageTitle(string.Format("{0}-{1}", month, year));
            var query = Query<Post>.New().Where(p => p.IsPublished).Is(true).And(p => p.Year).Is(year).And(p => p.Month).Is(month);
            return this.Find(query);
        }


        /// <summary>
        /// Find posts by category
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult Category(int id)
        {
            Tuple2<PagedList<Post>, Category> result = CategoryHelper.FindCategoryAndItems<Post>(id, 1, 10, "CategoryId");
            SetPageTitle(result.Second == null ? "" : result.Second.Name);
            var entityResult = new EntityActionResult(true, "", null, result.First, true, true);
            var actionResult = BuildActionResult(entityResult, "Pages/List");
            return actionResult;
        }
    }
}
