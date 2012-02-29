using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComLib.Data;
using ComLib.Web.Lib.Core;


namespace ComLib.Web.Modules.Parts
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class PartController : JsonController<Part>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Title", "Desc", "Roles" },
               columnProps: new List<Expression<Func<Part, object>>>() { a => a.Id, a => a.Title, a => a.Description, a => a.AccessRoles },
               columnWidths: null);
        }
    }
}
