using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Text;

using ComLib;
using ComLib.Patterns;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.ViewModels;
using ComLib.Web.Lib.Helpers;

namespace ComLib.Web.Modules.Categorys
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class CategoryController : JsonController<Category>
    {

        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Name", "Group", "Parent", "Sort #" },
               columnProps: new List<Expression<Func<Category, object>>>() { a => a.Id, a => a.Name, a => a.Group, a => a.ParentId, a => a.SortIndex },
               columnWidths: null);
        }
    }
}
