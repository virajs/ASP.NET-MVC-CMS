using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ComLib.Authentication;
using ComLib.Environments;
using ComLib.Configuration;
using ComLib.Logging;
using ComLib.Notifications;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Services.Information;


namespace ComLib.Web.Modules.Information
{

    [Info(Name = "Widgets", Description = "Widgets currently available", Roles = "Admin")]
    public class WidgetInfo : IInformation
    {
        /// <summary>
        /// The supported formats.
        /// </summary>
        public string SupportedFormats { get { return "html"; } }


        /// <summary>
        /// The current format to get the information in.
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// Gets the information.
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            var widgets = Widgets.Widget.GetAll();
            var buffer = new StringBuilder();
            buffer.Append("<table class=\"systemlist\">");
            buffer.Append("<tr><th>Name</th><th>Version</th><th>Author</th><th>Info</th></tr>");
            foreach (var widget in widgets)
            {
                buffer.Append("<tr>");
                buffer.Append("<td>" + widget.Name + "</td>");
                buffer.Append("<td>" + widget.Version + "</td>");
                buffer.Append("<td>" + widget.Author + "</td>");
                buffer.Append("<td>" 
                    + widget.Description + "<br />"
                    + widget.Email + "<br />"
                    + "<a href=\"" + widget.Url + "\">" + widget.Url + "</a><br />"
                    + widget.DeclaringType + "</td>");
                buffer.Append("</tr>");
            }
            buffer.Append("</table>");
            string html = buffer.ToString();
            return html;
        }
    }
}
