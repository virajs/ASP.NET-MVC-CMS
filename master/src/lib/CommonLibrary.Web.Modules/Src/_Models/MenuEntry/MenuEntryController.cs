using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using ComLib.Caching;
using ComLib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.Web.Modules.Pages;


namespace ComLib.Web.Modules.MenuEntrys
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class MenuController : JsonController<MenuEntry>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Name", "Roles", "Public", "Rerouted", "Sort", "Url" },
               columnProps: new List<Expression<Func<MenuEntry, object>>>() 
               { 
                   a => a.Id, a => a.Name, a => a.Roles, a => a.IsPublic, a => a.IsRerouted, a => a.SortIndex, a => a.Url 
               },
               columnWidths: null);
        }


        /// <summary>
        /// Create the menu entry.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post), Authorize(Roles = "Admin")]
        public override ActionResult Create(MenuEntry entity)
        {
            MenuEntryHelper.ClearCacheForMenu();
            return base.Create(entity);
        }



        /// <summary>
        /// Edit the menu entry.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public override ActionResult  Edit(MenuEntry item)
        {
            MenuEntryHelper.ClearCacheForMenu();
            return base.Edit(item);
        }
    }
}
