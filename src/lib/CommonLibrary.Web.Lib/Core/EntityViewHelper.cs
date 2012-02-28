using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Authentication;
using ComLib.Web.Lib.Models;
using ComLib.Data;
using ComLib.Web.Lib.Extensions;


namespace ComLib.Web.Lib.Core
{
    /// <summary>
    /// Class for helper methods pertaining the entity
    /// </summary>
    public class EntityViewHelper
    {
        /// <summary>
        /// Build one of the view models either for Details, Form, List.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="modelname"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static EntityBaseViewModel BuildViewModel<T>(EntityActionResult result, string modelname, string action, EntitySettingsHelper settings)
        {
            action = action.ToLower();
            EntityBaseViewModel viewModel = null;
            if (action == "details" )
                viewModel = BuildViewModelForDetails(result, modelname, action, settings);

            else if (action == "edit" || action == "create")
                viewModel = BuildViewModelForForm(result, modelname, action, settings);

            else if (action == "deleted")
                viewModel = BuildViewModelForDeleted(result, modelname, settings);

            else // List/Index pages.
                viewModel = BuildViewModelForIndex<T>(result, modelname, action, settings);

            return viewModel;
        }


        /// <summary>
        /// Build the details view model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static EntityDetailsViewModel BuildViewModelForDetails(EntityActionResult result, string modelname, string action, EntitySettingsHelper settings)
        {
            var viewmodel = new EntityDetailsViewModel();
            PopulateViewModel(viewmodel, modelname, "ModelDetails", settings);
            viewmodel.Entity = result.ItemAs<IEntity>();            
            return viewmodel;
        }


        /// <summary>
        /// Build the form view model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static EntityFormViewModel BuildViewModelForForm(EntityActionResult result, string modelname, string action, EntitySettingsHelper settings)
        {
            var viewmodel = new EntityFormViewModel();
            PopulateViewModel(viewmodel, modelname, "ModelForm", settings);
            viewmodel.Entity = result.ItemAs<IEntity>();
            viewmodel.FormActionName = action;
            return viewmodel;
        }


        /// <summary>
        /// Build the form view model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static EntityListViewModel BuildViewModelForIndex<T>(EntityActionResult result, string modelname, string action, EntitySettingsHelper settings)
        {
            PagedList<T> items = result.ItemAs<PagedList<T>>();
            var viewmodel = new EntityListViewModel<T>(items, null, true);
            PopulateViewModel(viewmodel, modelname, "ModelList", settings);
            return viewmodel;
        }


        /// <summary>
        /// Build the form view model.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="modelname"></param>
        /// <returns></returns>
        public static EntityDeletionViewModel BuildViewModelForDeleted(EntityActionResult result, string modelname, EntitySettingsHelper settings)
        {
            var viewmodel = new EntityDeletionViewModel(modelname, "Item has been deleted.");
            PopulateViewModel(viewmodel, modelname, "ModelDetails", settings);
            return viewmodel;
        }
        

        /// <summary>
        /// Build general view model values.
        /// </summary>
        /// <param name="viewmodel"></param>
        /// <param name="modelname"></param>
        /// <param name="controlpath"></param>
        /// <returns></returns>
        public static void PopulateViewModel(EntityBaseViewModel viewmodel, string modelname, string controlpath, EntitySettingsHelper settings)
        {
            viewmodel.UrlCancel = "";

            // This is "modellist", "modelform", "modeldetails" or whater is passed in.
            viewmodel.ControlPath = controlpath;

            // These two should be the same most of the time.
            viewmodel.Name = modelname;
            viewmodel.ControllerName = modelname;
            viewmodel.UrlCreate = settings.GetOrDefault(modelname, "View.UrlForCreate", "/" + modelname + "/create");
            viewmodel.UrlManage = settings.GetOrDefault(modelname, "View.UrlForManage", "/" + modelname + "/manage");
            viewmodel.UrlIndex  = settings.GetOrDefault(modelname, "View.UrlForIndex",  "/" + modelname + "/index");

            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                // Check the source url for supporting the back/cancel features.
                if (HttpContext.Current.Request.UrlReferrer != null
                    && !string.IsNullOrEmpty(HttpContext.Current.Request.UrlReferrer.PathAndQuery))
                {
                    viewmodel.UrlCancel = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
                    viewmodel.UrlBack = HttpContext.Current.Request.UrlReferrer.PathAndQuery;
                }
            }
        }
        

        /// <summary>
        /// Set the dault page locations for actions ( create/edit/list/manage ).
        /// </summary>
        /// <param name="viewSettings"></param>
        public static void CheckAndDefaultSettings(ModelSettings settings)
        {
            if (settings == null) return;

            var permissions = settings.Permissions;
            var viewSettings = settings.View;
            if(settings.Permissions != null)
                if (string.IsNullOrEmpty(permissions.RolesForEdit)) permissions.RolesForEdit = permissions.RolesForCreate;

            if (settings.View != null)
            {
                if (string.IsNullOrEmpty(viewSettings.PageLocationForCreate)) viewSettings.PageLocationForCreate = "Pages/Create";
                if (string.IsNullOrEmpty(viewSettings.PageLocationForEdit)) viewSettings.PageLocationForEdit = "Pages/Edit";
                if (string.IsNullOrEmpty(viewSettings.PageLocationForDetails)) viewSettings.PageLocationForDetails = "Pages/Details";
                if (string.IsNullOrEmpty(viewSettings.PageLocationForIndex)) viewSettings.PageLocationForIndex = "Pages/List";
                if (string.IsNullOrEmpty(viewSettings.PageLocationForManage)) viewSettings.PageLocationForManage = "Pages/Manage";
                if (string.IsNullOrEmpty(viewSettings.ActionForCreationSuccess)) viewSettings.ActionForCreationSuccess = "Details";
            }
        }


        /// <summary>
        /// Populate the model state with errors.
        /// </summary>
        /// <param name="result"></param>
        public static void PopulateErrors(EntityActionResult result, ModelStateDictionary modelState, IList<string> stringErrors = null, IErrors errors = null)
        {
            if (result.Success || result.Errors == null || !result.Errors.HasAny)
                return;

            if (modelState["Id"] != null) modelState.Remove("Id");
            if (result.Errors.HasAny) modelState.AddErrors(result.Errors);
            
            if (errors != null && errors.Count > 0)
                modelState.AddErrors(errors);
        }
    }
}
