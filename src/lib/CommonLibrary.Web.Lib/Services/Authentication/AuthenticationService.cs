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
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Services;


namespace ComLib.Web.Lib.Services
{

    /// <summary>
    /// Forms Authentication Implementation
    /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        /// <summary>
        /// Sign in issuing the Forms Authentication Ticket.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(string userName, bool createPersistentCookie)
        {
            SignIn(userName, string.Empty, createPersistentCookie);
        }


        /// <summary>
        /// Sign in issuing the Forms Authentication Ticket.
        /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(string userName, string roles, bool createPersistentCookie)
        {
            SecurityHelper.SetCookieContainingUserData(userName, roles, 30);     
        }


        /// <summary>
        /// Sign out.
        /// </summary>
        public void SignOut()
        {
            FormsAuthentication.SignOut();            
        }
    }
}
