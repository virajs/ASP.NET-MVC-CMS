using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using ComLib;
using ComLib.Authentication;
using ComLib.Data;
using ComLib.CaptchaSupport;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Services;
using ComLib.Web.Lib.Core;
using ComLib.Web.Modules.Services;


namespace ComLib.CMS.Controllers
{        
    /// <summary>
    /// http://www.codeproject.com/KB/web-security/formsroleauth.aspx
    /// 1. This allows to use custom membership/users/roles providers
    /// </summary>
    [HandleError]
    public class AccountController : CommonController
    {

        /// This constructor is used by the MVC framework to instantiate the controller using
        /// the default forms authentication and membership providers.
        public AccountController()
            : this(null, null)
        {
        }

        /// This constructor is not used by the MVC framework but is instead provided for ease
        /// of unit testing this type. See the comments in AccountModels.cs for more information.
        public AccountController(IAuthenticationService formsService, IMembershipService membershipService)
        {
            FormsService = formsService ?? new AuthenticationService();
            MembershipService = membershipService ?? new MembershipService();
        }


        /// <summary>
        /// Get set the form authentication service.
        /// </summary>
        public IAuthenticationService FormsService { get; private set; }


        /// <summary>
        /// Get / Set the membership service
        /// </summary>
        public IMembershipService MembershipService { get; private set; }
                

        /// <summary>
        /// Return the logon view page
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOn()
        {
            SetPageTitle("Logon");
            return View("LogOn");
        }


        /// <summary>
        /// Logon using the supplied credentials.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rememberMe"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings",
            Justification = "Needs to take same parameter type as Controller.Redirect()")]
        public ActionResult LogOn(LogOnModel model, bool rememberMe, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // This validates the user credentials and returns
                // a success flag and a string representing the roles of the user.
                string roles = string.Empty;
                BoolMessage result = MembershipService.ValidateUser(model.UserName, model.Password, ref roles);
                if (result.Success)
                {
                    FormsService.SignIn(model.UserName, roles, rememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        /// <summary>
        /// Return a new Registration view page.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            SetPageTitle("Register");
            if (Auth.IsAuthenticated())
                return RedirectToAction("home", "Console");
            return View();
        }

        
        /// <summary>
        /// Register a new user into the system.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Captcha.IsCorrect())
                {
                    ModelState.AddModelError("CaptchaUserInput", "Incorrect Verification Code.");
                    return View(model);
                }
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipCreateStatus.ProviderError;
                BoolMessage result = MembershipService.CreateUser(model.UserName, model.Password, model.Email, "User", ref createStatus);
                
                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, "User", false/* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string error = createStatus == MembershipCreateStatus.UserRejected ? result.Message : ErrorCodeToString(createStatus);
                    ModelState.AddModelError("", error);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [Authorize]
        public ActionResult Remove()
        {
            DashboardLayout(true);
            return View();
        }


        [Authorize]
        public ActionResult RemoveConfirmed(string username)
        {
            DashboardLayout(Auth.IsAuthenticated());
            BoolMessage result = MembershipService.DeleteUser(username);
            if (result.Success)
            {
                if (!Auth.IsAdmin())
                {
                    FormsService.SignOut();
                    DashboardLayout(false);
                }
                return View("Pages/GeneralMessage", new KeyValuePair<string, string>("Account Removed", "Account : " + username + " has been successfully removed."));
            }
            return View("Pages/GeneralMessage", new KeyValuePair<string, string>("Account Removed", result.Message));
        }


        /// <summary>
        /// Return view for changing password.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            DashboardLayout(true);
            return View();
        }


        /// <summary>
        /// Change the users password.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            DashboardLayout(true);
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        /// <summary>
        /// Return view page after changing password.
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePasswordSuccess()
        {
            DashboardLayout(true);
            return View();
        }


        /// <summary>
        /// Perform logoff the current user.
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult ForgotPassword()
        {
            SetPageTitle("Forgot Password");
            return View();
        }


        [HttpPost]
        public ActionResult ForgotPassword(FormCollection form)
        {
            string email = form["emailaddress"];
            string user = form["username"];
            BoolMessage result = MembershipService.SendPassword(user, email, true);
            if (!result.Success)
            {
                ModelState.AddModelError("emailaddress", result.Message);
                return View("ForgotPassword");
            }            
            this.ViewData["Message"] = "Your username/password have been mailed to : " + email;
            return View("ForgotPassword");
        }
 


        /// <summary>
        /// Enforce authentication type
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            // Guard.IsTrue(requestContext.HttpContext.User.Identity is WindowsIdentity, "Windows authentication is not supported.");
            base.Initialize(requestContext);
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            base.OnActionExecuting(filterContext);
        }


        /// <summary>
        /// Convert error code from MembershipProvider to a descriptive error.
        /// </summary>
        /// <param name="createStatus"></param>
        /// <returns></returns>
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://msdn.microsoft.com/en-us/library/system.web.security.membershipcreatestatus.aspx for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

    }
}
