using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComLib.Authentication;


namespace ComLib.Web.Lib.Attributes
{

    public class AdminAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Auth.IsAdmin();
        }
    }
}
