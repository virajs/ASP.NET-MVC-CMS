using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Modules.Services;
using ComLib.Web.Lib.Core;
using ComLib.ImportExport;


namespace ComLib.CMS.Controllers
{

    [HandleError]
    [Authorize]
    public class ToolsController : CommonController
    {
        public ToolsController()
        {
            DashboardLayout(true);
        }


        /// <summary>
        /// Returns the view for exporting data.
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            DashboardLayout(true);
            return View("Pages/FeatureNya");
        }


        /// <summary>
        /// Does the export for a specific model.
        /// </summary>
        /// <param name="entityname">The entityname.</param>
        /// <param name="format">The format.</param>
        /// <param name="all">if set to <c>true</c> [all].</param>
        /// <param name="batchsize">The batchsize.</param>
        /// <returns></returns>
        public ActionResult DoExport(string entityname, string format, bool all, int batchsize)
        {
            DashboardLayout(true);
            return View("Pages/FeatureNya");
        }


        /// <summary>
        /// Displays the ui for importing data for a specific model.
        /// </summary>
        /// <returns></returns>
        [AdminAuthorization]
        public ActionResult Import()
        {            
            IList<string> importables = CMS.Dashboard.ImportablesFor(true, "*");
            return View(importables);
        }


        /// <summary>
        /// Does the actual import of the data using the textual import data specified by <paramref name="content"/>
        /// </summary>
        /// <param name="content">The textual data representing the models to import.</param>
        /// <param name="model">The name of the model being imported.</param>
        /// <param name="format">The format of the data. e.g. csv,ini,xml,json,yml. Only ini is supported for now.</param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorization]
        public ActionResult DoImport()
        {
            string content = this.Request.Params["Content"];
            string model = this.Request.Params["Model"];
            string format = this.Request.Params["Format"];
            var io = ImportExports.Instance;
            var actualModelName = CMS.Dashboard.Models.ModelNameForAlias(model);
            var result = io.For(actualModelName).ImportTextAsObjects(content, "ini");
            if (!result.Success)
            {
                string finalError = BuildFriendlyError(result.Errors);
                finalError = "There were errors during the import, data was not imported. Please correct errors and try again. "
                           + " Click on the example button below to display and refer to the schema and example. <br/>" 
                           + finalError;
                return Json(new { Success = false, Message = finalError }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = true, Message = "Successfully imported data" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get a sample import file content.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ImportExampleFor(string model)
        {
            return ImportSampleFor(model, false);
        }


        /// <summary>
        /// Get a sample import schema content.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ImportSchemaFor(string model)
        {
            return ImportSampleFor(model, true);
        }


        private ActionResult ImportSampleFor(string model, bool isSchema)
        {
            var actualModelName = CMS.Dashboard.Models.ModelNameForAlias(model);
            string filepath = isSchema ? string.Format("/config/examples/import_{0}_schema.htm", actualModelName.ToLower())
                                       : string.Format("/config/examples/import_{0}_example.htm", actualModelName.ToLower());
            // Exists?
            filepath = Server.MapPath(filepath);
            if (!System.IO.File.Exists(filepath))
                return Json(new { Success = false, Message = "Example not available", Data = string.Empty }, JsonRequestBehavior.AllowGet);

            string content = System.IO.File.ReadAllText(filepath);

            // Return the Success / Message fields as there is
            return Json(new { Success = true, Message = string.Empty, Data = content }, JsonRequestBehavior.AllowGet);
        }



        private static string BuildFriendlyError(IErrors errors)
        {
            var buffer = new StringBuilder();
            errors.EachFull(err =>
            {
                if (!string.IsNullOrEmpty(err))
                {
                    if (err.StartsWith("Item #"))
                        buffer.Append("<br/>");

                    buffer.Append(err + "<br/>");
                }
            });
            return buffer.ToString();
        }
    }
}
