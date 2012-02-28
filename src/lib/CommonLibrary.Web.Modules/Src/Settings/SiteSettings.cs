using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Configuration;
using ComLib.EmailSupport;
using ComLib.Web.Lib.Settings;
using ComLib.Web.Modules.Posts;
using ComLib.Web.Modules.Events;


namespace ComLib.Web.Modules.Settings
{

    public class SiteSettings
    {
        #region Privates
        private static SiteSettings _instance;
        private static object _sync = new object();        
        #endregion

                
        public GeneralSettings Site { get; set; }
        public IEmailSettings Email { get; set; }
        public PostSettings Posts { get; set; }
        public EventSettings Events { get; set; } 
        


        /// <summary>
        /// Intialize your own version of the settings.
        /// </summary>
        /// <param name="settings"></param>
        public static void Init(SiteSettings settings)
        {
            lock (_sync)
            {
                if (_instance == null)
                {
                    _instance = settings;
                    _instance.Init();
                }
            }
        }


        /// <summary>
        /// Get the instance.
        /// </summary>
        public static SiteSettings Instance
        {
            get
            {
                if (_instance == null)
                    Init(new SiteSettings());

                return _instance;
            }
        }       


        /// <summary>
        /// Apply default settings.
        /// </summary>
        public SiteSettings()
        {
            Site = new GeneralSettings();
            Posts = Post.Settings;
            Events = Event.Settings;

            // Initialzie the heading settings.
            Site.Title = "CommonLibrary.NET Blog";
            Site.Description = "Using ASP.NET MVC 2";
            Site.IsLogoEnabled = false;
            Site.LogoPath = @"/Content/images/Common/logo.gif";
            Site.IsDashboardAutoDisplayedWhenLoggedIn = true;            
        }



        /// <summary>
        /// Intended for sub-classes to override settings.
        /// </summary>
        public virtual void Init()
        {
        }
    }
}
