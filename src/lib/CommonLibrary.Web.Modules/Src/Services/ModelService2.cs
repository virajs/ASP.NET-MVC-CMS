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
    public class ModelService2
    {
        private IDictionary<string, ModelMetaDataInfo> _metaInfo = new Dictionary<string, ModelMetaDataInfo>();


        /// <summary>
        /// Register the model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="propsToExcludeDelimited"></param>
        public void Register<T>(string name = "", string propsToExcludeDelimited = "")
        {
            Type type = typeof(T);
            if (string.IsNullOrEmpty(name))
                name = type.Name;

            // Exclude properties.
            var excludeMap = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(propsToExcludeDelimited))
            {
                var excluded = propsToExcludeDelimited.Split(',');
                foreach (var exclude in excluded)
                    excludeMap[exclude] = exclude;
            }

            // Get all the properties.
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.Public);
            var propsToStore = new Dictionary<string, PropertyInfo>();
            foreach (var prop in props)
            {
                if (excludeMap.Count == 0 || !excludeMap.ContainsKey(prop.Name))
                {
                    propsToStore[prop.Name] = prop;
                }
            }
            _metaInfo[type.FullName] = new ModelMetaDataInfo { Name = name, Props = propsToStore, DataType = type, ExcludedProps = excludeMap };
        }


        /// <summary>
        /// Get the model data for the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ModelMetaDataInfo Get<T>()
        {
            if (!_metaInfo.ContainsKey(typeof(T).FullName))
                return null;

            return _metaInfo[typeof(T).FullName];
        }


        /// <summary>
        /// Get a string representing the properties for the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string GetAsJson<T>()
        {
            return string.Empty;
        }
    }



    /// <summary>
    /// Metadata class for a specified type.
    /// </summary>
    public class ModelMetaDataInfo
    {
        public string Name;
        public Type DataType;
        public IDictionary<string, PropertyInfo> Props;
        public Dictionary<string, string> ExcludedProps; 
        public List<Resource> Resources;
        public List<OptionDef> Options;
        private Dictionary<string, OptionDef> _options;
        private Dictionary<string, Resource> _resources;
        private bool _hasResources = false;


        /// <summary>
        /// Initialize w/ the resouces and options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="resources"></param>
        public void Init(List<OptionDef> options, List<Resource> resources)
        {
            foreach (var option in options)
                _options[option.Key] = option;

            foreach (var resource in resources)
                _resources[resource.Key] = resource;
        }


        /// <summary>
        /// Gets the description for the specified propertyname.
        /// </summary>
        /// <param name="propertyName">Name of the property to get description for.</param>
        /// <returns></returns>
        public string Label(string propertyName)
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
        public string Example(string propertyName, string examplePrefix = "")
        {
            if (!_hasResources || !_resources.ContainsKey(propertyName))
                return string.Empty;

            return _resources[propertyName].Example;
        }
    }
}
