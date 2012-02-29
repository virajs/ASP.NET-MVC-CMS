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
    public class ModelFeedSettings
    {
        public string Author { get; set; }


        public string Title { get; set; }


        public string Description { get; set; }
    }

}
