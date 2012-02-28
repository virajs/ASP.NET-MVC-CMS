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
using ComLib.Web.Lib.Services.Macros;

namespace ComLib.Web.Modules.Information
{

    [Info(Name = "Macros", Description = "Macros currently available", Roles = "Admin")]
    public class MacroInfo : IInformation
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
            var buffer = new StringBuilder();
            buffer.Append("<table class=\"systemlist\">");
            buffer.Append("<tr><th>Id</th><th>Name</th><th>Info</th></tr>");
            CMS.CMS.Macros.Lookup.ForEach((pair) =>
            {
                var attrib = pair.Value.Attribute as MacroAttribute;
                buffer.Append("<tr>");
                buffer.Append("<td>" + attrib.Name + "</td>");
                buffer.Append("<td>" + attrib.DisplayName + "</td>");
                
                // Build up the example w/ all the parameters.
                string argsExample = "";
                string argsText = "";
                for (int ndx = 0; ndx < pair.Value.AdditionalAttributes.Count; ndx++)
                {
                    var parameter = pair.Value.AdditionalAttributes[ndx] as MacroParameter;
                    argsExample += string.Format(@" {0}=""{1}""", parameter.Name, parameter.Example);
                    argsText += string.Format(@"<b>{0}</b>: {1}", parameter.Name, parameter.Description) + "<br/>";
                }

                string example = "";
                if (attrib.HasInnerContent)
                    example = string.Format(@"$[{0} {1}]{2}[/{3}]", attrib.Name, argsExample, attrib.ExampleInnerContent, attrib.Name);
                else
                    example = string.Format(@"$[{0} {1} /]", attrib.Name, argsExample);

                buffer.Append("<td><b>Description</b>: " + attrib.Description + "<br/><br/>" + argsText + "<br/><b>Ex</b>: " + example +"</td>");
                buffer.Append("</tr>");
            });
            buffer.Append("</table>");
            string html = buffer.ToString();
            return html;
        }
    }
}
