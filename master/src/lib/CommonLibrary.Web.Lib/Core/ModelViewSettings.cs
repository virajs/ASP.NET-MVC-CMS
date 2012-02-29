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
        public string ActionForCreationSuccess { get; set; }


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
        /// Location for the create page.
        /// </summary>
        public string PageLocationForCreate { get; set; }

        
        /// <summary>
        /// Location for the details page.
        /// </summary>
        public string PageLocationForDetails { get; set;}
        

        /// <summary>
        /// Location for the edit page.
        /// </summary>
        public string PageLocationForEdit { get; set; }


        /// <summary>
        /// Location for the manage page.
        /// </summary>
        public string PageLocationForIndex { get; set; }


        /// <summary>
        /// Location for the manage page.
        /// </summary>
        public string PageLocationForManage { get; set; }


        /// <summary>
        /// Header for create page.
        /// </summary>
        public string HeadingForCreate { get; set; }


        /// <summary>
        /// Header for edit page.
        /// </summary>
        public string HeadingForEdit { get; set; }


        /// <summary>
        /// Header for details page.
        /// </summary>
        public string HeadingForDetails { get; set; }


        /// <summary>
        /// Header for index page.
        /// </summary>
        public string HeadingForIndex { get; set; }


        /// <summary>
        /// Header for manage page.
        /// </summary>
        public string HeadingForManage { get; set; }
    }

}
