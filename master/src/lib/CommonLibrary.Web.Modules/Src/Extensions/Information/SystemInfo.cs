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

    [Info(Name = "System", Description = "System Summary", Roles = "Admin")]
    public class SystemInfo : IInformation
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
            IList<KeyValuePair<string, string>> summary = new List<KeyValuePair<string, string>>();
            summary.Add(new KeyValuePair<string, string>("Environment", string.Format("{0}[{1}]", Env.Name, Env.EnvType.ToString())));
            summary.Add(new KeyValuePair<string, string>("Configuration", Env.RefPath));
            summary.Add(new KeyValuePair<string, string>("Database Server", Config.Get<string>("Database", "server")));
            summary.Add(new KeyValuePair<string, string>("Database Name", Config.Get<string>("Database", "database")));
            summary.Add(new KeyValuePair<string, string>("LogLevel", Logger.Default.Level.ToString()));
            summary.Add(new KeyValuePair<string, string>("Emails", Notifier.Settings.EnableNotifications.ToString()));
            summary.Add(new KeyValuePair<string, string>("Machine", Environment.MachineName));
            summary.Add(new KeyValuePair<string, string>("Started", ((DateTime)HttpContext.Current.Application["start_time"]).ToString("yyyy-MMM-dd : HH:mm:ss")));
            summary.Add(new KeyValuePair<string, string>("User", Auth.UserShortName));
            summary.Add(new KeyValuePair<string, string>("Version", typeof(ComLib.BoolMessage).Assembly.GetName().Version.ToString() + " - CommonLibrary.dll" ));
            summary.Add(new KeyValuePair<string, string>("Version", typeof(ModuleMap).Assembly.GetName().Version.ToString() + " - CommonLibrary.Web.Lib.dll"));
            summary.Add(new KeyValuePair<string, string>("Version", typeof(ComLib.Web.Modules.Events.Event).Assembly.GetName().Version.ToString() + " - CommonLibrary.Web.Modules.dll"));

            var buffer = new StringBuilder();
            buffer.Append("<table class=\"systemlist\">");
            buffer.Append("<tr><th>Area</th><th>Setting</th></tr>");
            foreach (var entry in summary)
            {
                buffer.Append("<tr>");
                buffer.Append("<td>" + entry.Key + "</td>");
                buffer.Append("<td>" + entry.Value + "</td>");
                buffer.Append("</tr>");
            }
            buffer.Append("</table>");
            string html = buffer.ToString();
            return html;
        }
    }
}
