using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ComLib.Web.Lib.Extensions
{
    public static class MvcHtmlStringExtensions
    {
        /// <summary>
        /// Return the MvcHtmlString inside a span tag.
        /// </summary>
        /// <param name="htmlstr"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static string InSpan(this MvcHtmlString htmlstr, string classname)
        {
            string content = "<span class=\"" + classname + "\">" + htmlstr.ToHtmlString() + "</span>";
            return content;
        }


        /// <summary>
        /// Return the MvcHtmlString inside a div tag.
        /// </summary>
        /// <param name="htmlstr"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static string InDiv(this MvcHtmlString htmlstr, string classname)
        {
            string content = "<div class=" + classname + ">" + htmlstr.ToHtmlString() + "</div>";
            return content;
        }
    }
}
