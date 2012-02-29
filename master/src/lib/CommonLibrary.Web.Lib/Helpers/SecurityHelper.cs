using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;


using ComLib;
using ComLib.Authentication;


namespace ComLib.Web.Lib.Helpers
{
    public class SecurityHelper
    {
        /// <summary>
        /// Authenticate the request.
        /// Using Custom Forms Authentication since I'm not using the "RolesProvider".
        /// Roles are manually stored / encrypted in a cookie in <see cref="FormsAuthenticationService.SignIn"/>
        /// This takes out the roles from the cookie and rebuilds the Principal w/ the decrypted roles.
        /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void RebuildUserFromCookies()
        {
            string cookieName = FormsAuthentication.FormsCookieName;
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];
            if (null == authCookie) return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            String[] roles = authTicket.UserData.Split(new char[] { ',' });

            // Create an Identity object
            IIdentity id = new UserIdentity(0, authTicket.Name, "Forms", true);
            IPrincipal principal = new UserPrincipal(0, authTicket.Name, roles, id);

            // Attach the new principal object to the current HttpContext object
            HttpContext.Current.User = principal;
        }


        /// <summary>
        /// Stores user data inside a cookie.
        /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
        /// </summary>
        /// <param name="username"></param>
        /// <param name="expireTimeInMinutes"></param>
        /// <param name="roles"></param>
        public static void SetCookieContainingUserData(string userName, string roles, int expireTimeInMinutes)
        {
            // Create the authetication ticket
            FormsAuthenticationTicket authTicket =
                new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(30), true, roles);

            // Now encrypt the ticket.
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            // Create a cookie and add the encrypted ticket to the cookie as data.
            HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            // Add the cookie to the outgoing cookies collection.
            HttpContext.Current.Response.Cookies.Add(authCookie);   
        }
    }
}
