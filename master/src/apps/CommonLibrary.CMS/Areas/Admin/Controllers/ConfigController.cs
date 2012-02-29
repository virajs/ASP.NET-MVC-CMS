using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;
using System.Reflection;

using ComLib.Configuration;
using ComLib.Data;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Models;
using ComLib.Web.Modules.Services;
using ComLib.Web.Modules.Settings;
using ComLib.Web.Modules.OptionDefs;
using ComLib.Web.Modules.Resources;


namespace ComLib.CMS.Areas.Admin
{
    [AdminAuthorization]
    public class ConfigController : Controller
    {

        /// <summary>
        /// Edit
        /// </summary>
        /// <returns></returns>
        public ViewResult Edit()
        {
            var config = GetSettings();
            return View("Edit", config);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(FormCollection form)
        {
            var config = GetSettings();
            IDictionary<string, string> excludeProps = null;

            // Not all the properties should be mapped from the source(form)
            if (config.Config is Configuration.ConfigSourceDynamic)
                excludeProps = ((Configuration.ConfigSourceDynamic)config.Config).ExcludedProps;

            // Custom UpdateModel support since the MVC base controller's UpdateModel doesn't handle interfaces.
            MapperSupport.MapperWebForms.UpdateModel(config.Config, this.Request.Form, null, excludeProps);

            // Save the config settings.
            config.Config.Save();

            // Load them again in UI.
            return View("Edit", config) ;
        }


        /// <summary>
        /// Gets the settings.
        /// </summary>
        /// <returns></returns>
        private ConfigViewModel GetSettings()
        {
            // Get the name of the config from either url params for from form.
            string configName = this.Request.Params["configname"];
            if (string.IsNullOrEmpty(configName))
                configName = this.Request.Form["confignameInForm"];

            var configData = CMS.Configs.Get(configName);
            var config = configData.Config as IConfigSourceDynamic;

            // This is the list of option definitions ( from the /config/data/optiondefs.csv ) file.
            var optionDefs = OptionDef.BySection(configName);

            // Resource file for the name of the setting, description, example values. These are localized.
            var resources = Resource.BySection(configName);
            return new ConfigViewModel() { Name = configName, Description = configData.Description,  Config = config, OptionDefs = optionDefs, Resources = resources };              
        }
    }



    public class ConfigViewModel
    {
        public string Name;
        public string Description;
        public IConfigSourceDynamic Config;
        public IList<OptionDef> OptionDefs;
        public IDictionary<string, Resource> Resources;
    }
}
