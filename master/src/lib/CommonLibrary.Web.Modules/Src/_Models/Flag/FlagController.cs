using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComLib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.CaptchaSupport;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Modules.Flags
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class FlagController : EntityController<Flag>
    {
        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model name. e.g. "Post" or "Event".</param>
        /// <param name="refid">The refid or id of the post being favored.</param>
        /// <param name="title">The title. The title of the post.</param>
        /// <param name="url">The URL of the post.</param>
        [HttpGet]
        public ActionResult Add(string model, int refid, string title, string url)
        {
            // This is called asychronously via JSON.
            // No need to return anything although a success/failure message would be good.
            Flag flag = new Flag() { Model = model, RefId = refid, Title = title, Url = url, FlagType = 1 };
            flag.Create();
            bool success = !flag.Errors.HasAny;
            string message = success ? model + " has been flagged for review." : "Unable to report " + model + ". " + flag.Errors.Message("<br/>");
            return Json(new { Success = success, Message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}
