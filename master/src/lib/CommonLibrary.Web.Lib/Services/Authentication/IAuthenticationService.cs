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


namespace ComLib.Web.Lib.Services
{
    /// <summary>
    /// Forms Authentication Interface.
    /// </summary>
    public interface IAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignIn(string userName, string roles, bool createPersistentCookie);
        void SignOut();
    }
}
