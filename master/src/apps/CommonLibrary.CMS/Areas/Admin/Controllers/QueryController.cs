using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib.NamedQueries;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Core;


namespace ComLib.CMS.Areas.Admin
{
    /// <summary>
    /// Named Queries Controller.
    /// </summary>
    [AdminAuthorization]
    public class QueryController : EntityController<NamedQuery>
    {
        /// <summary>
        /// Execute the named Query and return a DataTable with the results.
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        public ActionResult ExecuteQuery(string queryName)
        {
            return View();
        }
    }
}
