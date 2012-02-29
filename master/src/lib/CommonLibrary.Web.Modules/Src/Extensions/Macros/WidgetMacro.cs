using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Templating;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Lib.Services.Macros;


namespace ComLib.Web.Modules.Macros
{
    [Macro( Name = "widget", DisplayName = "Widget", Description = "Renders a widget that is self-renderable", IsReusable = true)]
    [MacroParameter(Name = "id", Description = "The id of the widget", DataType = typeof(int), Example = "2")]
    public class WidgetMacro : IMacro
    {
        /// <summary>
        /// Process the tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string Process(Tag tag)
        {
            var components = WidgetInstance.Lookup;
            if (!components.ContainsKey(tag.Id)) return string.Empty;

            // Get widget and content
            var widget = components[tag.Id];
            var widgetContent = widget.Render();
            StringBuilder buffer = new StringBuilder();
            buffer.Append("<div class=\"widget\">");
            buffer.Append("<div class=\"wheader\">" + widget.Header + "</div>");
            buffer.Append("<div class=\"wbody\">" + widgetContent + "</div>");
            buffer.Append("</div>");
            return buffer.ToString();
        }
    }
}
