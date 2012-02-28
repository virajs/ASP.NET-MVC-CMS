using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Collections;


namespace ComLib.Web.Lib.Core
{

    /// <summary>
    /// Class to map modules to ids.
    /// </summary>
    public class ModuleMap
    {
        /// <summary>
        /// Class representing a module.
        /// </summary>
        public class ModuleItem
        {
            public int Id;
            public Type ModuleType;
        }


        private DictionaryBidirectional<string, ModuleItem> _mappingsShortNames = new DictionaryBidirectional<string, ModuleItem>();
        private DictionaryBidirectional<int, ModuleItem> _mappingsIds = new DictionaryBidirectional<int, ModuleItem>();
        private DictionaryBidirectional<Type, ModuleItem> _mappingsTypes = new DictionaryBidirectional<Type, ModuleItem>();
        private DictionaryBidirectional<string, ModuleItem> _mappingsFullNames = new DictionaryBidirectional<string, ModuleItem>();
        private static ModuleMap _instance = new ModuleMap();


        /// <summary>
        /// Get the instance
        /// </summary>
        public static ModuleMap Instance 
        {
            get { return _instance; }
        }


        /// <summary>
        /// Get the count.
        /// </summary>
        public int Count { get { return _mappingsTypes.Count; } }


        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Type, ModuleItem>> GetEnumerator()
        {
            return _mappingsTypes.GetEnumerator();
        }


        /// <summary>
        /// Register the module.
        /// </summary>
        /// <param name="moduleType"></param>
        /// <param name="id"></param>
        public void Register(Type moduleType, int id)
        {
            var mod = new ModuleItem(){ ModuleType = moduleType, Id = id };
            _mappingsTypes[moduleType] = mod;
            _mappingsShortNames[mod.ModuleType.Name] = mod;
            _mappingsShortNames[mod.ModuleType.Name.ToLower()] = mod;
            _mappingsIds[id] = mod;
            _mappingsFullNames[mod.ModuleType.FullName] = mod;
        }


        /// <summary>
        /// Get set id for typename
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetId(string typeName)
        {
            return _mappingsShortNames[typeName].Id;
        }


        /// <summary>
        /// Get the short name for the module w/ the specified id.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetShortName(int id)
        {
            return _mappingsIds[id].ModuleType.Name;
        }


        /// <summary>
        /// Get the short name for the module w/ the specified id.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetLongName(int id)
        {
            return _mappingsIds[id].ModuleType.FullName;
        }


        /// <summary>
        /// Get id of the module using it's type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public int GetId(Type type)
        {
            return _mappingsTypes[type].Id;
        }


        /// <summary>
        /// Get type of the module using it's id.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type GetModuleTypeUsing(int id)
        {
            return _mappingsIds[id].ModuleType;
        }


        /// <summary>
        /// Get type of the module using it's id.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public Type GetModuleTypeUsing(string shortName)
        {
            return _mappingsShortNames[shortName].ModuleType;
        }
    }
}
