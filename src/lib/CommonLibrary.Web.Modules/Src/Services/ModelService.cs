using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using ComLib;
using ComLib.Extensions;
using ComLib.Web.Lib.Extensions;
using ComLib.Web.Modules.Resources;
using ComLib.Web.Modules.OptionDefs;


namespace ComLib.Web.Modules.Services
{
    /// <summary>
    /// Wrapper class around a model to encapsulate 
    /// 1. Dynamic property value retrieval
    /// 2. Getting description of the model properties
    /// 3. Getting examples for the model properties
    /// </summary>
    public class ModelService
    {
        private static IDictionary<string, string> _propsToExclude;
        
        private object _model;
        private HtmlHelper _htmlHelper;
        private IDictionary<string, OptionDef> _optionDefsMap;
        private IDictionary<string, PropertyInfo> _props;
        private IDictionary<string, Resource> _resources;
        private IList<OptionDef> _optionDefs;
        private IList<string> _propNames;
        private string _language = "english";
        private bool _hasResources;
        private bool _hasOptionDefs;
        private IDictionary<string, Func<MvcHtmlString>> _overrides = new Dictionary<string, Func<MvcHtmlString>>();
        private IDictionary<string, string> _excludes;

        /// <summary>
        /// Initializes the <see cref="ModelEditor"/> class.
        /// </summary>
        static ModelService()
        {
            _propsToExclude = new Dictionary<string, string>()
            {
                { "Id", "Id" },
                { "CreateDate", "CreateDate" },
                { "UpdateDate", "UpdateDate" },
                { "CreateUser", "CreateUser" },
                { "UpdateUser", "UpdateUser" },
                { "Version", "Version" },
                { "VersionRefId", "VersionRefId" },
                { "Errors", "Errors" },
                { "IsValid", "IsValid" }
            };            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptiveModel"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="helper">The helper.</param>
        public ModelService(object model, HtmlHelper helper)
        {
            _model = model;
            _htmlHelper = helper;
        }


        /// <summary>
        /// The properties that can be displayed.
        /// </summary>
        public IList<string> Properties
        {
            get { return _propNames; }
        }


        /// <summary>
        /// Loads the specified config section.
        /// </summary>
        /// <param name="configSection">The config section. This can be comma delimited. "Post"</param>
        /// <param name="resourceSection">The resource sections. This can be comma delimited. "Post"</param>
        /// <param name="language">The language. e.g. "english"</param>
        public void Load( string configSection, string resourceSection, string language)
        {
            _language = language;

            // This is the list of option definitions ( from the /config/data/optiondefs.csv ) file.
            IList<OptionDef> optionDefs = null;
            if (configSection.Contains(","))
            {
                string[] configNames = configSection.Split(',');
                optionDefs = OptionDef.BySection(configNames);
            }
            else
                optionDefs = OptionDef.BySection(configSection);

            bool hasInstanceExcludes = _excludes != null;
            _hasOptionDefs = optionDefs != null && optionDefs.Count > 0;
            _propNames = new List<string>();

            // This can be optimzed.
            ToDo.Optimize(ToDo.Priority.Normal, "Kishore", "The optiondefs, propsToExclude, resources can be cached.", () =>
            {
                if (_hasOptionDefs)
                {
                    _optionDefs = new List<OptionDef>();
                    _optionDefsMap = new Dictionary<string, OptionDef>();             
                    foreach (var optionDef in optionDefs)
                    {
                        // Only store if ok to process the option.
                        if (!_propsToExclude.ContainsKey(optionDef.Key) && (!hasInstanceExcludes || (hasInstanceExcludes && !_excludes.ContainsKey(optionDef.Key))))
                        {
                            _optionDefsMap[optionDef.Key] = optionDef;
                            _optionDefs.Add(optionDef);
                            _propNames.Add(optionDef.Key);
                        }
                    }
                }
                else
                {                    
                    foreach (var pair in _props)
                    {
                        // Only store if ok to process the option.
                        if (!_propsToExclude.ContainsKey(pair.Key) && (!hasInstanceExcludes || (hasInstanceExcludes && !_excludes.ContainsKey(pair.Key))))
                        {
                            _propNames.Add(pair.Key);
                        }
                    }
                }
            });

            // Resource file for the name of the setting, description, example values. These are localized.
            if (resourceSection.Contains(","))
            {
                string[] resourceNames = resourceSection.Split(',');
                _resources = Resource.BySection(resourceNames);
            }
            else
                _resources = Resource.BySection(resourceSection);

            _hasResources = _resources != null && _resources.Count > 0;

            // Loading all the public/instance/get-set properties.
            LoadProperties(_model);
        }



        /// <summary>
        /// Override property name with a specific builder.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="builder"></param>
        public void Exclude(string propertyName)
        {
            if (_excludes == null) 
                _excludes = new Dictionary<string, string>();

            _excludes[propertyName] = propertyName;
        }


        /// <summary>
        /// Override property name with a specific builder.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="builder"></param>
        public void Override(string propertyName, Func<MvcHtmlString> builder)
        {
            _overrides[propertyName] = builder;
        }


        /// <summary>
        /// Controls the specified key.
        /// </summary>
        /// <param name="propertyName">The propertyName.</param>
        /// <returns></returns>
        public MvcHtmlString Control(string propertyName)
        {
            if (_overrides.ContainsKey(propertyName))
                return _overrides[propertyName]();

            string dataType = string.Empty;
            string css = string.Empty;
            if (_hasOptionDefs)
            {
                dataType = _optionDefsMap[propertyName].ValType;
                css = _optionDefsMap[propertyName].DisplayStyle;
            }
            else
                dataType = _props[propertyName].PropertyType.Name;

            object val = _props[propertyName].GetValue(_model, null);
            string stringVal = val == null ? string.Empty : val.ToString();
            return _htmlHelper.Control(propertyName, dataType, stringVal, css);
        }


        /// <summary>
        /// Gets the description for the specified propertyname.
        /// </summary>
        /// <param name="propertyName">Name of the property to get description for.</param>
        /// <returns></returns>
        public string FriendlyName(string propertyName)
        {
            if (!_hasResources || !_resources.ContainsKey(propertyName))
                return propertyName;

            return _resources[propertyName].Name;
        }


        /// <summary>
        /// Gets the description for the specified propertyname.
        /// </summary>
        /// <param name="propertyName">Name of the property to get description for.</param>
        /// <returns></returns>
        public string Description(string propertyName)
        {
            if (!_hasResources || !_resources.ContainsKey(propertyName))
                return string.Empty;

            return _resources[propertyName].Description;
        }


        /// <summary>
        /// Gets example text for the specified propertyname.
        /// </summary>
        /// <param name="examplePrefix">The example prefix.</param>
        /// <param name="propertyName">The propertyName to get example for.</param>
        /// <returns></returns>
        public string Example(string examplePrefix, string propertyName)
        {
            if (!_hasResources || !_resources.ContainsKey(propertyName))
                return string.Empty;

            return _resources[propertyName].Example;
        }


        /// <summary>
        /// Loads the properties for the object supplied.
        /// </summary>
        /// <param name="obj">The obj.</param>
        private void LoadProperties(object obj)
        {
            if (obj == null) return;

            var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public);
            _props = new Dictionary<string, PropertyInfo>();
            foreach (var prop in props) _props[prop.Name] = prop;
        }
    }
}
