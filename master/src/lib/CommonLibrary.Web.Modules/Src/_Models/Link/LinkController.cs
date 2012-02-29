using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComLib.Web.Lib.Core;


namespace ComLib.Web.Modules.Links
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class LinkController : JsonController<Link>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Name", "Group", "Url", "Sort" },
               columnProps: new List<Expression<Func<Link, object>>>() { a => a.Id, a => a.Name, a => a.Group, a => a.Url, a => a.SortIndex },
               columnWidths: "25,200,125,150,40");
        }
    }
}
