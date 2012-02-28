using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Authentication;
using ComLib.Extensions;
using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Web.Lib.Models;
using ComLib.Data;


namespace ComLib.Web.Lib.Core
{
    /// <summary>
    /// Security helper for entities.
    /// </summary>
    public class EntitySettingsHelper
    {
        private Func<bool> _isAuthenticatedFetcher;
        private Func<string, bool> _isUserInRolesOrAdminFetcher;
        private Func<string, string, string> _rolesFetcher;
        private IDictionary _settings; 
        private string _commonPermissionsSectionName;
        private bool _allowAccessIfPermissionNotFound = false;
        private bool _usePrefixForActionName = false;
        private string _prefixForActionName = "Permissions.RolesFor";
        private IDictionary<Type, ModelSettings> _modelDefs;


        /// <summary>
        /// Initialize the roles
        /// </summary>
        /// <param name="roles"></param>
        public EntitySettingsHelper()
        {
            Init();
        }


        /// <summary>
        /// Initialize the roles
        /// </summary>
        /// <param name="roles"></param>
        public EntitySettingsHelper(IDictionary settings)
        {
            _settings = settings;
            Init();
        }


        /// <summary>
        /// Initialize the roles fetcher.
        /// </summary>
        /// <param name="rolesFetcher"></param>
        public EntitySettingsHelper(Func<string, string, string> rolesFetcher)
        {
            _rolesFetcher = rolesFetcher;
            Init();
        }


        /// <summary>
        /// Initialize the roles fetcher.
        /// </summary>
        /// <param name="rolesFetcher"></param>
        public void Init(IDictionary<Type, ModelSettings> modelDefs)
        {
            _modelDefs = modelDefs;
            Init();
        }


        /// <summary>
        /// Initialize the configuration with the settings.
        /// </summary>
        /// <param name="settings"></param>
        public void InitConfig(IDictionary settings)
        {
            _settings = settings;
        }
        


        /// <summary>
        /// Initialize lamdas.
        /// </summary>
        /// <param name="commonPermissionsSectionName"></param>
        /// <param name="isAuthenticatedFetcher"></param>
        /// <param name="isUserInRolesOrAdminFetcher"></param>
        public void Init(string commonPermissionsSectionName = "CommonPermissions", 
            Func<bool> isAuthenticatedFetcher = null, 
            Func<string, bool> isUserInRolesOrAdminFetcher = null)
        {
            _commonPermissionsSectionName = commonPermissionsSectionName;
            _isAuthenticatedFetcher = isAuthenticatedFetcher;
            _isUserInRolesOrAdminFetcher = isUserInRolesOrAdminFetcher;
        }


        /// <summary>
        /// Name of the common section name used for inherited permissions.
        /// </summary>
        public string CommonPermissionsSectionName
        {
            get { return _commonPermissionsSectionName; }
            set { _commonPermissionsSectionName = value; }
        }


        /// <summary>
        /// Whether or not to allow access to action if that action/permission is not found in the settings.
        /// </summary>
        public bool AllowAccessOnMissingPermission
        {
            get { return _allowAccessIfPermissionNotFound; }
            set { _allowAccessIfPermissionNotFound = value; }
        }


        /// <summary>
        /// Whether or not to check for permissions for a specific action ( Create ) by using a prefix based naming convention ( e.g. "Permissions.RolesFor" is default naming convention." ).
        /// </summary>
        public bool UsePrefixOnActionName
        {
            get { return _usePrefixForActionName; }
            set { _usePrefixForActionName = value; }
        }


        /// <summary>
        /// The prefix used for the naming convention when gettings permissions for a specific action.
        /// </summary>
        public string PrefixForActionName
        {
            get { return _prefixForActionName; }
            set { _prefixForActionName = value; }
        }


        /// <summary>
        /// Gets the model for the type supplied.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ModelSettings ModelFor<T>()
        {
            if(!_modelDefs.ContainsKey(typeof(T)))
                return null;

            return _modelDefs[typeof(T)];
        }


