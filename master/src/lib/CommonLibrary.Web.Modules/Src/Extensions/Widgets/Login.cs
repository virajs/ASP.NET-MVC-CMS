using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Feeds Widget
    /// </summary>
    [Widget(Name = "Login", IsCachable = true, IsEditable = true, SortIndex = 5, Path = "Widgets/Login")]
    public class Login : WidgetInstance
    {
        /// <summary>
        /// Gets or sets the name of the entity the archives apply to.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Label = "Url", Description = "The url to use for Ajax call to validate login")]
        public string LoginUrl { get; set; }

    }
}
