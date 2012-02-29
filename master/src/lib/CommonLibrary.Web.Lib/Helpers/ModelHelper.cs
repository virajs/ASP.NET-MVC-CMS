using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;
using System.Reflection;

using ComLib;
using ComLib.MapperSupport;
using ComLib.Reflection;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Lib.Helpers
{
    /// <summary>
    /// Helper class for 
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// Load widgets from the assembly provided.
        /// </summary>
        /// <param name="filePath">File path containing the model definitions</param>
        /// <param name="assemblyNamesDelimited">Comma delimited list of assembly names to load modeldefs from.</param>
        /// <param name="author">Override author</param>
        /// <param name="email">Override email</param>
        /// <param name="version">Override version</param>
        /// <param name="url">Ovverride url</param>
        /// <returns></returns>
        public static IList<ModelSettings> LoadAll(string filePath, string assemblyNamesDelimited, string author = "kishore reddy", string email = "kishore_reddy@codeplex.com", string version = "0.9.4.1", string url = "http://commonlibrarynetcms.codeplex.com")
        {
            bool fileProvided = !string.IsNullOrEmpty(filePath);
            bool hasAssemblys = !string.IsNullOrEmpty(assemblyNamesDelimited);

            if (!fileProvided && !hasAssemblys)
                throw new ArgumentException("File path for model defs or list of assembly names must be provided.");

            var models = new OrderedDictionary();
            if (hasAssemblys)
            {   
                // Any loaded from assembly? Load into dictionary.
                var modelDefsFromAssembly = LoadFromAssemblies(assemblyNamesDelimited);
                if (modelDefsFromAssembly != null && modelDefsFromAssembly.Count > 0)
                    foreach (var modelDef in modelDefsFromAssembly)
                        models[modelDef.Value.Name] = modelDef.Value;
                    
            }
            // Model definition config file supplied?
            if (fileProvided)
            {
                // Override the model defs from assembly with ones supplied in configuration file.
                var modelDefsFromFile = LoadFromFile(filePath);
                if (modelDefsFromFile != null && modelDefsFromFile.Count > 0)
                    foreach (var modelDef in modelDefsFromFile)
                        models[modelDef.Name] = modelDef;
            }

            // Now get all models from map as a list.
            var all = new List<ModelSettings>();
            foreach (DictionaryEntry pair in models) all.Add(pair.Value as ModelSettings);

            return all;
        }


        /// <summary>
        /// Loads all model definitions from all the assemblies supplied in the comma delimited string of assembly names.
        /// </summary>
        /// <param name="assemblyNamesDelimited">Comma delimited string of assembly names to load model defs from.</param>
        /// <returns></returns>
        public static IList<KeyValuePair<Type, ModelSettings>> LoadFromAssemblies(string assemblyNamesDelimited)
        {
            var allModels = new List<KeyValuePair<Type, ModelSettings>>();
            string[] assemblies = assemblyNamesDelimited.Split(',');
            if (assemblies != null && assemblies.Length > 0)
            {
                // Each assembly
                foreach (string assemblyName in assemblies)
                {
                    var modelDefsFromAssembly = LoadFromAssembly(assemblyName);

                    // Any loaded from assembly? Load into dictionary.
                    if (modelDefsFromAssembly != null && modelDefsFromAssembly.Count > 0)
                        allModels.AddRange(modelDefsFromAssembly);
                }
            }
            return allModels;
        }


        /// <summary>
        /// Load widgets from the assembly provided.
        /// </summary>
        /// <param name="assemblyname">Name of assembly to load widgets from.</param>
        /// <param name="author">Override author</param>
        /// <param name="email">Override email</param>
        /// <param name="version">Override version</param>
        /// <param name="url">Ovverride url</param>
        /// <returns></returns>
        public static IList<KeyValuePair<Type, ModelSettings>> LoadFromAssembly(string assemblyname, string author = "kishore reddy", string email = "kishore_reddy@codeplex.com", string version = "0.9.4.1", string url = "http://commonlibrarynetcms.codeplex.com")
        {
            var models = AttributeHelper.GetClassAttributesFromAssembly<ModelAttribute>("CommonLibrary.Web.Modules", pair =>
            {
                if (string.IsNullOrEmpty(pair.Value.Name)) pair.Value.Name = pair.Key.Name;
                if (string.IsNullOrEmpty(pair.Value.DisplayName)) pair.Value.DisplayName = pair.Key.Name;
                if (!string.IsNullOrEmpty(author)) pair.Value.Author = author;
                if (!string.IsNullOrEmpty(email)) pair.Value.Email = email;
                if (!string.IsNullOrEmpty(version)) pair.Value.Version = version;
                if (!string.IsNullOrEmpty(url)) pair.Value.Url = url;
                pair.Value.DeclaringType = pair.Key.FullName;
                pair.Value.DeclaringAssembly = assemblyname;
                
            });
            var modelDefs = new List<KeyValuePair<Type, ModelSettings>>();
            foreach (var pair in models)
            {
                var model = Convert(pair.Value);
                model.Model = pair.Key;
                modelDefs.Add(new KeyValuePair<Type, ModelSettings>(pair.Key, model));
            }

            return modelDefs;
        }


        /// <summary>
        /// Loads model definitions from the file path provided.
        /// </summary>
        /// <param name="filePath">File location of the model definitions</param>
        /// <param name="author">Override author</param>
        /// <param name="email">Override email</param>
        /// <param name="version">Override version</param>
        /// <param name="url">Ovverride url</param>
        /// <returns></returns>
        public static IList<ModelSettings> LoadFromFile(string filePath, string author = "kishore reddy", string email = "kishore_reddy@codeplex.com", string version = "0.9.4.1", string url = "http://commonlibrarynetcms.codeplex.com")
        {
            var allmodelDefs = Mapper.MapConfigFile<ModelSettings>(filePath);
            var modelDefs = (from m in allmodelDefs where !string.IsNullOrEmpty(m.DeclaringType) select m).ToList();
            int id = 1000;
            foreach (var modelDef in modelDefs)
            {
                // ComLib.Web.Modules.Pages.Page,CommonLibrary.Web.Modules
                Type type = Type.GetType(modelDef.DeclaringType + "," + modelDef.DeclaringAssembly);
                modelDef.Model = type;
                if (string.IsNullOrEmpty(modelDef.Name)) modelDef.Name = type.Name;
                if (string.IsNullOrEmpty(modelDef.DisplayName)) modelDef.DisplayName = type.Name;
                if (!string.IsNullOrEmpty(author)) modelDef.Author = author;
                if (!string.IsNullOrEmpty(email)) modelDef.Email = email;
                if (!string.IsNullOrEmpty(version)) modelDef.Version = version;
                if (!string.IsNullOrEmpty(url)) modelDef.Url = url;
            }
            return modelDefs;
        }


        /// <summary>
        /// Converts a widgetattribute to a widget definition.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static ModelSettings Convert(ModelAttribute attribute)
        {
            ModelSettings model = new ModelSettings();
            model.Id = attribute.Id;
            model.Name = attribute.Name;
            model.DisplayName = attribute.DisplayName;
            model.Author = attribute.Author;
            model.Email = attribute.Email;
            model.DeclaringType = attribute.DeclaringType;
            model.DeclaringAssembly = attribute.DeclaringAssembly;
            model.SortIndex = attribute.SortIndex;            
            model.Url = attribute.Url;
            model.Version = attribute.Version;
            model.IsPagable = attribute.IsPagable;
            model.IsSystemModel = attribute.IsSystemModel;
            model.IO.IsExportable = attribute.IsExportable;
            model.IO.IsImportable = attribute.IsImportable;
            model.IO.FormatsForExport = attribute.FormatsForExport;
            model.IO.FormatsForImport = attribute.FormatsForImport;
            model.View.UrlForCreate = attribute.UrlForCreate;
            model.View.UrlForIndex = attribute.UrlForIndex;
            model.View.UrlForManage = attribute.UrlForManage;
            model.View.HeadingForCreate = attribute.HeadingForCreate;
            model.View.HeadingForDetails = attribute.HeadingForDetails;
            model.View.HeadingForEdit = attribute.HeadingForEdit;
            model.View.HeadingForIndex = attribute.HeadingForIndex;
            model.View.HeadingForManage = attribute.HeadingForManage;
            model.Permissions.RolesForModel = attribute.RolesForModel;
            model.Permissions.RolesForCreate = attribute.RolesForCreate;
            model.Permissions.RolesForView = attribute.RolesForView;
            model.Permissions.RolesForIndex = attribute.RolesForIndex;
            model.Permissions.RolesForManage = attribute.RolesForManage;
            model.Permissions.RolesForDelete = attribute.RolesForDelete;
            model.Permissions.RolesForImport = attribute.RolesForImport;
            return model;
        }
    }
}
