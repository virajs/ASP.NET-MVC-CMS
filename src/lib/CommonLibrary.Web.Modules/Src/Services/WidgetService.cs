using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Reflection;
using ComLib.Extensions;
using ComLib.Data;
using ComLib.Entities;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Services;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Modules.OptionDefs;


namespace ComLib.Web.Modules.Widgets
{
    /// <summary>
    /// Information Service
    /// </summary>
    public class WidgetService : ExtensionService<WidgetAttribute, WidgetInstance>
    {

        private IList<WidgetInstance> _allInstances;
        private LookupMulti<Widget> _widgets;

        public WidgetService()
        {
            Init();
        }


        /// <summary>
        /// Creates the specified widget using the definition name supplied.
        /// </summary>
        /// <param name="widgetDefinitionName">Name of the widget definition.</param>
        /// <returns></returns>
        public WidgetInstance Create(string defName)
        {
            Widget widget = Widget.Lookup[defName];
            Assembly assembly = Assembly.Load(widget.DeclaringAssembly);
            Type type = assembly.GetType(widget.DeclaringType);
            var instance = Activator.CreateInstance(type);
            var widgetInstance = instance as WidgetInstance;
            widgetInstance.DefName = defName;
            return widgetInstance;
        }


        /// <summary>
        /// Creates the specified widget using the definition name supplied.
        /// </summary>
        /// <param name="widgetDefinitionName">Name of the widget definition.</param>
        /// <returns></returns>
        public WidgetInstance CreateWithDefaultSettings(string defName)
        {
            var widget = Create(defName);
            widget.DefName = defName;
            widget.IsActive = true;
            widget.Header = defName;
            widget.SortIndex = 20;
            return widget;
        }


