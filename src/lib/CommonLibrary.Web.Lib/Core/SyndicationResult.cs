using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.ServiceModel.Syndication;
using ComLib.Feeds;


namespace ComLib.Web.Lib.Core
{
    public class FeedActionResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the feed.
        /// </summary>
        /// <value>The feed.</value>
        public SyndicationFeed Feed { get; set; }
        

        /// <summary>
        /// Gets or sets the format either Rss or Atom
        /// </summary>
        /// <value>The format.</value>
        public string Format { get; set; }


        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";
            string format = string.IsNullOrEmpty(Format) ? "rss" : Format.ToLower();
            FeedBuilder.Build(Feed, context.HttpContext.Response.Output, format);
        }
    }
}
