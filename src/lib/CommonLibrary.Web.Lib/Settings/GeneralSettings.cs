using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Configuration;


namespace ComLib.Web.Lib.Settings
{
    /// <summary>
    /// Site / General settings.
    /// </summary>
    public class GeneralSettings : ConfigSourceDynamic
    {
        /// <summary>
        /// Set default settings.
        /// </summary>
        public GeneralSettings()
        {
        }


        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string LogoPath { get; set; }
        public bool IsLogoEnabled { get; set; }
        public bool IsDashboardAutoDisplayedWhenLoggedIn { get; set; }
        public bool  StatsEnabled { get; set; }
        public string StatsAccountId { get; set; }
    }
}
