using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComLib.Authentication;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Core;


namespace ComLib.Web.Modules.Themes
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class ThemeController : EntityController<Theme>
    {
        /// <summary>
        /// Activates the theme
        /// </summary>
        [Authorize(Roles = "Admin")]
        public ActionResult ActivateTheme(int themeid)
        {
            Theme.Activate(themeid);
            return Json(new BoolMessage(true, "Theme has been changed."), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Edits the CSS for the current users theme.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]        
        public ActionResult EditCss()
        {
            DashboardLayout(true);
            return View("Pages/FeatureNya");
        }


        /// <summary>
        /// Show the layouts possible.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Layouts()
        {
            DashboardLayout(true);
            var viewModel = BuildLayoutsViewModel();
            return View(viewModel);
        }


        /// <summary>
        /// Show the layouts possible.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult ChangeLayout(string layout)
        {
            DashboardLayout(true);
            bool canChangeLayout = Theme.Current.IsLayoutChangeable;
            
            // Check.
            if (!canChangeLayout)
            {
                return Json(new { Success = false, Message = "Can not change the layout for this theme." }, JsonRequestBehavior.AllowGet);
            }

            // Check.
            string newLayoutName = layout;
            if (string.IsNullOrEmpty(layout) || !Theme.Current.LayoutsMap.ContainsKey(newLayoutName))
            {
                return Json(new { Success = false, Message = "Unknown layout name. Can not change layout." }, JsonRequestBehavior.AllowGet);
            }
            
            Theme.Current.SelectedLayout = newLayoutName;
            return Json(new { Success = true, Message = "Layout has been changed." }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Gets paged index of entities that can be managed(edited/deleted).
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public override ActionResult Manage(int page = 1, int pageSize = 15)
        {
            // Allow manage to view all posts.
            DashboardLayout(true);
            return FindPaged(page, pageSize, Query<Theme>.New().Where(t => t.CreateUser).NotNull().OrderBy(t => t.SortIndex));
        }


        private LayoutsViewModel BuildLayoutsViewModel()
        {
            bool canChangeLayout = Theme.Current.IsLayoutChangeable;
            ReadOnlyCollection<Layout> layouts = Theme.Current.LayoutsList;
            string message = string.Empty;
            if (!canChangeLayout) ToDo.Implement(ToDo.Priority.Low, "kishore", "Localize the message", () => message = "This layout only has a single layout");
            var viewModel = new LayoutsViewModel() { CanChangeLayout = canChangeLayout, Layouts = layouts, Message = message };
            return viewModel;
        }
    }
}
