using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Users Widget
    /// </summary>    
    [Widget(Name = "Users", Author = "Kishore Reddy", Email = "kishore_reddy@codeplex.com", 
        IsCachable = true, IsEditable = true, SortIndex = 5, Version = "0.9.4.1", Path = "Widgets/Users")]
    public class Users : WidgetInstance
    {       
        /// <summary>
        /// Gets or sets the number of entries to show horizontally
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 40)]
        [PropertyDisplay(Name = "Number Of Entries Across", Order = 2, DefaultValue = "3", Description = "How many entries to display horizontally")]
        public int NumberOfEntriesAcross { get; set; }


        /// <summary>
        /// Gets or sets the total number of entries to get/display
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 200)]
        [PropertyDisplay(Name = "Number Of Entries", Order = 2, DefaultValue = "18", Description = "How many total entries to display")]
        public int NumberOfEntries { get; set; }
    }
}
