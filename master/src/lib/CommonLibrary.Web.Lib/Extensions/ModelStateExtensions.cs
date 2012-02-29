using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

using ComLib;
using ComLib.Entities;


namespace ComLib.Web.Lib.Extensions
{
    /// <summary>
    /// Class to provided localized resource strings for labels.
    /// This is currently in place but using default data for now.
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Add errors from entity to the model state.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="entity"></param>
        public static void AddErrors(this ModelStateDictionary modelState, IErrors errors)
        {
            if (errors.Count > 0)
            {
                errors.EachFull(err => modelState.AddModelError("", err));
            }
        }
    }
}
