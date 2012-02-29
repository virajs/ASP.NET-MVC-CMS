using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using ComLib;
using ComLib.Web.Services.Search;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Services;
using ComLib.Web.Lib.Core;

namespace ComLib.CMS.Controllers
{
    /// <summary>
    /// Search site / web
    /// </summary>
    [HandleError]
    public class SearchController : CommonController
    {

        /// <summary>
        /// Searchs the site/web for the keywords specified.
        /// </summary>
        /// <returns></returns>
        public ActionResult DoOpenSearch(string keywords, string searchtype, int pagenumber)
        {
            bool isWeb = !string.IsNullOrEmpty(searchtype) && string.Compare(searchtype, "web", true) == 0;
            if (string.Compare(searchtype, "undefined", true) == 0) isWeb = true;

            if (pagenumber <= 0) pagenumber = 1;

            if (string.IsNullOrEmpty(keywords))
            {
                var result = Json(new { Success = false, Message = "No keywords specified to search.", Items = string.Empty }, JsonRequestBehavior.AllowGet);
                return result;
            }

            PagedList<SearchResult> results = null;
            string localsite = this.Request.Url.Authority;

            if (isWeb || localsite.Contains("localhost"))
                results = (PagedList<SearchResult>)SearchEngine.Search(keywords, pagenumber, 15, string.Empty);
            else
            {
                results = (PagedList<SearchResult>)SearchEngine.SearchSite(keywords, localsite, pagenumber, 15, string.Empty);
            }

            bool success = results != null && results.Count > 0;
            var json = Json(new { Success = success, Message = string.Empty, TotalRecords = results.TotalCount, TotalPages = results.TotalPages, Items = results }, JsonRequestBehavior.AllowGet);
            return json;
        }


        /// <summary>
        /// Displays the search view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            string keywords = this.Request.Params["keywords"];
            string searchType = this.Request.Params["searchtype"];

            bool autosearch = string.IsNullOrEmpty(keywords) ? false : true;
            bool isWeb = !string.IsNullOrEmpty(searchType) && string.Compare(searchType, "web", true) == 0;

            this.ViewData["autosearch"] = autosearch;           
            if (autosearch)
            {
                // Escape the keywords for javascript.
                keywords = keywords.Replace("'", "\'");
                this.ViewData["searchkeywords"] = keywords;
                this.ViewData["searchtype"] = searchType;
            }

            return View();
        }
    }
}
