using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Attributes
{  
    /// <summary>
    /// Attribute used to define a widget.
    /// </summary>
    public class ModelAttribute : ExtensionAttribute
    {
        /// <summary>
        /// Default values.
        /// </summary>
        public ModelAttribute()
        {
        }


        /// <summary>
        /// Id of the module.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// Used to sort model in list of models.
        /// </summary>
        public int SortIndex { get; set; }

        
        public bool IsSystemModel { get; set; }


        public bool IsImportable { get; set; }


        public bool IsExportable { get; set; }


        public bool IsPagable { get; set; }


        public string FormatsForExport { get; set; }


        public string FormatsForImport { get; set; }


        /// <summary>
        /// Gets or sets the role for model. Used in conjunction with <see cref="IsOnlyForRole"/>
        /// This is role to check for the entire model.
        /// </summary>
        /// <value>The role for model.</value>
        public string RolesForModel { get; set; }


        public string RolesForCreate { get; set; }


        public string RolesForView { get; set; }


        public string RolesForIndex { get; set; }


        public string RolesForManage { get; set; }


        public string RolesForDelete { get; set; }


        public string RolesForImport { get; set; }


        public string RolesForExport { get; set; }


        public string UrlForIndex { get; set; }


        public string UrlForManage { get; set; }


        public string UrlForCreate { get; set; }


        public string HeadingForCreate { get; set; }


        public string HeadingForEdit { get; set; }
        
        
        public string HeadingForIndex { get; set; }


        public string HeadingForDetails { get; set; }
                

        public string HeadingForManage { get; set; }
    }
}
