using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib.Queue;
using ComLib.Web.Lib.Attributes;


namespace ComLib.CMS.Areas.Admin
{
    [AdminAuthorization]
    public class QueueController : Controller
    {
        /// <summary>
        /// Get list of all the entries in the Cache.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = Queues.GetMetaInfo();
            return View("List", model);
        }
    }
}
