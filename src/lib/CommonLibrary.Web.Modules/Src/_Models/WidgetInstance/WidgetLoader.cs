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


namespace ComLib.Web.Modules.Widgets
{
    public class WidgetLoader
    {
        private IList<WidgetInstance> _allInstances;
        private LookupMulti<Widget> _widgets;

        /// <summary>
        /// Initialize the widget loader.
        /// </summary>
        public WidgetLoader()
        {
            Init();
        }
        

        /// <summary>
        /// Iterate over each widget/instance in the specified zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="executor"></param>
        public void ForEachInZone(string zone, Action<WidgetInstance> executor)
        {
            var instances = (from i in _allInstances
                                  where string.Compare(i.Zone, zone, true) == 0 && i.IsActive == true
                                  orderby i.SortIndex
                                  select i).ToList();

            foreach (var instance in instances)
            {
                var widget = _widgets[instance.DefName];
                instance.Definition = widget;
                executor(instance);
            }
        }


        /// <summary>
        /// Gets all the widget/instance in the specified zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="executor"></param>
        public IList<WidgetInstance> GetWidgetsInZone(string zone)
        {
            var instances = (from i in _allInstances
                             where string.Compare(i.Zone, zone, true) == 0 && i.IsActive == true
                             orderby i.SortIndex
                             select i).ToList();

            foreach (var instance in instances)
            {
                var widget = _widgets[instance.DefName];
                instance.Definition = widget;
            }
            return instances;
        }


        /// <summary>
        /// Load all the widgets and the instances associated w/ the tenant id( not implemented yet )
        /// </summary>
        private void Init()
        {
            _widgets = Widget.Lookup;          
            ToDo.Implement(ToDo.Priority.Normal, "Kishore", "This is slow in the RepositoryInMemory<T> implementation. So i'm getting all. Modification needed for MultiTenant", () =>
            {
                _allInstances = WidgetInstance.GetAll();
            });
        }
    }
}
