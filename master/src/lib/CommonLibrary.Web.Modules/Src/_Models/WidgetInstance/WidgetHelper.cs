using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ComLib;
using ComLib.Reflection;
using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Caching;
using ComLib.Data;
using ComLib.MapperSupport;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.Web.Modules.Widgets
{
    public class WidgetHelper
    {
        /// <summary>
        /// Mapping of widget definition names to functions/lamda's that can create the proper instances of them.
        /// </summary>
        private static IDictionary<string, Func<WidgetInstance>> _factory = new Dictionary<string, Func<WidgetInstance>>();
        private static WidgetService _service = new WidgetService();

        /// <summary>
        /// Creates the specified widget using the definition name supplied.
        /// </summary>
        /// <param name="defname">Name of the widget definition.</param>
        /// <returns></returns>
        public static WidgetInstance Create(string defname)
        {
            return _service.Create(defname);
        }


        /// <summary>
        /// Creates the specified widget using the definition name supplied.
        /// </summary>
        /// <param name="defname">Name of the widget definition.</param>
        /// <returns></returns>
        public static WidgetInstance CreateWithDefaultSettings(string defname)
        {
            return _service.CreateWithDefaultSettings(defname);
        }


        /// <summary>
        /// Copies the widget by first loading back state data and then resetting id.
        /// </summary>
        /// <param name="widget"></param>
        public static void CopyWidget(WidgetInstance widget)
        {
            // Reset the id so it's not marked as persistant and load back state.
            widget.LoadState(); 
            widget.ToNewCopy();
        }
        

        public static BoolMessage SaveOrderings(string orderings)
        {
            string[] entries = orderings.Split(';');
            var lookup = WidgetInstance.Lookup;
            var widgets = new List<WidgetInstance>();

            // Each ordering id, zone, sortindex
            foreach (string ordering in entries)
            {
                string[] props = ordering.Split(',');
                if (props != null && props.Length > 0 && !string.IsNullOrEmpty(props[0]))
                {
                    // Get the id, zone and sort index.
                    int id = System.Convert.ToInt32(props[0]);
                    string zone = props[1];
                    int sortIndex = System.Convert.ToInt32(props[2]);
                    var widget = lookup[id];

                    // Update the widget zone and sort index.
                    if (widget != null)
                    {
                        widget.Zone = zone;
                        widget.SortIndex = sortIndex;
                        widgets.Add(widget);
                    }
                }
            }
            foreach (var widget in widgets)
                WidgetInstance.Save(widget);

            return new BoolMessage(true, "Order of widgets has been saved.");
        }


        /// <summary>
        /// Lamda used for post-processing of mapped records of widgetinstances.
        /// </summary>
        /// <param name="widgets"></param>
        public static void ReloadState(IList<WidgetInstance> widgets)
        {
            foreach (var widget in widgets) widget.LoadState();
        }
    }
}
