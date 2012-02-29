using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Feeds;
using ComLib.Entities;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// BlogRoll Widget
    /// </summary>
    [Widget(Name = "BlogRoll", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class BlogRoll : WidgetInstance
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public BlogRoll()
        {
            NumberOfEntries = 5;
            MaxTitleLength = 20;
            Format = "rss";
        }


        /// <summary>
        /// Gets or sets the name of the blogger.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Label = "Blogger Name", Order = 1, DefaultValue = "", Description = "The name of the blogger")]
        public string BloggerName { get; set; }


        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        [Required]
        [RegularExpression(@"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$", ErrorMessage = "Url is invalid")]
        [PropertyDisplay(Order = 2, DefaultValue = "", Description = "The url of the rss feed")]
        public string Url { get; set; }


        /// <summary>
        /// Gets or sets the number of entries.
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 50)]
        [PropertyDisplay(Label = "Number of entries", Order = 3, Description = "The number of entries to display")]
        public int NumberOfEntries { get; set; }


        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>The format.</value>
        [Required]
        [PropertyDisplay(Order = 4, DefaultValue = "", Description = "The format of the feed ( atom,rss )")]
        public string Format { get; set; }


        /// <summary>
        /// Max length (in chars ) of the title of each blog post.
        /// </summary>
        [PropertyDisplay(Label = "Max Title Length", Order = 5, DefaultValue = "", Description = "The maximum number of characters of a post title")]
        public int MaxTitleLength { get; set; }


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
            string html = string.Empty;
            html = TryRender(() =>
            {
                IList<KeyValue<string, string>> links = FeedHelper.LoadUrlItemsTitle(Url, NumberOfEntries);
                var buffer = new StringBuilder();

                buffer.Append("<span class=\"wtitle\">" + HttpUtility.HtmlEncode(BloggerName) + "</span>" + Environment.NewLine);
                buffer.Append("<ul>" + Environment.NewLine);
                foreach (var item in links)
                {
                    string title = StringHelper.TruncateWithText(item.Key, MaxTitleLength, "..");
                    string link = string.Format("<li><a href=\"{0}\">{1}</a></li>", HttpUtility.HtmlEncode(item.Value), HttpUtility.HtmlEncode(title));
                    buffer.Append(link);
                }
                buffer.Append("</ul>" + Environment.NewLine);
                string finalHtml = buffer.ToString();
                return finalHtml;
            });
            return html; 
        }
    }
}
