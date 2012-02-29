using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

using ComLib.Entities;
using ComLib.Authentication;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Lib.Helpers
{
    public class PagerHelper
    {
        /// <summary>
        /// Build pager view model from a non-paged list of items.
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static EntityListViewModel<T> ConvertToListViewModel<T>(System.Web.Mvc.UrlHelper urlhelper, IList<T> items, string controllerName, IDictionary conf) where T : IEntity
        {
            if (string.IsNullOrEmpty(controllerName))
                controllerName = typeof(T).Name;

            // Url for pager.
            // This List page uses a paged result by default.
            // Since we are getting ALL the posts with matching tags, just wrap them in a paged result.
            string url = string.Format("/{0}/index/{1}", controllerName, 1);

            // Check for empty.
            if (items == null)
            {
                var result = new EntityListViewModel<T>(PagedList<T>.Empty, url, false);
                result.ControlPath = "ModelList";
            }

            PagedList<T> pagedList = new PagedList<T>(1, items.Count + 1, items.Count, items);
            var viewModel = new EntityListViewModel<T>(pagedList, url, false);
            ViewHelper.BuildViewModel<T>(urlhelper, viewModel, default(T), controllerName, "ModelList", false, conf);
            viewModel.Items = pagedList;
            viewModel.PageIndex = pagedList.PageIndex;
            viewModel.TotalPages = pagedList.TotalPages;
            viewModel.ShowEditDelete = false;
            return viewModel;
        }
    }
}
