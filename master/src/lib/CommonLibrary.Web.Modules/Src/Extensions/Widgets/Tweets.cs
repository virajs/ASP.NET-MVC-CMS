using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Services.TwitterSupport;


namespace ComLib.CMS.Models.Widgets
{
    /// <summary>
    /// Tweets Widget
    /// </summary>
    [Widget(Name = "Tweets", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class Tweets : WidgetInstance
    {
        /// <summary>
        /// Gets or sets the name of the blogger.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Order = 1, DefaultValue = "horizonguy", Description = "The twitter username")]
        public string UserName { get; set; }


        /// <summary>
        /// Gets or sets the number of entries.
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 30)]
        [PropertyDisplay(Label = "Number of Entries", Order = 2, DefaultValue = "5", Description = "How many twitter posts to display")]
        public int NumberOfEntries { get; set; }


        /// <summary>
        /// Whether or not to only get authors tweets and not others replies.
        /// </summary>
        [PropertyDisplay(Label = "Only get author tweets", Order = 2, DefaultValue = "false", Description = "Whether or not to only get the authors tweets")]
        public bool OnlyGetAuthorTweets { get; set; }


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
            IList<Tweet> items = Twitter.GetFeed(UserName, NumberOfEntries);
            var buffer = new StringBuilder();

            buffer.Append("<div class=\"wtitle\">" + HttpUtility.HtmlEncode(UserName) + "</div>" + Environment.NewLine);
            buffer.Append("<ul>" + Environment.NewLine);
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    string link = string.Format("<li><a href=\"{0}\">{1}</a></li>", HttpUtility.HtmlEncode(item.Link), HttpUtility.HtmlEncode(item.Text));
                    buffer.Append(link);
                }
            }
            buffer.Append("</ul>" + Environment.NewLine);
            string html = buffer.ToString();
            return html;
        }
    }
}
