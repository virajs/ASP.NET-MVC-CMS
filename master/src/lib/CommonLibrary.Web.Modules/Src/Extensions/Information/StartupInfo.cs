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
using ComLib.BootStrapSupport;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Services.Information;


namespace ComLib.Web.Modules.Information
{

    [Info(Name = "Startup", Description = "Startup tasks", Roles = "Admin")]
    public class StartupInfo : IInformation
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
        /// <td><%= result.Name%></td>
        /// <td><%= result.ExecutedOn.ToShortTimeString() %></td>
        /// <td><%= result.Status()%></td>
        /// <td><%= result.Group %></td>
        /// <td><%= result.Priority.ToString() %></td>
        /// <td><%= result.Message %></td>
        /// </summary>
        /// <returns></returns>
        public string GetInfo()
        {
            BootStrapper booter = CMS.CMS.Bootup;
            var results = booter.GetStartupStatus();
            var buffer = new StringBuilder();
            buffer.Append("<table class=\"systemlist\">");
            buffer.Append("<tr><th>Name</th><th>Executed</th><th>Status</th><th>Group</th><th>Priority</th><th>Message</th></tr>");
            foreach (var result in results)
            {
                buffer.Append("<tr>");
                buffer.Append("<td>" + result.Name + "</td>");
                buffer.Append("<td>" + result.ExecutedOn.ToShortDateString() + "</td>");
                buffer.Append("<td>" + result.Status() + "</td>");
                buffer.Append("<td>" + result.Group + "</td>");
                buffer.Append("<td>" + result.Priority.ToString() + "</td>");
                buffer.Append("<td>" + result.Message + "</td>");
                buffer.Append("</tr>");
            }
            buffer.Append("</table>");
            string html = buffer.ToString();
            return html;
        }
    }
}
