using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Collections;

using ComLib.Extensions;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Settings;
using ComLib.Web.Lib.Services;


namespace ComLib.Web.Lib.Core
{

    public class CommonController : Controller
    {
        protected static Func<GeneralSettings> _settingsHandler;
        protected static Action<string, Exception> _errorHandler;
        protected static IMediaService _mediaFileHandler;
        protected static Func<IDictionary> _modelSettingsFetcher;
        protected static Func<EntitySettingsHelper> _entitySettingsHandler;
        private bool _enableMasterPageTemplates = true;
        private bool _useDashBoardLayout;
        private bool _forceTheme = false;
        

        /// <summary>
        /// Sets the error handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public static void SetErrorHandler(Action<string, Exception> handler)
        {
            _errorHandler = handler;
        }


        /// <summary>
        /// Sets the error handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public static void SetMediaHandler(IMediaService handler)
        {
            _mediaFileHandler = handler;
        }


        /// <summary>
        /// Set the handler that gets the instance of the general settings.
        /// </summary>
        /// <param name="handler"></param>
        public static void SetSettingsHandler(Func<GeneralSettings> handler)
        {
            _settingsHandler = handler;
        }


        /// <summary>
        /// Set the handler that gets the instance of the general settings.
        /// </summary>
        /// <param name="handler"></param>
        public static void SetModelSettingsHandler(Func<IDictionary> handler)
        {
            _modelSettingsFetcher = handler;
        }


        public static void SetEntitySecurityHandler(Func<EntitySettingsHelper> handler)
        {
            _entitySettingsHandler = handler;                
        }


        /// <summary>
        /// Set the title of the page.
        /// </summary>
        /// <param name="title"></param>
        public virtual void SetPageTitle(string title)
        {
            if (_settingsHandler == null)
                this.HttpContext.Items["cms-page-title"] = title;
            else
            {
                var settings = _settingsHandler();
                string suffix = settings == null ? string.Empty : settings.Title;
                this.HttpContext.Items["cms-page-title"] = title + " - " + suffix;
            }
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        protected virtual void Init()
        {
        }


        #region Flash Messages / Errors
        /// <summary>
        /// Flashes an error or a messages depending on success/fail flag in result.
        /// </summary>
        /// <param name="result"></param>
        public void Flash(BoolMessage result)
        {
            if (result == null) return;
            if (result.Success)
                FlashMessages(result.Message);
            else
                FlashErrors(result.Message);
        }


        /// <summary>
        /// Flash an error from the message in <paramref name="result"/> if result is not successful.
        /// </summary>
        /// <param name="result">The boolean message result</param>
        public void FlashIfError(BoolMessage result)
        {
            if (result != null && !result.Success)
                FlashErrors(result.Message);
        }


        /// Flash the message and the exception message in <paramref name="result"/> if result is not successful.
        /// </summary>
        /// <param name="result">The boolean message result</param>
        public void FlashIfError(BoolMessageEx result)
        {
            if (result != null && !result.Success)
                FlashErrors(result.Message, result.Ex == null ? string.Empty : result.Ex.Message);
        }
        

        /// <summary>
        /// Flash all the errors provided.
        /// </summary>
        /// <param name="errors"></param>
        public void FlashErrors(params string[] errors)
        {
            if (errors == null || errors.Length == 0) return;
            foreach (string error in errors) 
                this.ModelState.AddModelError("", error);
        }


        /// <summary>
        /// Flash all the errors provided.
        /// </summary>
        /// <param name="errors"></param>
        public void FlashErrors(IErrors errors)
        {
            if (errors == null || errors.Count == 0) return;
            errors.EachFull(error => this.ModelState.AddModelError("", error));
        }


        /// <summary>
        /// Flash all the errors provided.
        /// </summary>
        /// <param name="errors"></param>
        public void FlashErrors(IList<string> errors)
        {
            if (errors == null || errors.Count == 0) return;
            foreach (string error in errors) 
                this.ModelState.AddModelError("", error);
        }


        /// <summary>
        /// Flash all the messages provided.
        /// </summary>
        /// <param name="messages"></param>
        public void FlashMessages(params string[] messages)
        {
            var messagemap = this.HttpContext.Items.Get<Messages>("flashmessages");
            if (messagemap == null)
            {                
                messagemap = new Messages();
                this.HttpContext.Items["flashmessages"] = messagemap;
            }

            foreach (string message in messages) messagemap.Add(message);
        }
        #endregion


        /// <summary>
        /// Gets or sets a value indicating whether [use dashboard layout].
        /// </summary>
        /// <value><c>true</c> if [use dashboard layout]; otherwise, <c>false</c>.</value>
        public void DashboardLayout(bool useDashboardLayout)
        {
            _useDashBoardLayout = useDashboardLayout;
        }


        public void EnablePageLayout(bool enable)
        {
            _enableMasterPageTemplates = enable;
        }


        public void ForceTheme(bool enable)
        {
            _forceTheme = enable;
        }


        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                string url = this.Request.Url.AbsoluteUri;
                string hostname = this.Request.UserHostAddress;
                string message = filterContext.Exception.Message;
                string error = string.Format("Error :{0}, HostName :{1}, Url :{2}", message, hostname, url);
                if(_errorHandler != null)
                    _errorHandler(error, filterContext.Exception);

                base.OnException(filterContext);
            }
            catch (Exception) { }
        }


        /// <summary>
        /// Called after the action method is invoked. This allow changing the masterpage/layout dynamically.
        /// </summary>
        /// <param name="filterContext">Information about the current request and action.</param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (_enableMasterPageTemplates)
            {
                string overrideLayout = "Site.Dashboard";
                ViewHelper.SetMasterPage(filterContext, this.ControllerContext, _useDashBoardLayout, overrideLayout, _forceTheme);
                base.OnActionExecuted(filterContext);
            }
        }
    }
}
