using System;
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
    /// Gravitar Widget
    /// </summary>
    [Widget(Name = "Part", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class Part : WidgetInstance
    {

        /// <summary>
        /// Descriptive text about the person represented by this gravatar.
        /// </summary>
        [PropertyDisplay(Order = 1, Description = "The content to display", DisplayStyle = "wide")]
        public string Content { get; set; }


        /// <summary>
        /// Whether or not this can render html itself or uses a control to do it.
        /// </summary>
        /// <value></value>
        public override bool IsSelfRenderable
        {
            get { return true; }
        }


        /// <summary>
        /// Renders this instance as Html.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            return Content;
        }
    }
}
