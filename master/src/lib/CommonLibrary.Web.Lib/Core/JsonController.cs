using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;

using ComLib.Entities;
using ComLib.Data;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Helpers;


namespace ComLib.Web.Lib.Core
{

    /// <summary>
    /// Controller class that is Json/ajax based.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonController<T> : EntityController<T> where T : class, IEntity, new()
    {
        /// <summary>
        /// stores view information needed for json.
        /// </summary>
        protected JsonModelViewData _jsonView = new JsonModelViewData();


        /// <summary>
        /// Initialize the column names, properties, widths in one-go.
        /// </summary>
        /// <param name="columnNames"></param>
        /// <param name="columnProps"></param>
        /// <param name="columnWidths"></param>
        public void InitColumns(List<string> columnNames, List<Expression<Func<T, object>>> columnProps, string columnWidths = null, string customRowBuilder = "")
        {
            _jsonView.ColumnNames = columnNames;
            _jsonView.ColumnWidths = columnWidths;
            _jsonView.ColumnProps = new List<PropertyInfo>();
            _jsonView.CustomRowBuilder = customRowBuilder;
            for (int ndx = 0; ndx < columnProps.Count; ndx++)
            {
                string propname = ComLib.ExpressionHelper.GetPropertyName<T>(columnProps[ndx]);
                PropertyInfo prop = typeof(T).GetProperty(propname);
                _jsonView.ColumnProps.Add(prop);
            }
        }


        /// <summary>
        /// Save the sortindex of the items.
        /// </summary>
        /// <param name="orderings">Comma delimited text of id:sortindex pairs.
        /// e.g. "2:5,6:9" etc.</param>
        /// <returns></returns>
        public virtual ActionResult Reorder(string orderings)
        {
            var result = _helper.SaveOrdering(orderings);
            return Json(new { Success = result.Success, Message = result.Message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Turns on or off an item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult OnOff(int id)
        {
            var result = _helper.OnOff(id);
            return Json(new { Success = result.Success, Message = result.Message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Turns on or off an item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Clone(int id)
        {
            var result = _helper.Clone(id);
            return Json(new { Success = result.Success, Message = result.Message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Manage entities.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Manage(int page = 1, int pagesize = 15)
        {
            if (!Authentication.Auth.IsAuthenticated())
                return View(PageLocationForNotAuthorized);

            var model = BuildManageViewModel(page, pagesize);
            return View("Pages/manage2", model);
        }


        /// <summary>
        /// Find recent items in Json based format.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public virtual ActionResult ManageUsingJson(int page = 1, int pagesize = 15, string orderByColumn = "")
        {
            var result = _helper.ManageUsingJson(page, pagesize, _jsonView.ColumnProps, actionResultFetcher: (p, size) => _helper.FindByOwner(p, size, orderByColumn));
            string jsontext = result.ItemAs<string>();
            return Json(jsontext, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Find recent items in Json based format.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult FindByRecentUsingJson(int page = 1, int pagesize = 15)
        {
            var result = _helper.FindByAsJson(page, pagesize, _jsonView.ColumnProps);
            string jsontext = result.ItemAs<string>();
            return Json(jsontext, JsonRequestBehavior.AllowGet);
        }


        public override ActionResult Delete(int id)
        {
            var result = _helper.Delete(id);
            var message = result.Success ? _modelname + " has been deleted" : result.Message;
            return Json(new { Success = result.Success, Message = message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Builds the view model for the Manage page. This includes information such as the model name, column name, column property names, widths.
        /// </summary>
        /// <param name="page">The current page.</param>
        /// <param name="pagesize">The page size.</param>
        /// <returns></returns>
        public virtual JsonManageViewModel BuildManageViewModel(int page = 1, int pagesize = 15)
        {
            // Build up
            // 1. Column Names: "Id, Title, Author, Published Date, Url"
            // 2. Column Props: "Id, Title, CreateUser, PublishDate, Url"
            // 3. Column Width: "50, 200, 80, 80, 100"
            // 4. Column Types: "number,string,string,bool,date,string"
            string cols = _jsonView.ColumnNames.JoinDelimited(",", null);
            string colsmap = _jsonView.ColumnProps.JoinDelimited(",", p => p.Name);
            string coltypes = _jsonView.ColumnProps.JoinDelimited(",", p => p.PropertyType.Name);
            string width = _jsonView.ColumnWidths == null || _jsonView.ColumnWidths.Length == 0 ? string.Empty : _jsonView.ColumnWidths;

            // Determine functionality based on interfaces supported.
            bool isSortable = IsType(typeof(IEntitySortable));
            bool isActivatable = IsType(typeof(IEntityActivatable));
            bool isClonable = true;
            bool hasDetailsPage = _viewSettings.PageLocationForDetails.ToLower().Contains("details");
            return new JsonManageViewModel() { Name = _modelname, ColumnNames = cols, ColumnProps = colsmap, ColumnWidths = width, ColumnTypes = coltypes, 
                RowBuilder = _jsonView.CustomRowBuilder, IsSortable = isSortable, IsActivatable = isActivatable, IsCloneable = isClonable, HasDetailsPage = hasDetailsPage };
        }


        protected bool IsType(Type type)
        {
            return typeof(T).GetInterface(type.FullName) != null;
        }
    }



    /// <summary>
    /// Class for json managed models view information.
    /// </summary>
    public class JsonModelViewData
    {
        /// <summary>
        /// The name of columns to display for managing the entities.
        /// </summary>
        public List<string> ColumnNames;


        /// <summary>
        /// The properties of the entity data members associated w/ the same index in the columnNames.
        /// e.g. _columnsNames[2] = "Title" =  _columnProps[2] = entity.Title member expression.
        /// </summary>
        public List<PropertyInfo> ColumnProps;


        /// <summary>
        /// The widths of the columns as a comma delimited string. The order should match w/ the column names.
        /// </summary>
        public string ColumnWidths;


        /// <summary>
        /// Custom row builder for entity.
        /// </summary>
        public string CustomRowBuilder;
    }
}
