using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Templating;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Lib.Services.Macros;
using ComLib.Extensions;

namespace ComLib.Web.Modules.Macros
{
    [Macro( Name = "today", DisplayName = "Today", Description = "Renders the current date", IsReusable = true)]
    [MacroParameter( Name = "format", Description = "How to format the date", DataType = typeof(string), Example = "MM/dd/YYYY" )]
    [MacroParameter( Name = "adddays", Description = "Number of days to add to the date", DataType = typeof(int), Example = "1", ExampleMultiple = "1 | 2 | -1 | -2" )]        
    public class CurrentDateMacro : IMacro
    {
        /// <summary>
        /// Process the tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public string Process(Tag tag)
        {
            int days = tag.Attributes.GetOrDefault<int>("adddays", 0);
            string format = tag.Attributes.GetOrDefault<string>("format", "MM/dd/yyyy");
            string date = DateTime.Today.AddDays(days).ToString(format);
            return date;
        }
    }
}
