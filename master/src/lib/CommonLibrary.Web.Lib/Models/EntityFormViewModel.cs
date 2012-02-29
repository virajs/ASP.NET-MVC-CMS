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
    public class EntityFormViewModel : EntityBaseViewModel
    {
        public IEntity Entity;
        public string FormActionName;
        public object RouteValues;
    }



    /// <summary>
    /// Typed versiion of the EntityDetailsViewModel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityFormViewModel<T> : EntityFormViewModel where T : IEntity
    {
        public T EntityTyped;

        public EntityFormViewModel()
        {
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entity"></param>
        /// <param name="editUrl"></param>
        /// <param name="deleteUrl"></param>
        /// <param name="allowEdit"></param>
        /// <param name="allowDelete"></param>
        public EntityFormViewModel(string controlPath, T entity, string editUrl, string deleteUrl, string indexUrl, bool allowEdit, bool allowDelete)
        {
            ControlPath = controlPath;
            Entity = entity;
            EntityTyped = entity;
            UrlEdit = editUrl;
            UrlDelete = deleteUrl;
            UrlIndex = indexUrl;
            AllowEdit = allowEdit;
            AllowDelete = allowDelete;
        }
    }
}