        /// <summary>
        /// Get the model permissions for.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public ModelPermissionSettings PermissionsFor<T>()
        {
            if (!_modelDefs.ContainsKey(typeof(T)))
                return null;

            return _modelDefs[typeof(T)].Permissions;
        }


        /// <summary>
        /// Whether or not the currently logged in user has access to the model,action name.
        /// </summary>
        /// <param name="modelName">Link</param>
        /// <param name="actionName">Create</param>
        /// <returns></returns>
        public bool HasAccessTo(string modelName, string actionName)
        {
            string roles = PermissionsFor(modelName, actionName);
            return IsAuthorizedFor(roles);
        }


        /// <summary>
        /// Whether or not the current user is authorized for the roles supplied.
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        public bool IsAuthorizedFor(string roles)
        {
            if (string.IsNullOrEmpty(roles))
                return _allowAccessIfPermissionNotFound;

            // Guest ok.. no authentication required.
            if (string.Compare(roles, "?") == 0)
                return true;

            bool isAuthenticated = _isAuthenticatedFetcher == null
                                 ? Auth.IsAuthenticated()
                                 : _isAuthenticatedFetcher();
            bool anyAuthentcated = string.Compare(roles, "*") == 0;

            // Any authenticated user.
            if (anyAuthentcated && isAuthenticated)
                return true;

            // Require authentication.
            if (anyAuthentcated && !isAuthenticated)
                return false;

            // Now check user roles.
            bool authorized = _isUserInRolesOrAdminFetcher == null
                            ? Auth.IsUserInRolesOrAdmin(roles)
                            : _isUserInRolesOrAdminFetcher(roles);
            return authorized;
        }


        /// <summary>
        /// Get the roles for the model,action combination using string based access.
        /// </summary>
        /// <param name="modelName">Link</param>
        /// <param name="actionName">Create</param>
        /// <returns></returns>
        public string PermissionsFor(string modelName, string actionName)
        {
            if (_rolesFetcher != null)
                return _rolesFetcher(modelName, actionName);

            // model section: Article.
            // key : <actionname>
            string section = modelName;
            string key = _usePrefixForActionName ? _prefixForActionName + actionName : actionName;

            // Check if section exists?
            if (!_settings.Contains(modelName))
                section = _commonPermissionsSectionName;

            string roles = _settings.GetOrDefault<string>(section, key, string.Empty);
            return roles;
        }


        /// <summary>
        /// Whether or not the current user has access to create the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToCreate<T>()
        {
            return HasAccessTo<T>(p => p.RolesForCreate);
        }


        /// <summary>
        /// Whether or not the current user has access to create the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToEdit<T>()
        {
            return HasAccessTo<T>(p => p.RolesForEdit);
        }


        /// <summary>
        /// Whether or not the current user has access to view the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToView<T>()
        {
            return HasAccessTo<T>(p => p.RolesForView);
        }


        /// <summary>
        /// Whether or not the current user has access to delete the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToDelete<T>()
        {
            return HasAccessTo<T>(p => p.RolesForDelete);
        }


        /// <summary>
        /// Whether or not the current user has access to manage the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToManage<T>()
        {
            return HasAccessTo<T>(p => p.RolesForManage);
        }


        /// <summary>
        /// Whether or not the current user has access to index the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToIndex<T>()
        {
            return HasAccessTo<T>(p => p.RolesForIndex);
        }


        /// <summary>
        /// Whether or not the current user has access to index the model specified by T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public bool HasAccessToImport<T>()
        {
            return HasAccessTo<T>(p => p.RolesForImport);
        }



        /// <summary>
        /// Gets or defaults the value for the given section/key in config file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetOrDefault<T>(string section, string key, T defaultValue)
        {
            return this._settings.GetOrDefault<T>(section, key, defaultValue);
        }


        protected bool HasAccessTo<T>(Func<ModelPermissionSettings, string> roleFetcher)
        {
            var permissions = PermissionsFor<T>();
            if (permissions == null)
                return false;

            string roles = roleFetcher(permissions);
            return IsAuthorizedFor(roles);
        }
    }
}
