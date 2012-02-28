using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Web.Lib.Services;
using ComLib.Web.Lib.Services.Information;
using ComLib.Web.Lib.Services.Macros;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Modules.Services;
using ComLib.Web.Lib.Core;
using ComLib.BootStrapSupport;
using ComLib.Scheduling;


namespace ComLib.CMS
{
    /// <summary>
    /// A light-weight hybrid service locator.
    /// </summary>
    public class CMS
    {
        private static IDictionary<Type, InstanceMetaData> _services = new Dictionary<Type, InstanceMetaData>();
        private class InstanceMetaData
        {
            public object Instance;
            public Func<object> Fetcher;
            public bool IsSingleton;
        }


        /// <summary>
        /// Register the instance of type t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void Register<T>(T instance) where T : class
        {
            var meta = new InstanceMetaData();
            meta.Instance = instance;
            meta.IsSingleton = true;
            _services[typeof(T)] = meta;
        }


        /// <summary>
        /// Register the instance of type t.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void Register<T>(Func<T> fetcher) where T : class
        {
            var meta = new InstanceMetaData();
            meta.IsSingleton = false;
            meta.Fetcher = fetcher;
            _services[typeof(T)] = meta;
        }


        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : class
        {
            var meta = _services[typeof(T)];
            if (meta.IsSingleton)
                return meta.Instance as T;

            var obj = meta.Fetcher();
            return obj as T;
        }
        

        /// <summary>
        /// Authentication service.
        /// </summary>
        public static IAuthenticationService Authentication
        {
            get { return Get<IAuthenticationService>(); }
        }
        

        /// <summary>
        /// Membership service.
        /// </summary>
        public static IMembershipService Membership
        {
            get { return Get<IMembershipService>(); }
        }


        /// <summary>
        /// Media service.
        /// </summary>
        public static IMediaService Media
        {
            get { return Get<IMediaService>(); }
        }
        

        /// <summary>
        /// Info service.
        /// </summary>
        public static IInformationService Info
        {
            get { return Get<IInformationService>(); }
        }


        /// <summary>
        /// Model service.
        /// </summary>
        public static ModelService Models
        {
            get { return Get<ModelService>(); }
        }


        /// <summary>
        /// Custom Tags service.
        /// </summary>
        public static IMacroService Macros
        {
            get { return Get<IMacroService>(); }
        }


        public static WidgetService Widgets
        {
            get { return Get<WidgetService>(); }
        }


        /// <summary>
        /// Get the scheduler service.
        /// </summary>
        public static SchedulerService Scheduler
        {
            get { return Get<SchedulerService>(); }
        }


        /// <summary>
        /// Get the configuration service.
        /// </summary>
        public static ConfigurationService Configs
        {
            get { return Get<ConfigurationService>(); }
        }


        /// <summary>
        /// Get dashboard instance.
        /// </summary>
        public static DashboardService Dashboard
        {
            get { return Get<DashboardService>(); }
        }


        /// <summary>
        /// Get the boot strapper.
        /// </summary>
        public static BootStrapper Bootup
        {
            get { return Get<BootStrapper>(); }
        }


        /// <summary>
        /// Gets the entity settings.
        /// </summary>
        public static EntitySettingsHelper EntitySettings
        {
            get { return Get<EntitySettingsHelper>(); }
        }
    }
}
