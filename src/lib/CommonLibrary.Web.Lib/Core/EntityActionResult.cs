using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Authentication;
using ComLib.Web.Lib.Models;
using ComLib.Data;


namespace ComLib.Web.Lib.Core
{
    public class EntityActionResult
    {
        /// <summary>
        /// Initialize the entity action result.
        /// </summary>
        /// <param name="success"></param>
        /// <param name="message"></param>
        /// <param name="viewName"></param>
        /// <param name="errors"></param>
        /// <param name="item"></param>
        public EntityActionResult(bool success, string message = "", IErrors errors = null, object item = null, bool isAuthorized = true, bool isAvailable = true)
        {
            Success = success;
            Message = message;
            Errors = errors;
            IsAuthorized = isAuthorized;
            IsAvailable = isAvailable;
            Item = item;
        }


        /// <summary>
        /// Whether or not the action being requested/done is successful.
        /// </summary>
        public bool Success;


        /// <summary>
        /// Any error message associated w/ the action.
        /// </summary>
        public string Message;


        /// <summary>
        /// Collection of errors
        /// </summary>
        public IErrors Errors;


        /// <summary>
        /// The object(s) being retrieved.
        /// </summary>
        public object Item;


        /// <summary>
        /// Whether or not the action is authorized. 
        /// e.g. A user can not edit/access another users data.
        /// Or some other security / permissions rules are violated.
        /// </summary>
        public bool IsAuthorized;


        /// <summary>
        /// Whether or not the action is available. 
        /// e.g. if an entity is deleted and not available, this is true.
        /// </summary>
        public bool IsAvailable;


        /// <summary>
        /// Gets the items as type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ItemAs<T>() where T: class
        {
            return Item as T; 
        }
    }

}
