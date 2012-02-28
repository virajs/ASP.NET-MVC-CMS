using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Account;


namespace ComLib.Web.Lib.Models
{    
    
    /// <summary>
    /// View model used for showing the List/index page for entities.
    /// </summary>
    public class EntityListViewModel : EntityBaseViewModel
    {

        /// <summary>
        /// The current page index.
        /// </summary>
        public int PageIndex;


        /// <summary>
        /// Total pages available.
        /// </summary>
        public int TotalPages;


        /// <summary>
        /// Whether or not to show the Edit/Delete links.
        /// </summary>
        public bool ShowEditDelete;
    }



    /// <summary>
    /// Typed Pager View model with the actual PagedList of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityListViewModel<T> : EntityListViewModel
    {
        /// <summary>
        /// List of items for this specific page.
        /// </summary>
        public PagedList<T> Items;


        /// <summary>
        /// Default initialization.
        /// </summary>
        public EntityListViewModel() : this(null, string.Empty, false)
        {
        }


        /// <summary>
        /// Initialize w/ a pagedlist and the url.
        /// </summary>
        /// <param name="pagedData"></param>
        /// <param name="url"></param>
        public EntityListViewModel(PagedList<T> pagedData, string urlIndex, bool showEditDelete)
        {
            PageIndex = 1;
            TotalPages = 1;
            if (pagedData == null)
                Items = PagedList<T>.Empty;
            else
            {
                Items = pagedData;
                PageIndex = pagedData.PageIndex;
                TotalPages = pagedData.TotalPages;
            }
            UrlIndex = urlIndex;
            ShowEditDelete = showEditDelete;
        }
    }



    /// <summary>
    /// This is used to develop the "copy", "edit", "delete" display behaviour on the manage pages.
    /// </summary>
    public class EntityListManageViewModel
    {
        public int Id;
        public object Item;
        public EntityBaseViewModel ViewInfo;
        public bool ShowDelete = true;
        public bool ShowEdit = true;
        public bool ShowCopy = true;

        
        /// <summary>
        /// Gets the URL copy.
        /// </summary>
        /// <value>The URL copy.</value>
        public string UrlCopy
        {
            get { return ViewInfo.UrlCopy + Id; }
        }


        /// <summary>
        /// Gets the URL edit.
        /// </summary>
        /// <value>The URL edit.</value>
        public string UrlEdit
        {
            get { return ViewInfo.UrlEdit + Id; }
        }


        /// <summary>
        /// Gets the URL delete.
        /// </summary>
        /// <value>The URL delete.</value>
        public string UrlDelete
        {
            get { return ViewInfo.UrlDelete + Id; }
        }
    }
}
