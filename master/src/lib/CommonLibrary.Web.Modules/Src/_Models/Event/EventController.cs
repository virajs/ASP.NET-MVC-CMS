using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Linq.Expressions;

using ComLib;
using ComLib.Patterns;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.LocationSupport;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.ViewModels;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Modules.Categorys;
using ComLib.Web.Modules.Tags;


namespace ComLib.Web.Modules.Events
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class EventController : JsonController<Event>
    {
        private string _modelAlias = "Events";


        /// <summary>
        /// Initialize the index page size.
        /// </summary>
        protected override void Init()
        {
            _titleFetcher = (post) => post.SlugUrl;
            InitColumns(
               columnNames: new List<string>() {"Id", "Updated", "Title", "Description", "Starts", "Time", "Category" },
               columnProps: new List<Expression<Func<Event, object>>>() 
               { 
                   a => a.Id, a => a.UpdateDate, a => a.Title, a => a.Description, a => a.StartDate, a => a.Starts, a => a.CategoryName 
               },
               columnWidths: null, customRowBuilder: "EventGridBuilder");
        }


        public override ActionResult Create()
        {
            SetPageTitle("Create " + _modelAlias);
            var result = _helper.Create();
            var entity = result.ItemAs<Event>();
            entity.StartDate = DateTime.Today;
            entity.EndDate = DateTime.Today;
            entity.StartTime = DateTime.Now.TimeOfDay.ToMilitaryInt();
            entity.EndTime = DateTime.Now.TimeOfDay.ToMilitaryInt();
            entity.Address.Country = "United States";
            entity.Address.State = "New York";
            return BuildActionResult(result, _viewSettings.PageLocationForCreate);
        }


        /// <summary>
        /// Set the page title.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public override ActionResult Index(int page = 1, int pagesize = 15)
        {
            SetPageTitle(_modelAlias);
            return base.Index(page, pagesize);
        }


        /// <summary>
        /// Set whether or not to show the search control.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public override ActionResult Manage(int page = 1, int pagesize = 15)
        {
             this.ViewData["DisableSearchControl"] = true;
 	         return base.Manage(page, pagesize);
        }


        /// <summary>
        /// Find posts by category
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult Category(int id)
        {            
            Tuple2<PagedList<Event>, Category> result = CategoryHelper.FindCategoryAndItems<Event>(id, 1, 10, "CategoryId");
            SetPageTitle(result.Second == null ? "" : result.Second.Name);
            var entityResult = new EntityActionResult(true, "", null, result.First, true, true);
            var actionResult = BuildActionResult(entityResult, "Pages/List");
            return actionResult;
        }


        /// <summary>
        /// Find posts by tags.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public ActionResult Tags(string tag)
        {
            SetPageTitle(tag);
            var items = TagHelper.GetByTags<Event>(tag, (ids) => Query<Event>.New().Where(e => e.Id).In<int>(ids).And(p => p.IsPublished).Is(true));
            var pagedItems = new PagedList<Event>(1, 10000, items.Count, items);
            return BuildActionResult(new EntityActionResult(true, "", null, pagedItems, true, true), "Pages/List");
        }


        /// <summary>
        /// Search events.
        /// </summary>
        /// <param name="keywords">Tags to search</param>
        /// <param name="location">Location of events to search for.</param>
        /// <param name="category">Category of events to search for</param>
        /// <param name="page">The page number of the results to get.</param>
        /// <returns></returns>
        public ActionResult Search(string keywords, string location, int category, int page = 1)
        {
            SetPageTitle("Search " + _modelAlias);
            return base.FindPaged(page, 15);
        }
    }
    
}
