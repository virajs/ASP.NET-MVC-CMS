using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib.NamedQueries;
using ComLib.EmailSupport;
using ComLib.BootStrapSupport;
using ComLib.Extensions;
using ComLib.Environments;
using ComLib.Configuration;
using ComLib.Logging;
using ComLib.Notifications;
using ComLib.Authentication;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Services;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Areas.Admin
{
    /// <summary>
    /// Named Queries Controller.
    /// </summary>    
    public class ConsoleController : CommonController
    {
        /// <summary>
        /// Initialize w/ dashboard layout.
        /// </summary>
        public ConsoleController()
        {
            DashboardLayout(true);
        }


        /// <summary>
        /// Execute the named Query and return a DataTable with the results.
        /// </summary>
        /// <param name="queryName"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Home()
        {
            return View();
        }


        /// <summary>
        /// Gets System summary information and widget installed.
        /// </summary>
        /// <returns></returns>
        [AdminAuthorization]  
        public ActionResult GetInfo(string name)
        {
            var result = CMS.Info.GetInfoTask(name, true, (roles) => Auth.IsUserInRolesOrAdmin(roles));
            return View("Info", result);
        }


        [AdminAuthorization]
        public ActionResult Email()
        {
            var message = new EmailViewModel();
            var settings = EmailHelper.GetSettings(Config.Current, "EmailSettings");
            message.SmtpServer = settings.SmptServer;
            message.Port = settings.Port;
            message.From = Config.Get<string>("EmailSettings", "From");
            message.AuthUser = settings.AuthenticationUserName;
            
            return View(message);
        }


        [AdminAuthorization]
        [HttpPost]
        [ValidateInput(false)]        
        public ActionResult Email(EmailViewModel message)
        {
            if(message == null) return View("Email");
            
            BoolMessageEx result = Try.CatchLogReturnBoolResultEx("Email has been sent.", string.Empty, () =>
                {
                    EmailService service = new EmailService(Config.Current, "EmailSettings");
                    return service.Send(message, true, message.AuthUser, message.AuthPassword, message.SmtpServer, message.Port);
                });

            Flash(result);
            FlashMessages(message.ToString());
            return View("Email");
        }
    }



    /// <summary>
    /// ViewModel for sending an email.
    /// </summary>
    public class EmailViewModel : EmailMessage
    {
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string AuthUser { get; set; }
        public string AuthPassword { get; set; }


        public override string ToString()
        {
            string pass = string.IsNullOrEmpty(AuthPassword ) ? string.Empty : AuthPassword.Substring(AuthPassword.Length - 4);
            string result = string.Format("To: {0} " + Environment.NewLine +
                                          "From: {1} " + Environment.NewLine +
                                          "Subject: {2} " + Environment.NewLine +
                                          "Smtp: {3} " + Environment.NewLine +
                                          "Port: {4} " + Environment.NewLine +
                                          "AuthUser: {5} " + Environment.NewLine +
                                          "AuthPass: {6} " + Environment.NewLine, To, From, Subject, SmtpServer, Port, AuthUser, pass);
            return result;
        }
    }
}
