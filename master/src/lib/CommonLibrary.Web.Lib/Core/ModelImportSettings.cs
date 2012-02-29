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
