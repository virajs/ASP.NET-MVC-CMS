using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Tags;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Tags Widget
    /// </summary>
    [Widget(Name = "Tags", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class Tags : WidgetInstance
    {
        
        /// <summary>
        /// Gets or sets the name of the entity that this set of tags apply to.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Label = "Entity Name", Order = 1, Description = "The name of the entity the tags apply to")]
        public string EntityName { get; set; }


        /// <summary>
        /// Gets or sets the number of entries.
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 200)]
        [PropertyDisplay(Label = "Number Of Entries", Order = 1, Description = "Total number of tags to display")]
        public int NumberOfEntries { get; set; }


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
            string html = string.Empty;
            html = TryRender(() =>
            {
                string entityName = EntityName;
                int groupId = ComLib.Web.Lib.Core.ModuleMap.Instance.GetId(entityName);
                string url = "/" + entityName + "/tags/";
                string template = @"<a class='size${size}' href='" + url + "${urltagname}'>${tagname}</a> ";
                string cloud = Tag.BuildTagCloud(groupId, NumberOfEntries, template);
                var buffer = new StringBuilder();

                buffer.Append("<div id=\"tagcloud\">" + Environment.NewLine);
                buffer.Append(cloud);
                buffer.Append("</div>" + Environment.NewLine);
                string finalhtml = buffer.ToString();
                return finalhtml;
            });
            return html;
        }        
    }
}
