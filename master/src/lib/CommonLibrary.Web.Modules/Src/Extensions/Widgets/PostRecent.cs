using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Feeds;
using ComLib.Extensions;
using ComLib.Entities;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Posts;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// PostRecent Widget
    /// </summary>
    [Widget(Name = "PostRecent", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class PostRecent : WidgetInstance
    {
        private static IDictionary<string, KeyValuePair<Type, Func<int, IList<IPublishable>>>> _fetchers = new Dictionary<string, KeyValuePair<Type, Func<int, IList<IPublishable>>>>();


        /// <summary>
        /// Initialize the mappers.
        /// </summary>
        /// <param name="mappers"></param>
        public static void Init(params KeyValuePair<Type, Func<int, IList<IPublishable>>>[] mappers)
        {
            if (mappers == null || mappers.Length == 0)
                return;

            foreach (var mapper in mappers)
                _fetchers[mapper.Key.Name.ToLower()] = mapper;
        }


        /// <summary>
        /// Gets or sets the name of the entity the archives apply to.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Order = 1, DefaultValue = "", Description = "The name of the entity to get recent posts for( blog, event, etc )")]
        public string EntityName { get; set; }


        /// <summary>
        /// Gets or sets the number of entries.
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 20)]
        [PropertyDisplay(Label = "Number of Entries", Order = 2, DefaultValue = "5", Description = "Total number of entries to display")]
        public int NumberOfEntries { get; set; }


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
            // Check             
            string entityName = EntityName;
            if(string.IsNullOrEmpty(entityName))
                return "Unable to obtain recent items";
            
            entityName = entityName.ToLower().Trim();
            if (!_fetchers.ContainsKey(entityName))
                return "Unable to obtain recent " + entityName;

            Func<int, IList<IPublishable>> fetcher = _fetchers[entityName].Value;
            IList<IPublishable> posts = fetcher(NumberOfEntries);   

            var buffer = new StringBuilder();
            
            buffer.Append("<ul>" + Environment.NewLine);
            foreach (IPublishable post in posts)
            {
                string link = string.Format("<li><a href=\"/" + entityName + "/show/{0}\">{1}</a></li>", HttpUtility.HtmlEncode(post.UrlRelative), HttpUtility.HtmlEncode(post.Title.TruncateWithText(22, "..")));
                buffer.Append(link);
            }
            buffer.Append("</ul>" + Environment.NewLine);
            string html = buffer.ToString();
            return html;
        }
    }
}
