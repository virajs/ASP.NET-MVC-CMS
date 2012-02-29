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
    /// View model used for displaying an entity details inside another content item.
    /// The content item("contentitem.asc") will supply the "edit"/"delete" links for the entity.
    /// </summary>
    public class EntityBaseViewModel
    {
        public string ControlPath;
        public string ControllerName;
        public string Name;
        public string UrlManage;
        public string UrlCreate;
        public string UrlIndex;
        public string UrlCopy;
        public string UrlEdit;
        public string UrlDelete;
        public string UrlCancel;
        public string UrlBack;
        public bool AllowEdit;
        public bool AllowDelete;
        public IDictionary Config;
    }
}
