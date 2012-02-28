using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

using ComLib.Data;
using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Authentication;
using ComLib.Extensions;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;


namespace ComLib.Web.Modules.Widgets
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class WidgetController : JsonController<WidgetInstance>
    {
        private static IDictionary<string, string> _excludeMappingProps;

        /// <summary>
        /// Initializes the <see cref="WidgetController"/> class.
        /// </summary>
        static WidgetController()
        {
            string[] propsToExcludeForMapping = new string[] { "AppId", "CreateDate", "UpdateDate", "CreateUser", "UpdateUser", "Settings", "IsValid", "Errors", "UpdateComment" };
            _excludeMappingProps = propsToExcludeForMapping.ToDictionary();
        }



        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Type", "Header", "Active", "Tag" },
               columnProps: new List<Expression<Func<WidgetInstance, object>>>() { a => a.Id, a => a.DefName, a => a.Header, a => a.IsActive, a => a.EmbedTag },
               columnWidths: "");
        }


        /// <summary>
        /// This is the initial create page.
        /// </summary>
        /// <param name="widgettype"></param>
        /// <returns></returns>
        [AdminAuthorization]
        [HttpGet]
        public override ActionResult Create()
        {
            DashboardLayout(true);
            var widgettype = Request.Params["widgettype"];
            
            // Create the concrete type associated w/ widgettype and assign default values.
            var result = _helper.Create(() => WidgetHelper.CreateWithDefaultSettings(widgettype));
            return BuildActionResult(result, _viewSettings.PageLocationForCreate, onAfterViewModelCreated: (entityViewModel) =>
            {
                var formModel = entityViewModel as EntityFormViewModel;
                formModel.FormActionName = "Create";
                formModel.RouteValues = new { @widgettype = widgettype };
            });
        }


        /// <summary>
        /// Postback after filling data for widget.
        /// </summary>
        /// <returns></returns>
        [AdminAuthorization]
        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(WidgetInstance instance)
        {
            DashboardLayout(true);
            var widgettype = Request.Params["widgettype"];
            var widget = WidgetHelper.Create(widgettype);

            // NOTE: Since widgets extend from WidgetInstance, so there can be any properties.
            // not just the ones from WidgetInstance.
            // Custom UpdateModel support since the MVC base controller's UpdateModel doesn't handle interfaces/derived classes.
            MapperSupport.MapperWebForms.UpdateModel(widget, this.Request.Form, null, _excludeMappingProps);
            return base.Create(widget);
        }


        /// <summary>
        /// Edit the entity by passing in the form collection so it maps the 
        /// html fields to the entity properites.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="fetcher"></param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult Edit(WidgetInstance instance)
        {
            DashboardLayout(true);
            // 1. Get the entity.
            // 2. Load its state back into it's properties.
            // 3. Clearn any errors ( used to compatibility between In-Memory repo and real )
            // 4. Update it's data using the form
            var service = EntityRegistration.GetService<WidgetInstance>();
            var entity = service.Get(instance.Id);

            // NOTE: Since widgets extend from WidgetInstance, so there can be any properties.
            // not just the ones from WidgetInstance.
            // Custom UpdateModel support since the MVC base controller's UpdateModel doesn't handle interfaces/derived classes.
            MapperSupport.MapperWebForms.UpdateModel(entity, this.Request.Form, null, _excludeMappingProps);
            return base.Edit(entity);
        }


        /// <summary>
        /// Copies an existing entity.
        /// </summary>
        /// <returns></returns>
        // [Authorize]
        public override ActionResult Copy(int id)
        {
            var result = _helper.Copy(id);
            var actionResult = BuildActionResult(result, _viewSettings.PageLocationForCreate, onAfterViewModelCreated: (entityViewModel) =>
            {
                var formModel = entityViewModel as EntityFormViewModel;
                formModel.FormActionName = "Create";
                formModel.RouteValues = new { @widgettype = result.ItemAs<WidgetInstance>().DefName };
            });
            return actionResult;
        }


        /// <summary>
        /// Disables a widget
        /// </summary>
        /// <returns></returns>
        public ActionResult Modify(int id, Action<WidgetInstance> action)
        {
            var widget = _helper.FindById(id);
            if (!widget.IsOwnerOrAdmin())
                return Json(new { Success = false, Message = "Not authorized." }, JsonRequestBehavior.AllowGet);

            action(widget);
            widget.Save();
            if (widget.Errors.HasAny)
                return Json(new { Success = false, Message = widget.Errors.Message("<br/>") }, JsonRequestBehavior.AllowGet);

            string state = widget.IsActive ? "On" : "Off";
            return Json(new { Success = true, Message = "Widget has been turned " + state }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Save the ordering of the widgets.
        /// </summary>
        /// <param name="orderings">Delimited list of widget orderings id,zone,sortindex;id,zone,sortindex. e.g. 2,zoneright,3;4,zoneright;8 etc.</param>
        /// <returns></returns>
        public override ActionResult  Reorder(string orderings)
        {
            var result = WidgetHelper.SaveOrderings(orderings);
            return Json(new { Success = result.Success, Message = result.Message }, JsonRequestBehavior.AllowGet);
        }


        public override ActionResult Manage(int page = 1, int pagesize = 15)
        {
            DashboardLayout(true);
            return ManageUsingQuery(page, 100, null);
        }

    }
}