        /// <summary>
        /// Load all the widgets from the extension assemblies.
        /// </summary>
        /// <param name="assembliesDelimited"></param>
        public void Load(string assembliesDelimited)
        {
            string[] assemblies = assembliesDelimited.Split(',');
            var widgets = new List<KeyValuePair<Type, Widget>>();

            // 1. Process each assembly to find widgets.
            foreach (var assemblyname in assemblies) widgets.AddRange(LoadFromAssembly(assemblyname));

            // 2. Now get list of all the properties for each widget.
            var optiondefs = Convert(widgets);
            optiondefs.AddRange(LoadWidgetCommonProperties());

            // 4. Create the widget definitions.
            var widgetdefs = new List<Widget>();
            widgets.ForEach(pair => widgetdefs.Add(pair.Value));
            Widget.Create(widgetdefs, w => w.Name);

            // 5. Create the optiondefs 
            OptionDef.Repository.Delete(Query<OptionDef>.New().Where(o => o.Section).Like("W_", false, true));
            OptionDef.Create(optiondefs, o => o.Section, o => o.Key);
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


        #region Static helpers
        /// <summary>
        /// Load widgets from the assembly provided.
        /// </summary>
        /// <param name="assemblyname">Name of assembly to load widgets from.</param>
        /// <param name="author">Override author</param>
        /// <param name="email">Override email</param>
        /// <param name="version">Override version</param>
        /// <param name="url">Ovverride url</param>
        /// <returns></returns>
        public static IList<KeyValuePair<Type, Widget>> LoadFromAssembly(string assemblyname, string author = "kishore reddy", string email = "kishore_reddy@codeplex.com", string version = "0.9.4.1", string url = "http://commonlibrarynetcms.codeplex.com")
        {
            var widgets = AttributeHelper.GetClassAttributesFromAssembly<WidgetAttribute>("CommonLibrary.Web.Modules", pair =>
            {
                if (!string.IsNullOrEmpty(author)) pair.Value.Author = author;
                if (!string.IsNullOrEmpty(email)) pair.Value.Email = email;
                if (!string.IsNullOrEmpty(version)) pair.Value.Version = version;
                if (!string.IsNullOrEmpty(url)) pair.Value.Url = url;
                pair.Value.DeclaringType = pair.Key.FullName;
                pair.Value.DeclaringAssembly = assemblyname;

                // Now setup the included properties comma-delimited list.
                var props = LoadWidgetPropertiesToDisplay(new List<Type>() { pair.Key }, 1);
                string includedProps = EnumerableExtensions.JoinDelimited<KeyValuePair<PropertyInfo, PropertyDisplayAttribute>>(props, ",", item => item.Key.Name);
                pair.Value.IncludeProperties = includedProps;
            });
            var widgetdefs = new List<KeyValuePair<Type, Widget>>();
            widgets.ForEach(pair => widgetdefs.Add(new KeyValuePair<Type, Widget>(pair.Key, Convert(pair.Value))));
            return widgetdefs;
        }



        /// <summary>
        /// Processes the types( representing widgets ) and gets all the properties of each widget type that has properties that can be displayed edited.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static IList<KeyValuePair<PropertyInfo, PropertyDisplayAttribute>> LoadWidgetPropertiesToDisplay(IList<Type> types, int appid = 1)
        {
            var options = AttributeHelper.GetPropertiesWithAttributesOnTypes<PropertyDisplayAttribute>(types, (type, pair) =>
            {
                pair.Value.AppId = 1;
                pair.Value.Group = "W_" + type.Name;
                pair.Value.Name = pair.Key.Name;
                pair.Value.DataType = pair.Key.PropertyType.Name;
                pair.Value.IsBasicType = true;
                pair.Value.Order = pair.Value.Order * 10;
                if (string.IsNullOrEmpty(pair.Value.Label))
                    pair.Value.Label = pair.Key.Name;
            });
            return options;
        }


        public static IList<OptionDef> LoadWidgetCommonProperties()
        {
            var options = new List<OptionDef>()
            {
                new OptionDef(){ Key = "DefName", Section = "Widget", IsBasicType = true, IsRequired = true, SortIndex = 1},
                new OptionDef(){ Key = "Header", Section = "Widget", IsBasicType = true, IsRequired = true, SortIndex = 1, ValType = typeof(string).Name},
                new OptionDef(){ Key = "Zone", Section = "Widget", IsBasicType = true, IsRequired = true, SortIndex = 2, ValType = typeof(string).Name},
                new OptionDef(){ Key = "Roles", Section = "Widget", IsBasicType = true, IsRequired = true, SortIndex = 3, ValType = typeof(string).Name},
                new OptionDef(){ Key = "SortIndex", Section = "Widget", IsBasicType = true, IsRequired = true, SortIndex = 4, ValType = typeof(int).Name},
                new OptionDef(){ Key = "IsActive", Section = "Widget", IsBasicType = true, IsRequired = true, SortIndex = 5, ValType = typeof(bool).Name}
            };
            return options;
        }


        /// <summary>
        /// Converts a widgetattribute to a widget definition.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static Widget Convert(WidgetAttribute attribute)
        {
            Widget widget = new Widget();
            widget.Author = attribute.Author;
            widget.Email = attribute.Email;
            widget.DeclaringType = attribute.DeclaringType;
            widget.DeclaringAssembly = attribute.DeclaringAssembly;
            widget.IsCacheable = attribute.IsCachable;
            widget.Name = attribute.Name;
            widget.Path = attribute.Path;
            widget.SortIndex = attribute.SortIndex;
            widget.Url = attribute.Url;
            widget.Version = attribute.Version;
            widget.IncludeProperties = attribute.IncludeProperties;
            widget.IsCacheable = attribute.IsCachable;
            widget.IsEditable = attribute.IsEditable;
            return widget;
        }


        /// <summary>
        /// Converts a widgetattribute to a widget definition.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public static OptionDef Convert(PropertyDisplayAttribute attribute)
        {
            OptionDef option = new OptionDef();
            option.AppId = attribute.AppId;
            option.DefaultValue = attribute.DefaultValue;
            option.DisplayStyle = attribute.DisplayStyle;
            option.IsBasicType = true;
            option.Key = attribute.Name;
            option.Section = attribute.Group;
            option.SortIndex = attribute.Order;
            option.ValType = attribute.DataType;
            return option;
        }


        /// <summary>
        /// Gets all the properties of widgets that can be edited as optiondefs.
        /// </summary>
        /// <param name="widgets"></param>
        /// <returns></returns>
        public static IList<OptionDef> Convert(IList<KeyValuePair<Type, Widget>> widgets)
        {
            var types = new List<Type>();
            widgets.ForEach(pair => types.Add(pair.Key));
            var options = LoadWidgetPropertiesToDisplay(types, 1);
            var optiondefs = new List<OptionDef>();
            options.ForEach(pair => optiondefs.Add(Convert(pair.Value)));
            return optiondefs;
        }
        #endregion
    }
}
