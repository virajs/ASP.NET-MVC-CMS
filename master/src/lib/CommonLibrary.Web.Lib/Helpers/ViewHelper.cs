using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using ComLib.Entities;
using ComLib.Authentication;
using ComLib.Extensions;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Extensions;


namespace ComLib.Web.Lib.Helpers
{
    public class ViewHelper
    {
        private static IDictionary<string, bool> _actionToExcludeLayout = new Dictionary<string, bool>();

        
        /// <summary>
        /// Excludes the layout setup on action specified.
        /// </summary>
        /// <param name="actions">The actions.</param>
        public static void ExcludeLayoutSetupOnActions(params string[] actions)
        {
            foreach (string action in actions)
            {
                _actionToExcludeLayout[action] = true;
            }
        }


        /// <summary>
        /// Builds the view model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="view">The view.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="controlPath">The control path.</param>
        /// <param name="useForm">if set to <c>true</c> [use form].</param>
        /// <param name="conf">The conf.</param>
        public static void BuildViewModel<T>(System.Web.Mvc.UrlHelper url, EntityBaseViewModel view, T entity, string controllerName, string controlPath, bool useForm, IDictionary conf) where T: IEntity
        {
            string baseurl = string.Empty;
            var editUrl = url.Link("edit", controllerName, null);
            var deleteUrl = url.Link("delete", controllerName, null);
            var indexUrl = url.Link("index", controllerName, null);
            var manageUrl = url.Link("manage", controllerName, null);
            var copyUrl = url.Link("copy", controllerName, null);
            bool allowEdit = false;

            if (entity != null)
            {
                copyUrl += entity.Id;
                editUrl += entity.Id;
                deleteUrl += entity.Id;
                allowEdit = Auth.IsUserOrAdmin(entity.CreateUser);
            }
            view.Name = controllerName;
            view.ControllerName = controllerName;
            view.UrlEdit = editUrl;
            view.UrlCopy = copyUrl;
            view.UrlDelete = deleteUrl;
            view.UrlIndex = indexUrl;
            view.UrlManage = manageUrl;
            view.UrlCancel = manageUrl;
            view.AllowDelete = allowEdit;
            view.AllowDelete = allowEdit;
            view.ControlPath = controlPath;
            view.Config = conf;
            if (view is EntityDetailsViewModel)
                ((EntityDetailsViewModel)view).Entity = entity;
            else if (view is EntityFormViewModel)
                ((EntityFormViewModel)view).Entity = entity;
        }


        /// <summary>
        /// Sets the master page.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public static void SetMasterPage(ActionExecutedContext filterContext, ControllerContext controllerContext, bool overrideControllerBasedLayoutLogic, string overrideLayout, bool forceTheme)
        {
            var view = filterContext.Result as ViewResult;
            if (view != null)
            {
                // Check layout override.
                // e.g. /Theme/Layouts should use the Site.Dashboard layouts/masterpage.
                if (overrideControllerBasedLayoutLogic && !string.IsNullOrEmpty(overrideLayout))
                {
                    view.MasterName = overrideLayout;
                    return;
                }

                // Check if action is create/edit/manage, the don't do any master page / layout modification.
                string action = controllerContext.RouteData.Values["action"] as string;
                if (!forceTheme && !string.IsNullOrEmpty(action) && _actionToExcludeLayout.ContainsKey(action.ToLower()))
                    return;

                // Set the master page.
                string masterpage = controllerContext.RequestContext.HttpContext.Items["theme_layout"] as string;
                if (string.IsNullOrEmpty(masterpage))
                    masterpage = "Site";

                view.MasterName = masterpage;
            }
        }


        /// <summary>
        /// Sets the theme in current request.
        /// </summary>
        /// <param name="themeName">Name of the theme.</param>
        /// <param name="masterPageLayout">The master page layout.</param>
        /// <param name="theme">The theme.</param>
        public static void SetThemeInCurrentRequest(string themeName, string masterPageLayout, object theme)
        {
            string layout = string.IsNullOrEmpty(masterPageLayout) ? "Site" : masterPageLayout;
            string nameOfTheme = string.IsNullOrEmpty(themeName) ? "Sapphire" : themeName;
            HttpContext.Current.Items["theme"] = theme;
            HttpContext.Current.Items["theme_name"] = nameOfTheme;
            HttpContext.Current.Items["theme_layout"] = layout;
        }
    }
}
