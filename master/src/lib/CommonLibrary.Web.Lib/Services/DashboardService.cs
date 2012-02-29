using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;

using ComLib.Caching;
using ComLib.Authentication;
using ComLib.Configuration;
using ComLib.Collections;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Lib.Services
{
    public enum AuthType
    {
        Admin,
        Authenticated,        
        Guest,
        Role
    }


    /// <summary>
    /// Class to represent the dashboard.
    /// </summary>
    public class DashboardService
    {
        private WebAppModels _models = new WebAppModels();
        private Func<EntitySettingsHelper> _settingsFetcher = null;
        private IDictionary<string, IConfigSourceBase> _configs = new Dictionary<string, IConfigSourceBase>();
        private List<Menu> _sections = new List<Menu>();
        private ReadOnlyCollection<Menu> _adminSections;
        private ReadOnlyCollection<Menu> _nonAdminAuthenticatedSections;
        private string _adminRole = string.Empty;
        private bool _enableCache = false;
        private int _cacheTimeInSeconds = 300;


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="settingsFetcher"></param>
        public void Init(Func<EntitySettingsHelper> settingsFetcher)
        {
            _settingsFetcher = settingsFetcher;
        }


        /// <summary>
        /// Get the models.
        /// </summary>
        public WebAppModels Models
        {
            get { return _models; }
        }
        

        /// <summary>
        /// Gets the section with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Menu SectionNamed(string name)
        {
            var item = (from s in _sections where string.Compare(s.Name, name) == 0 select s).FirstOrDefault();
            return item;
        }


        /// <summary>
        /// Get the name of the admin role.
        /// </summary>
        public string AdminRole
        {
            get { return _adminRole; }
            set { _adminRole = value; }
        }




        /// <summary>
        /// Get the models.
        /// </summary>
        public IList<Menu> Sections
        {
            get { return _sections; }
        }



        /// <summary>
        /// Get all the section names.
        /// </summary>
        public string SectionNames
        {
            get
            {
                string[] names = (from section in _sections select section.Name).ToArray<string>();
                string namesDelimited = names.JoinDelimited(",", (item) => item);
                return namesDelimited;
            }
        }


        /// <summary>
        /// Get the importable model names for the supplied role.
        /// </summary>
        /// <param name="role">Role that should have access to the model.</param>
        /// <returns></returns>
        public IList<string> ImportablesFor(bool isAdmin, string role)
        {
            IList<string> importables = new List<string>();
            foreach (ModelSettings model in _models)
            {
                if (!model.IO.IsImportable)
                    continue;

                if (isAdmin)
                    importables.Add(model.Name);

                else if (model.Permissions.RolesForImport == "*" || (model.Permissions.RolesForImport != null && model.Permissions.RolesForImport.Contains(role)))
                    importables.Add(model.Name);
            }
            return importables;
        }


        public ReadOnlyCollection<Menu> GetSections(AuthType authtype, string role = null)
        {
            string search = "*";
            if (authtype == AuthType.Admin)
                search = _adminRole;
            else if (authtype == AuthType.Authenticated)
                search = "*";
            else if (authtype == AuthType.Role )
                search = role;

            var cachekey = "dash_sections_" + search;

            // Available from cache?
            var sections = Cacher.Get<ReadOnlyCollection<Menu>>(cachekey);
            if (sections != null && sections.Count > 0)
                return sections;

            // Get the items.
            var items = (from section in _sections where section.Roles.Contains(search) select section).ToList();
            var readonlyItems = new ReadOnlyCollection<Menu>(items);
            Cacher.Insert(cachekey, readonlyItems);

            // Return items.
            return readonlyItems;
        }


        /// <summary>
        /// Get admin specific sections.
        /// </summary>
        public ReadOnlyCollection<Menu> AdminSections
        {
            get { return GetSections(AuthType.Admin); }
        }


        /// <summary>
        /// Get only authenticated sections
        /// </summary>
        public ReadOnlyCollection<Menu> AuthenticatedSections
        {
            get { return GetSections(AuthType.Authenticated); }
        }


        /// <summary>
        /// Add errors from entity to the model state.
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="entity"></param>
        public bool CanCreateModel(ModelSettings settings)
        {
            bool isAdmin = Auth.IsAdmin();
            if( isAdmin ) return true;    
            
            // Not Admin and system model
            if (settings.IsSystemModel)
                return false;

            var helper = _settingsFetcher();

            // only for role and not in role
            if (settings.Permissions.IsOnlyForRole && Auth.IsUserInRoles(settings.Permissions.RolesForModel))
                return true;

            return helper.IsAuthorizedFor(settings.Permissions.RolesForCreate);
        }
    }
    


    /// <summary>
    /// Class to handle models
    /// </summary>
    public class WebAppModels : GenericListBase<ModelSettings>
    {
        /// <summary>
        /// Add a list of models that should be shown in the dashboard on the sidebar.
        /// </summary>
        /// <param name="models"></param>
        public void Add(params Type[] models)
        {
            if (models == null || models.Length == 0)
                return;

            // Add the default definitions
            foreach (Type model in models)
                this.Add(new ModelSettings(model));
        }


        /// <summary>
        /// Gets the actual model name for a possible alias.
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public string ModelNameForAlias(string alias)
        {
            var actualModelName = (from m in (IList<ModelSettings>)this._items where m.Name == alias || m.DisplayName == alias select m.ModelName).FirstOrDefault();
            return actualModelName;
        }
    }
}
