using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Collections;
using System.Collections.ObjectModel;

using ComLib.Caching;
using ComLib.Configuration;
using ComLib.Collections;


namespace ComLib.Web.Lib.Models
{
    /// <summary>
    /// Represents a specific entry in the Dashboard model section.
    /// </summary>
    public class ModelDef
    {
        private bool _isOnlyForAdmin = false;


        /// <summary>
        /// Initializes a new instance of the <see cref="ModelEntry"/> class.
        /// </summary>
        public ModelDef()
        {
            Init();
        }


        /// <summary>
        /// Initialize using the various properties.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="alias"></param>
        /// <param name="indexUrlPart"></param>
        /// <param name="manageUrlPart"></param>
        public ModelDef(Type model, string alias = null, string indexUrlPart = null, string manageUrlPart = null)
        {
            Init();
            Model = model;
            DisplayName = string.IsNullOrEmpty(alias) ? model.Name : alias;
            View.UrlForIndex = string.IsNullOrEmpty(indexUrlPart) ? "index" : indexUrlPart;
            View.UrlForManage = string.IsNullOrEmpty(manageUrlPart) ? "manage" : manageUrlPart;
        }


        /// <summary>
        /// The id of the module.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public Type Model { get; set; }


        /// <summary>
        /// Get the name to display.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /// <value>The alias.</value>
        public string DisplayName { get; set; }


        /// <summary>
        /// Name of the model/datatype.
        /// </summary>
        public string ModelName { get { return Model == null ? Name : Model.Name; } }


        /// <summary>
        /// Author of the widget.
        /// </summary>
        public string Author { get; set; }


        /// <summary>
        /// Email of the author.
        /// </summary>
        public string Email { get; set; }


        /// <summary>
        /// Version of the widget.
        /// </summary>
        public string Version { get; set; }

        
        /// <summary>
        /// Used to sort model in list of models.
        /// </summary>
        public int SortIndex { get; set; }


        /// <summary>
        /// Url for reference / documentation.
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// Whether or not this model is pagable.
        /// </summary>
        public bool IsPagable { get; set; }


        /// <summary>
        /// Permissions for model.
        /// </summary>
        public ModelPermissionSettings Permissions { get; set; }


        /// <summary>
        /// View settings for model.
        /// </summary>
        public ModelViewSettings View { get; set; }


        /// <summary>
        /// Import export settings
        /// </summary>
        public ModelImportSettings IO { get; set; }


        /// <summary>
        /// The name of the assembly this model is defined in.
        /// </summary>
        public string DeclaringAssembly { get; set; }


        /// <summary>
        /// The fully qualified name of the declaring type of this model.
        /// e.g. ComLib.Web.Modules.Posts.Post
        /// </summary>
        private string _declaringType;
        public string DeclaringType
        {
            get { return _declaringType; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _declaringType = value;
                    Type type = Type.GetType(value);
                    if (type != null)
                    {
                        Model = type;
                    }
                }
            }
        }


        private string _controllerName;
        public string ControllerName
        {
            get { return string.IsNullOrEmpty(_controllerName) ? Name : _controllerName; }
            set { _controllerName = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is only for admin.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is only for admin; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnlyForAdmin
        {
            get { return _isOnlyForAdmin; }
            set
            {
                _isOnlyForAdmin = value;
                if (_isOnlyForAdmin)
                {
                    Permissions.RolesForCreate = "Admin";
                    Permissions.RolesForDelete = "Admin";
                    Permissions.RolesForManage = "Admin";
                    Permissions.RolesForIndex = "Admin";
                }
            }
        }


        private void Init()
        {
            Permissions = new ModelPermissionSettings();
            View = new ModelViewSettings();
            IO = new ModelImportSettings();
        }
    }



    /// <summary>
    /// Permissions class to store various access rights/permissions for a model.
    /// </summary>
    public class ModelPermissionSettings
    {
        /// <summary>
        /// Indicates if this model and all it's crud actions are only available for a specific role.
        /// </summary>
        public bool IsOnlyForRole { get; set; }


        /// <summary>
        /// Gets or sets the role for model. Used in conjunction with <see cref="IsOnlyForRole"/>
        /// This is role to check for the entire model.
        /// </summary>
        /// <value>The role for model.</value>
        public string RolesForModel { get; set; }

        /// <summary>
        /// Gets or sets the roles for create.
        /// </summary>
        /// <value>The roles for create.</value>
        public string RolesForCreate { get; set; }


        /// <summary>
        /// Gets or sets the roles for viewing.
        /// </summary>
        public string RolesForView { get; set; }


        /// <summary>
        /// Get / set the roles that can import this model.
        /// </summary>
        public string RolesForImport { get; set; }


        /// <summary>
        /// Gets or sets the roles for delete.
        /// </summary>
        /// <value>The roles for delete.</value>
        public string RolesForDelete { get; set; }


        /// <summary>
        /// Gets or sets the roles for manage.
        /// </summary>
        /// <value>The roles for manage.</value>
        public string RolesForManage { get; set; }


        /// <summary>
        /// Gets or sets the index of the roles for.
        /// </summary>
        /// <value>The index of the roles for.</value>
        public string RolesForIndex { get; set; }
    }



    /// <summary>
    /// Class to store the view relatated config settings.
    /// </summary>
    public class ModelViewSettings
    {
        /// <summary>
        /// Gets or sets the create URL part. e.g. /create
        /// </summary>
        /// <value>The create URL part.</value>
        public string UrlForCreate { get; set; }


        /// <summary>
        /// Get the url to redirect to when the creation of an entity is successful.
        /// </summary>
        public string UrlForCreationSuccess { get; set; }


        /// <summary>
        /// Gets or sets the index URL part. e.g. /index
        /// </summary>
        /// <value>The index URL part.</value>
        public string UrlForIndex { get; set; }


        /// <summary>
        /// Gets or sets the manage URL part. e.g. /manage
        /// </summary>
        /// <value>The index URL part.</value>
        public string UrlForManage { get; set; }


        /// <summary>
        /// Name of the model for the index page.
        /// </summary>
        public string ModelNameForIndex { get; set; }
    }



    public class ModelImportSettings
    {

        /// <summary>
        /// Gets or sets the supported import formats.
        /// </summary>
        /// <value>The supported import formats.</value>
        public string FormatsForImport { get; set; }


        /// <summary>
        /// Gets or sets the supported export formats.
        /// </summary>
        /// <value>The supported export formats.</value>
        public string FormatsForExport { get; set; }


        /// <summary>
        /// Whether or not this is importable.
        /// </summary>
        public bool IsImportable { get; set; }


        /// <summary>
        /// Whether or not this is exportable
        /// </summary>
        public bool IsExportable { get; set; }
    }
}
