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
    [Widget(Name = "Feeds", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class Feeds : WidgetInstance
    {
        /// <summary>
        /// Gets or sets the name of the entity the archives apply to.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Label = "Entity Name", Order = 1, DefaultValue = "", Description = "The name of the entity for which to get feeds for")]
        public string EntityName { get; set; }


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
            var buffer = new StringBuilder();

            buffer.Append("<ul>" + Environment.NewLine);
            
            string entityName = HttpUtility.HtmlEncode(EntityName);
            string rsslink = string.Format("<li><a href=\"\\{0}\\rss\" target=\"_blank\"><img src=\"\\content\\images\\generic\\rss.gif\" alt=\"rss\" /></a>&nbsp&nbsp&nbsp&nbsp<a href=\"\\{1}\\rss\" target=\"_blank\">{2} - Rss</a></li>", entityName, entityName, entityName);
            string atomlink = string.Format("<li><a href=\"\\{0}\\atom\" target=\"_blank\"><img src=\"\\content\\images\\generic\\rss.gif\" alt=\"rss\" /></a>&nbsp&nbsp&nbsp&nbsp<a href=\"\\{1}\\atom\" target=\"_blank\">{2} - Atom</a></li>", entityName, entityName, entityName);
            
            buffer.Append(rsslink + Environment.NewLine);
            buffer.Append(atomlink + Environment.NewLine);
            buffer.Append("</ul>" + Environment.NewLine);
            
            string html = buffer.ToString();
            return html;
        }
    }
}
