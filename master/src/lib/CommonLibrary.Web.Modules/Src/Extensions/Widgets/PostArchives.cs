using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Modules.Posts;

namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Post Archives Widget
    /// </summary>
    [Widget(Name = "PostArchives", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class PostArchives : WidgetInstance
    {
        /// <summary>
        /// The display type of the archives list. E.g. DropDown,Normal,FullExpandCollapse
        /// </summary>
        [PropertyDisplay(Order = 1, DefaultValue = "", Description = "How to display the archives( DropDown,Normal,FullExpandCollapse")]
        public string DisplayType { get; set; }


        /// <summary>
        /// Whether or not this can render html itself or uses a control to do it.
        /// </summary>
        /// <value></value>
        public override bool IsSelfRenderable
        {
            get { return true; }
        }


        public override string Render()
        {
            // NOTE: 
            // In this example, it's easier to get a datatable for displaying Group By data
            // instead of converting back to Entities(Post) objects.    
            DataTable groups = Post.GetArchives();
            var buffer = new StringBuilder();

            buffer.Append("<ul>" + Environment.NewLine);
            for (int ndx = 0; ndx < groups.Rows.Count; ndx++)
            {
                int year = Convert.ToInt32(groups.Rows[ndx]["Year"]);
                int month = Convert.ToInt32(groups.Rows[ndx]["Month"]);
                int count = Convert.ToInt32(groups.Rows[ndx]["Count"]);
                DateTime date = new DateTime(year, month, 1);
                string link = string.Format("<li><a href=\"/post/archives/{0}/{1}\">{2} ( {3} )</a></li>", year, month, date.ToString("yyyy, MMMM"), count);
                buffer.Append(link);
            }
            buffer.Append("</ul>" + Environment.NewLine);
            string html = buffer.ToString();
            return html;
        }
    }
}
