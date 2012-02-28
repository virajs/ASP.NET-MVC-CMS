using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.MenuEntrys;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Pages Widget
    /// </summary>
    [Widget(Name = "Pages", IsCachable = true, IsEditable = true, SortIndex = 5)]    
    public class Pages : WidgetInstance
    {
        /// <summary>
        /// Gets or sets the number of records to display.
        /// </summary>
        /// <value>The number of entries.</value>
        [PropertyDisplay(Label = "Front Pages Only", Order = 1, DefaultValue = "", Description = "Whether or not to only show the front pages")]
        public bool FrontPagesOnly { get; set; }


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
            var menuitems = MenuEntry.FrontPages();
            var buffer = new StringBuilder();


            buffer.Append("<ul>" + Environment.NewLine);
            foreach (var item in menuitems)
            {
                string link = string.Format("<li><a href=\"{0}\">{1}</a></li>", HttpUtility.HtmlEncode(item.UrlAbsolute), HttpUtility.HtmlEncode(item.Name));
                buffer.Append(link);
            }
            buffer.Append("</ul>" + Environment.NewLine);
            string html = buffer.ToString();
            return html;
        }
    }
}
