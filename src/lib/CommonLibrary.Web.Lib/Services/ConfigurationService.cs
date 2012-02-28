using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ComLib.Collections;
using ComLib.Configuration;

namespace ComLib.Web.Lib.Services
{
    /// <summary>
    /// Information Service
    /// </summary>
    public class ConfigurationService
    {
        private IDictionary<string, ConfigData> _lookup = new DictionaryOrdered<string, ConfigData>();
        
        /// <summary>
        /// Metadata about the config settings.
        /// </summary>
        public class ConfigData
        {
            public object Config;
            public string Id;
            public string DisplayName;
            public string Description;
            public bool IsSelfStorageSupported { get { return Config == null ? false : Config is IConfigSourceBase; } }
            public Func<object> Loader;
            public Action<object> Saver;
        }


        /// <summary>
        /// Lookup all the information tasks available.
        /// </summary>
        public IDictionary<string, ConfigData> Lookup { get { return _lookup; } }


        /// <summary>
        /// Register the config source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="settings"></param>
        public void Register<T>(T settings, string id, string displayName = "", string description = "",
            Func<object> loader = null, Action<object> saver = null)
        {
            var config = new ConfigData();
            if (string.IsNullOrEmpty(id))
                id = typeof(T).Name.ToLower();

            config.Id = id;
            config.DisplayName = displayName;
            config.Description = description;
            config.Config = settings;
            config.Loader = loader;
            config.Saver = saver;
            _lookup[id] = config; 
        }


        /// <summary>
        /// Get the config.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ConfigData Get<T>() where T : class
        {
            return Get(typeof(T).Name);
        }


        /// <summary>
        /// Get the config object using the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public ConfigData Get(string name)
        {
            name = name.ToLower();
            if (!_lookup.ContainsKey(name))
                return null;

            var config = _lookup[name];
            return config;
        }


        /// <summary>
        /// Get the config object using the specified name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public void ForEach(Action<ConfigData> callback)
        {
            var ordered = _lookup as DictionaryOrdered<string, ConfigData>;
            for (int ndx = 0; ndx < ordered.Count; ndx++)
            {
                callback(ordered[ndx]);
            }
        }
    }
}
