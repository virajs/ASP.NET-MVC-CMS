using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Helpers
{
    public static class UrlHelperExtensions
    {
        private static IDictionary<string, bool> _adminAreas = new Dictionary<string, bool>();

        
        public static IDictionary<string, bool> AdminAreas { get { return _adminAreas; } }


        /// <summary>
        /// Generates the link for the url action/controller combination.
        /// </summary>
        /// <param name="urlhelper"></param>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string Link(this System.Web.Mvc.UrlHelper urlhelper, string action, string controller, object routeValues)
        {
            string baseurl = _adminAreas.ContainsKey(controller.ToLower()) ? "/admin" : string.Empty;
            string url = baseurl + "/" + controller + "/" + action + "/";                
            return url;
        }


        /// <summary>
        /// Builds the link for a media folder that's associated with an entity.
        /// </summary>
        /// <param name="urlhelper">UrlHelper</param>
        /// <param name="action">Action on the controller.</param>
        /// <param name="controller">Controller</param>
        /// <param name="id">Entity Id</param>
        /// <param name="refgroup">Entity Group ( e.g. post or event )</param>
        /// <returns></returns>
        public static string LinkForMediaForEntity(this System.Web.Mvc.UrlHelper urlhelper, string action, string controller, int id, int refgroup)
        {
            string baseurl = _adminAreas.ContainsKey(controller.ToLower()) ? "/admin" : string.Empty;
            string url = baseurl + "/" + controller + "/" + action + "?id=" + id + "&refgroup=" + refgroup;
            return url;
        }
    }
}
