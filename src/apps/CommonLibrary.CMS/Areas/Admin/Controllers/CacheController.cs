using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib.Caching;
using ComLib.Web.Lib.Attributes;


namespace ComLib.CMS.Areas.Admin
{
    [AdminAuthorization]
    public class CacheController : Controller
    {
        /// <summary>
        /// Get list of all the entries in the Cache.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = Cacher.GetDescriptors();
            return View("List", model);
        }


        /// <summary>
        /// Remove the cache entry with the supplied name.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public ActionResult Remove(string cacheKey)
        {
            Cacher.Remove(cacheKey);
            return Index();
        }


        /// <summary>
        /// Remove all the entries in the Cache.
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveAll()
        {
            Cacher.RemoveAll(Cacher.Keys);
            return Index();
        }
    }
}
