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


namespace ComLib.Web.Lib.Core
{

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
        /// Gets or sets the roles for create.
        /// </summary>
        /// <value>The roles for create.</value>
        public string RolesForEdit { get; set; }


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

}
