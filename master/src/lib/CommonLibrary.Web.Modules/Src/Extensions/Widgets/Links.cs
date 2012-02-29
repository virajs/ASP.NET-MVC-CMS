using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Entities;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Links;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Links widget
    /// </summary>
    [Widget(Name = "Links", IsCachable = true, IsEditable = true, SortIndex = 5, Path = "Widgets/Links")]
    public class Links : WidgetInstance
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Links()
        {
            NumberOfEntries = 5;
        }


        /// <summary>
        /// Gets or sets the name of the blogger.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Order = 1, DefaultValue = "", Description = "The name of the group the links")]
        public string Group { get; set; }


        /// <summary>
        /// Gets or sets the number of records to display.
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 100)]
        [PropertyDisplay(Label = "Number of entries", Order = 2, DefaultValue = "4", Description = "How many links to display")]
        public int NumberOfEntries { get; set; }


        /*
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
            IQuery<Link> criteria = Query<Link>.New().Where(l => l.Group).Is(DataUtils.Encode(Group)).OrderBy(l => l.SortIndex).Limit(NumberOfEntries);
            var items = Link.Find(criteria);
            var buffer = new StringBuilder();

                
            buffer.Append("<ul>" + Environment.NewLine);
            foreach (var item in items)
            {
                string link = string.Format("<li><a href=\"{0}\">{1}</a></li>", HttpUtility.HtmlEncode(item.Url), HttpUtility.HtmlEncode(item.Name));
                buffer.Append(link);
            }
            buffer.Append("</ul>" + Environment.NewLine);
            string html = buffer.ToString();
            return html;
        }
        */
    }
}
