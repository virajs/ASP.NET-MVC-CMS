using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Web.Lib.Models;
using ComLib.Data;


namespace ComLib.Web.Lib.Core
{
    public class EntityJsonHelper
    {
        /// <summary>
        /// Builds the json result for managing the entities.
        /// </summary>
        /// <param name="success">Whether or not the fetching of the entities was succesful.</param>
        /// <param name="message">Status message or error message depending on the success.</param>
        /// <param name="items">The paged items</param>
        /// <returns>JSON result</returns>
        public static string BuildManageJsonResult<T>(bool success, string message, PagedList<T> items, string itemsText, Func<string, string> encoder)
        {

            List<T> entities = items;
            StringBuilder buffer = new StringBuilder();
            buffer.Append("{ ");
            buffer.Append(" \"Success\" : " + (success ? "true" : "false"));
            if(encoder == null)
                buffer.Append(" , \"Message\" : \"" + HttpContext.Current.Server.HtmlEncode(message) + "\"");
            else
                buffer.Append(" , \"Message\" : \"" + encoder(message) + "\"");

            buffer.Append(" , \"Page\" : " +  items.PageIndex);
            buffer.Append(" , \"PageSize\" : " + items.PageSize);
            buffer.Append(" , \"TotalPages\" : " + items.TotalPages);
            buffer.Append(" , \"Items\" : [ " + itemsText + " ] ");
            buffer.Append(" }");
            string json = buffer.ToString();
            return json;
        }
    }
}
