using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using ComLib.Caching;
using ComLib.Entities;
using ComLib.Extensions;
using ComLib.Data;
using ComLib.Web.Modules.MenuEntrys;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;


namespace ComLib.Web.Modules.Pages
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class PageController : JsonController<Page>
    {
        /// <summary>
        /// Initialize the page title builder
        /// </summary>
        protected override void Init()
        {
            _titleFetcher = (page) => string.IsNullOrEmpty(page.Slug) ? page.Title : page.Slug;
            InitColumns(
               columnNames: new List<string>() { "Id", "Title", "Slug", "Roles", "Parent", "Front", "Public", "Published" },
               columnProps: new List<Expression<Func<Page, object>>>() 
               { a => a.Id, a => a.Title, a => a.Slug, a => a.AccessRoles, a => a.Parent, a => a.IsFrontPage, a => a.IsPublic, a => a.IsPublished },
               columnWidths: null);
            base.Init();
        }


        /// <summary>
        /// Get the page from the database/cache and show it in the view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Show(int id)
        {
            return ShowByEntity(Page.Lookup[id]);
        }


        /// <summary>
        /// Show the page based by using the page name.
        /// </summary>
        /// <param name="pagename"></param>
        /// <returns></returns>
        public ActionResult ShowByName(string pagename)
        {
            return ShowByEntity(MenuEntry.GetPage(pagename));
        }

        
        [ValidateInput(false)]
        [AdminAuthorization]
        [AcceptVerbs(HttpVerbs.Post)]
        public override ActionResult Create(Page page)
        {
            string addToMenu = this.Request.Params["IsFrontPage"];
            bool isPartOfMenu = addToMenu.Contains("true");
            var result = _helper.Create(page);
            if (result.Success)
            {
                if (isPartOfMenu) 
                    MenuEntry.CreateFromPage(page);

                MenuEntryHelper.ClearCacheForMenu();
            }
            return BuildRedirectResult(result, successAction: "Details", routeValues: new { id = page.Id },
                        viewLocationForErrors: _viewSettings.PageLocationForEdit, viewActionForErrors: "Create");
        }


        [ValidateInput(false)]
        [AdminAuthorization]
        public override ActionResult Edit(Page page)
        {
            string addToMenu = this.Request.Params["IsFrontPage"];
            bool isPartOfMenu = addToMenu.Contains("true");
            var result = _helper.Edit(page);
            if (result.Success)
            {
                if( isPartOfMenu) 
                    MenuEntry.UpdateFromPage(isPartOfMenu, page.Id);
                MenuEntryHelper.ClearCacheForMenu();
            }
            return BuildRedirectResult(result, successAction: "Details", routeValues: new { id = page.Id },
                        viewLocationForErrors: _viewSettings.PageLocationForEdit, viewActionForErrors: "Edit");
        }

        
        [AdminAuthorization]
        public override ActionResult Delete(int id)
        {
            EntityActionResult result = _helper.Delete(id);
            var message = result.Success ? _modelname + " has been deleted" : result.Message;
            if (!result.Success)
                return Json(new { Success = result.Success, Message = message }, JsonRequestBehavior.AllowGet);

            var items = MenuEntry.Find(Query<MenuEntry>.New().Where(m => m.RefId).Is(id));
            if (items != null && items.Count > 0)
            {
                var item = items[0];
                MenuEntry.Delete(item);
            }
            return Json(new { Success = result.Success, Message = message }, JsonRequestBehavior.AllowGet);
        }


        private ActionResult ShowByEntity(Page page)
        {
            if (page == null || !page.IsPublic)
                return View(PageLocationForNotFound);

            var result = new EntityActionResult(true, item: page, isAuthorized: true, isAvailable: true);
            HandlePageTitle(result);
            return BuildActionResult(result, _viewSettings.PageLocationForDetails);
        }
    }
}
