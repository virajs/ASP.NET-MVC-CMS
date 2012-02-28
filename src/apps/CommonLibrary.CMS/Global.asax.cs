using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Security;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;
using System.IO;

// CommonLibrary.NET Services.
using ComLib.Account;
using ComLib.Logging;
using ComLib.Data;
using ComLib.Caching;
using ComLib.IO;
using ComLib.Environments;
using ComLib.Configuration;
using ComLib.Cryptography;
using ComLib.Authentication;
using ComLib.EmailSupport;
using ComLib.Notifications;
using ComLib.Entities;
using ComLib.LocationSupport;
using ComLib.Extensions;
using ComLib.BootStrapSupport;
using ComLib.ImportExport;
using ComLib.Queue;
using ComLib.Reflection;
using ComLib.Scheduling;
using ComLib.MapperSupport;
using ComLib.Web.Services.Search;
using ComLib.Maps;
using ComLib.Feeds;

// CommonLibrary.NET Web Modules
using ComLib.Web.Lib;
using ComLib.Web.Lib.Settings;
using ComLib.Web.Modules;
using ComLib.Web.Modules.Handlers;
using ComLib.Web.Modules.Pages;
using ComLib.Web.Modules.Events;
using ComLib.Web.Modules.Links;
using ComLib.Web.Modules.Posts;
using ComLib.Web.Modules.Parts;
using ComLib.Web.Modules.Tags;
using ComLib.Web.Modules.Profiles;
using ComLib.Web.Modules.Feedbacks;
using ComLib.Web.Modules.MenuEntrys;
using ComLib.Web.Modules.Themes;
using ComLib.Web.Modules.Widgets;
using ComLib.Web.Modules.Articles;
using ComLib.Web.Modules.Services;
using ComLib.Web.Modules.Settings;
using ComLib.Web.Modules.Comments;
using ComLib.Web.Modules.Favorites;
using ComLib.Web.Modules.Resources;
using ComLib.Web.Modules.OptionDefs;
using ComLib.Web.Modules.Flags;
using ComLib.Web.Modules.Media;
using ComLib.Web.Modules.Categorys;

// CommonLibrary.NET ASP.NET MVC 2 Specific code.
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Services;
using ComLib.Web.Lib.Services.Information;
using ComLib.Web.Lib.Services.Macros;
using ComLib.Web.Lib.Attributes;
using ComLib.CMS.Areas.Admin;


namespace ComLib.CMS
{
    /// <summary>
    /// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    /// visit http://go.microsoft.com/?LinkId=9394801    
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Configure the application and then register all the routes.
        /// This is because after the configuration, the dynamic html pages will be processed in the routes.
        /// </summary>
        protected void Application_Start()
        {
            var app = new AppConfigurator();
            Configure(app);
        }


        /// <summary>
        /// This configures the application by initializing all the various services.
        /// 1. Set environment. There is dev and dev2 which correspond to the config files in \config directory.
        /// 2. Config files
        /// 3. Email notifications                
        /// 4. Repository configuration
        /// 5. Account Membership.
        /// 6. Entity/Repository initialization            
        /// 7. Authentication
        /// 8. Logging to In-Memory Database. Can be replaced w/ real db.            
        /// 9. Configure dashboard w/ the models.                            
        /// </summary>
        public void Configure(AppConfigurator app)
        {
            // IMPORTANT  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Once in a while you get a 404 - resource not found error. here is why
            // http://insomniacgeek.com/how-to-fix-resource-not-found-error-with-your-asp-net-mvc-project/

            // Initialize all the various services for the application including:
            string configDir = Server.MapPath(@"~/Config/Env");
            string _ENVIRONMENT_ = System.Configuration.ConfigurationManager.AppSettings["environment"];
            bool useRealData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["useRealData"]);
            bool clearData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["clearData"]);
            bool loadSchema = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["loadSchema"]);
            bool loadData = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["loadData"]);
            bool updateSchema = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["updateSchema"]);

            CMS.Register<BootStrapper>(new BootStrapper());
            Application["start_time"] = DateTime.Now;
            Application["model.settings"] = app.GetModelSettingsAsDictionary();

            // Non-Fluent API to bootup core bootup tasks for any app, web/non-web.
            CMS.Bootup.OnStart("Environment",       "core", BootTask.Importance.High,   false, (ctx) => Envs.Set(_ENVIRONMENT_, "prod,qa,dev2:dev,dev", "prod.config,qa.config,dev2.config,dev.config", true, true));
            CMS.Bootup.OnStart("Configuration",     "core", BootTask.Importance.High,   false, (ctx) => Config.Init(Configs.LoadFiles(configDir, Env.RefPath)));
            CMS.Bootup.OnStart("Database Config",   "core", BootTask.Importance.High,   false, (ctx) => RepositoryFactory.Add(Config.Get<string>("Database", "connectstr")));
            CMS.Bootup.OnStart("Forms Auth",        "core", BootTask.Importance.High,   false, (ctx) => FormsAuthentication.Initialize());
            CMS.Bootup.OnStart("Repositories",      "core", BootTask.Importance.High,   false, (ctx) => app.ConfigureRepositories(useRealData, ctx.Bag));
            CMS.Bootup.OnStart("Logs",              "core", BootTask.Importance.High,   false, (ctx) => Logger.Default.Replace(new LogDatabase("commons.mvc", "db", ctx.Bag["logRepo"] as IRepository<LogEventEntity>, LogLevel.Debug)));
            CMS.Bootup.OnStart("Database Clear",    "core", BootTask.Importance.High,   clearData, false, (ctx) => app.DeleteData());
            CMS.Bootup.OnStart("Database Schema",   "core", BootTask.Importance.High,   loadSchema, false, (ctx) => app.CreateSchema());
            CMS.Bootup.OnStart("Database Update",   "core", BootTask.Importance.High,   updateSchema, false, (ctx) => app.UpdateSchema());                
            CMS.Bootup.OnStart("Queues",            "core", BootTask.Importance.High,   false, (ctx) => app.ConfigureQueueProcessing());
            CMS.Bootup.OnStart("Schedule",          "core", BootTask.Importance.High,   false, (ctx) => app.ConfigureSchedules());
            CMS.Bootup.OnStart("Notifications",     "core", BootTask.Importance.High,   false, (ctx) => app.ConfigureNotifications());
            CMS.Bootup.OnStart("Auth1",             "core", BootTask.Importance.Low,    false, (ctx) => Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "admin", "Admin", "custom", true))));

            // Example of Fluent API.
            CMS.Bootup.OnStart(BootTask.Named("Services"    ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureServices()));
            CMS.Bootup.OnStart(BootTask.Named("Themes"      ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureThemes()));
            CMS.Bootup.OnStart(BootTask.Named("ImportExport").InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureImportExport()));
            CMS.Bootup.OnStart(BootTask.Named("Extensions"  ).InGroup("core").PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureExtensions()));
            CMS.Bootup.OnStart(BootTask.Named("Locations"   ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureLocations()));
            CMS.Bootup.OnStart(BootTask.Named("App Config"  ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureConfigs()));
            CMS.Bootup.OnStart(BootTask.Named("Dashboard"   ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureDashBoard())); 
            CMS.Bootup.OnStart(BootTask.Named("Modules"     ).InGroup("app").PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureModules()));
            CMS.Bootup.OnStart(BootTask.Named("Data Load"   ).If(loadData).InGroup("app" ).PriorityNormal.CanFail(  ).ActionIs((ctx) => app.LoadData()));
            CMS.Bootup.OnStart(BootTask.Named("Settings"    ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureSettings(ctx.Bag)));
            CMS.Bootup.OnStart(BootTask.Named("Web Search"  ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureSearch()));
            CMS.Bootup.OnStart(BootTask.Named("Web Maps"    ).InGroup("app" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureMaps()));
            CMS.Bootup.OnStart(BootTask.Named("MVC_Areas"   ).InGroup("mvc" ).PriorityHigh.MustSucceed().ActionIs((ctx) => AreaRegistration.RegisterAllAreas()));
            CMS.Bootup.OnStart(BootTask.Named("MVC_Routes"  ).InGroup("mvc" ).PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureRoutes()));
            CMS.Bootup.OnStart(BootTask.Named("Handlers"    ).InGroup("core").PriorityHigh.MustSucceed().ActionIs((ctx) => app.ConfigureHandlers()));
            CMS.Bootup.OnStart(BootTask.Named("Auth"        ).InGroup("core").PriorityHigh.MustSucceed().ActionIs((ctx) => Auth.Init(new AuthWeb("Admin"))));
            
            CMS.Bootup.StartUp(new AppContext());
            Article.Init(new RepositoryInMemory<Article>(), false);
            if(Article.Count() == 0 )
                for (int count = 1; count < 80; count++)
                    Article.Repository.Create(new Article() { Title = "article " + count, Author = "kishore_" + count, IsExternal = false, PublishDate = DateTime.Now, NumberOfComments = count });

            Logger.Info("Starting CMS");
        }


        /// <summary>
        /// Authenticate the request.
        /// Using Custom Forms Authentication since I'm not using the "RolesProvider".
        /// Roles are manually stored / encrypted in a cookie in <see cref="FormsAuthenticationService.SignIn"/>
        /// This takes out the roles from the cookie and rebuilds the Principal w/ the decrypted roles.
        /// http://msdn.microsoft.com/en-us/library/aa302397.aspx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            SecurityHelper.RebuildUserFromCookies();
        }


        /// <summary>
        /// Currently used for re-writing static pages urls.
        /// E.g. Instead of adding to routes "news", "aboutus". it's handled here a bit more dynamically.
        /// Otherwise, we would have to register each static page in the route table.
        /// Need to store the original url becuase an asp.net bug always puts in the
        /// physical url as the postback url for forms.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_BeginRequest(object sender, EventArgs e)
        {   
            // This needs to be optimized.
            // Set the theme. It's done here to allow Multi-Tenancy in the next version.
            Theme theme = Theme.GetActiveTheme();
            string masterpage = theme != null ? theme.SelectedLayout : "Site";
            string themename = theme != null ? theme.Name : "Sapphire";
            ViewHelper.SetThemeInCurrentRequest(themename, masterpage, theme);
            
            string filename = Path.GetFileName(HttpContext.Current.Request.RawUrl);
            if (!string.IsNullOrEmpty(filename))
            {
                if (MenuEntry.Lookup.ContainsKey(filename) || Page.Lookup.ContainsKey(filename))
                {
                    Context.Items["originalurl"] = HttpContext.Current.Request.RawUrl;
                    string url = "/page/showbyname/" + filename;
                    HttpContext.Current.RewritePath(url);
                }
            }
        }
    }



    public class AppConfigurator
    {
        /// <summary>
        /// Configures the various services used in the CMS.
        /// </summary>
        public void ConfigureServices()
        {
            // These are ok as SINGLETONS ( Not implemented as singletons however )
            // The CMS class is very lightweight ServiceLocator.
            // NOTE: A non-singleton can be registered via CMS.Register<T>( Func<T> fetcher);
            CMS.Register<EntitySettingsHelper>(() => GetModelSettingsAsHelper());
            CMS.Register<IAuthenticationService>(new AuthenticationService());
            CMS.Register<IInformationService>(new InformationService());
            CMS.Register<IMembershipService>(new MembershipService());
            CMS.Register<IMacroService>(new MacroService());
            CMS.Register<IMediaService>(new MediaService());
            CMS.Register<WidgetService>(new WidgetService());
            CMS.Register<DashboardService>(new DashboardService());
            CMS.Register<ConfigurationService>(new ConfigurationService());
        }


        /// <summary>
        /// Loads all information tasks
        /// </summary>
        /// <param name="summary"></param>
        public void ConfigureExtensions()
        {
            string assemblies = System.Configuration.ConfigurationManager.AppSettings["assembliesForExtensions"];
            CMS.Info.Load(assemblies);
            CMS.Macros.Load(assemblies);
            CMS.Widgets.Load(assemblies);

            // Now register how the recent widget should handle recent items for different entities.
            ComLib.CMS.Models.Widgets.PostRecent.Init(
                new KeyValuePair<Type, Func<int, IList<IPublishable>>>(typeof(Post), (pageSize) => Post.Repository.FindRecentAs<IPublishable>(1, pageSize)),
                 new KeyValuePair<Type, Func<int, IList<IPublishable>>>(typeof(Event), (pageSize) => Event.Repository.FindRecentAs<IPublishable>(1, pageSize)));
        }


        /// <summary>
        /// Create the sql express|enterprise database schema using the code-generation component.
        /// </summary>
        public void CreateSchema()
        {
            //string con = @"data source=.\SQLEXPRESS;AttachDbFileName=|DataDirectory|cms.mdf;Integrated Security=True;User Instance=True";
            string con = Config.Get<string>("Database", "connectstr");
            WebModels models = new WebModels(new ConnectionInfo(con));
            CodeGeneration.CodeBuilder.CreateDatabase(models.GetModelContext());
        }


        public void UpdateSchema()
        {
            // 1. add declaringtype to widget defs
            // 2. add declaringassembly to widget defs.
            // 3. add description to widget defs.
            // 3. add pathtoeditor to widget defs.
            string con = Config.Get<string>("Database", "connectstr");
            string[] sqlupdates = new string[]
            {
                "ALTER TABLE Events ADD [HasMediaFiles] [bit] NULL; ",
                "ALTER TABLE Events ADD [TotalMediaFiles] [int] NULL;",
                "ALTER TABLE Posts ADD [HasMediaFiles] [bit] NULL; ",
                "ALTER TABLE Posts ADD [TotalMediaFiles] [int] NULL;",
                "ALTER TABLE Profiles ADD [HasMediaFiles] [bit] NULL; ",
                "ALTER TABLE Profiles ADD [TotalMediaFiles] [int] NULL;",
                "ALTER TABLE Widgets ADD [Description] [ntext] NULL, [PathToEditor] [nvarchar](150) NULL, [DeclaringType] [nvarchar](150) NULL, [DeclaringAssembly] [nvarchar](150) NULL; ",
                "ALTER TABLE MediaFiles ADD [Title] [nvarchar](100) NULL;",
                "ALTER TABLE MediaFiles ALTER COLUMN [Description] [nvarchar](200) NULL;",
                "ALTER TABLE MediaFolders ALTER COLUMN [Description] [nvarchar](200) NULL;",
                "ALTER TABLE MediaFolders ADD [HasMediaFiles] [bit] NULL; ",
                "ALTER TABLE MediaFolders ADD [TotalMediaFiles] [int] NULL;",
                "UPDATE users SET createuser = username, updateuser = username"
            };
            var dbhelper = new Database(con);
            var errors = new Errors();
            foreach (string sqlupdate in sqlupdates)
            {
                Try.Catch(
                    () => dbhelper.ExecuteNonQueryText(sqlupdate), 
                    (ex) => errors.Add("Error updateing schema with command : " + sqlupdate + ". " + ex.Message));
            }
        }


        /// <summary>
        /// Deletes all data from the storage.
        /// </summary>
        public void DeleteData()
        {
            Try.CatchLog(() => Tag.DeleteAll());
            Try.CatchLog(() => Profile.DeleteAll());
            Try.CatchLog(() => User.DeleteAll());
            Try.CatchLog(() => Page.DeleteAll());
            Try.CatchLog(() => Part.DeleteAll());
            Try.CatchLog(() => Link.DeleteAll());
            Try.CatchLog(() => WidgetInstance.DeleteAll());
            Try.CatchLog(() => Widget.DeleteAll());
            Try.CatchLog(() => Post.DeleteAll());
            Try.CatchLog(() => Event.DeleteAll());
            Try.CatchLog(() => Category.DeleteAll());
            Try.CatchLog(() => Theme.DeleteAll());
            Try.CatchLog(() => Comment.DeleteAll());
            Try.CatchLog(() => Resource.DeleteAll());
            Try.CatchLog(() => Favorite.DeleteAll());
            Try.CatchLog(() => MenuEntry.DeleteAll());
            Try.CatchLog(() => OptionDef.DeleteAll());
            Try.CatchLog(() => MediaFile.DeleteAll());
            Try.CatchLog(() => MediaFolder.DeleteAll());
            Try.CatchLog(() => LogEventEntity.DeleteAll());
            Try.CatchLog(() => EntityRegistration.GetRepository<City>().DeleteAll());
            Try.CatchLog(() => EntityRegistration.GetRepository<State>().DeleteAll());
            Try.CatchLog(() => EntityRegistration.GetRepository<Country>().DeleteAll());
        }


        /// <summary>
        /// Load data from the csv/ini files into the appropriate entity repos.
        /// This does a conditional load, meaning the data from the csv/ini files are ONLY
        /// saved into the system if there are no existing records w/ matching fields.
        /// E.g. In the case of the Link model, it's only saved to the system if there is 
        /// NO record with matching values for Name, Group, and Url fields.
        /// </summary>
        public void LoadData()
        {
            var server = HttpContext.Current.Server;
            // NOTE: Location data.
            // 1. The city, state, country classes do NOT extend from ActiveRecordBaseEntity<T> so can not use the static Create methods.
            // 2. However, since the underlying implementation of ActiveRecordEntity<T> uses IEntityService<T>, this is the perfect use case for using IEntityService<T> directly.
            // 3. The order of creation MUST be Countries, States, Cities, since the state references a countryid, and city references the state,countryid.
            EntityRegistration.GetService<Country>().Create(Mapper.MapConfigFile<Country>(server.MapPath("~/config/data/Location_countries.csv.config")), c => c.Name);
            EntityRegistration.GetService<State>().Create(Mapper.MapConfigFile<State>(server.MapPath("~/config/data/Location_states.csv.config")), c => c.Name, c => c.CountryName);
            EntityRegistration.GetService<City>().Create(Mapper.MapConfigFile<City>(server.MapPath("~/config/data/Location_cities.csv.config")), c => c.Name, c => c.StateName, c => c.CountryName);
            
            // NOTE: The .MapConfigFile removes the .config extension from the file name (profiles.csv.config)
            //       and determines the extension as .csv and calls the csv mapper.
            User.Create(Mapper.MapConfigFile<User>(server.MapPath("~/config/data/users.csv.config")), u => u.UserName);
            Page.Create(Mapper.MapConfigFile<Page>(server.MapPath("~/config/data/pages.xml.config")), p => p.Title, p => p.Slug);
            Part.Create(Mapper.MapConfigFile<Part>(server.MapPath("~/config/data/parts.xml.config")), r => r.Title);
            Link.Create(Mapper.MapConfigFile<Link>(server.MapPath("~/config/data/links.csv.config")), l => l.Name, l => l.Group, l => l.Url);
            Profile.Create(Mapper.MapConfigFile<Profile>(server.MapPath("~/config/data/profiles.csv.config")), true, u => u.UserName);
            Resource.Create(Mapper.MapConfigFile<Resource>(server.MapPath("~/config/data/resources.csv.config")), r => r.Language, r => r.ResourceType, r => r.Section, r => r.Key);
            OptionDef.Create(Mapper.MapConfigFile<OptionDef>(server.MapPath("~/config/data/optiondefs.csv.config")), o => o.Section, o => o.Key);
            Widget.Create(Mapper.MapConfigFile<Widget>(server.MapPath("~/config/data/widgetdefs.csv.config")), w => w.Name);
            Category.Create(Mapper.MapConfigFile<Category>(server.MapPath("~/config/data/categories.csv.config")), true, c => c.Name, c => c.Group, c => c.ParentId);
            Post.Create(Mapper.MapConfigFile<Post>(server.MapPath("~/config/data/posts.ini.config")), p => p.Title, p => p.CreateUser);
            Event.Create(Mapper.MapConfigFile<Event>(server.MapPath("~/config/data/events.ini.config")), e => e.Title, e => e.CreateUser);
            Theme.Create(Mapper.MapConfigFile<Theme>(server.MapPath("~/config/data/themes.csv.config")), t => t.Name);
            Comment.Create(Mapper.MapConfigFile<Comment>(server.MapPath("~/config/data/comments.csv.config")), c => c.GroupId, c => c.RefId, c => c.Email);
            Favorite.Create(Mapper.MapConfigFile<Favorite>(server.MapPath("~/config/data/favorites.csv.config")), f => f.RefId, f => f.Url);
            MenuEntry.Create(Page.FrontPagesAsMenuEntrys().AddRange(Mapper.MapConfigFile<MenuEntry>(server.MapPath("~/config/data/menu.csv.config"))), m => m.Name);
            MediaFolder.Create(Mapper.MapConfigFile<MediaFolder>(server.MapPath("~/config/data/mediafolders.csv.config")), m => m.Name);
            MediaFile.Create(Mapper.MapConfigFile<MediaFile>(server.MapPath("~/config/data/mediafiles.csv.config")), true);
            WidgetInstance.Create(Mapper.MapIniAs<WidgetInstance>(server.MapPath("~/config/data/widgets.ini.config"), true, "DefName", (defName) => WidgetHelper.Create(defName)), true);
            
            if (LogEventEntity.Count() == 0)
                ((int)40).Times(ndx => Logger.Default.Log(LogLevel.Info, "Testing logging"));
        }


        /// <summary>
        /// Configures the routes for MVC.
        /// NOTE: The order of these may need to be changed to slightly.
        /// </summary>
        public void ConfigureRoutes()
        {
            RouteCollection routes = RouteTable.Routes;
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("media/{*pathInfo}");
            routes.MapRoute("Pages",    "page/showbyname/{pagename}", new { controller = "Page", action = "ShowByName", pagename = "" });
            routes.MapRoute("ProfileN", "profile/show/{username}", new { controller = "Profile", action = "Show", username = "" });
            routes.MapRoute("Profiles", "profile/editprofilebyname/{username}", new { controller = "Profile", action = "EditProfileByName", username = "" });
            routes.MapRoute("Feedback", "feedback/", new { controller = "Feedback", action = "Create" });
            routes.MapRoute("Account",  "account/{action}", new { controller = "Account", action = "" });
            routes.MapRoute("Tag",      "{controller}/tags/{tag}", new { controller = "Home", action = "Tags", tag = "" });
            routes.MapRoute("Archive",  "{controller}/archives/{year}/{month}", new { controller = "Home", action = "Archives", year = "", month = "" });
            routes.MapRoute("Index",    "{controller}/index/{page}", new { controller = "Home", action = "Index", page = "" });
            routes.MapRoute("IndexM",   "{controller}/indexmanage/{page}", new { controller = "Home", action = "IndexManage", page = "" });
            routes.MapRoute("Manage",   "{controller}/manage/{page}", new { controller = "Home", action = "Manage", page = "" });
            routes.MapRoute("Seo",      "{controller}/show/{title}", new { controller = "Home", action = "Show", title = "" });
            routes.MapRoute("Default",  "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });

            // Handle layout/master page setup.
            ViewHelper.ExcludeLayoutSetupOnActions("create", "copy", "edit", "manage", "changepassword", "editprofilebyname");
            UrlHelperExtensions.AdminAreas["country"] = true;
            UrlHelperExtensions.AdminAreas["state"] = true;
            UrlHelperExtensions.AdminAreas["city"] = true;
            UrlHelperExtensions.AdminAreas["user"] = true;
            UrlHelperExtensions.AdminAreas["log"] = true;
        }


        /// <summary>
        /// Saves all the data from the internal system to csv files.
        /// </summary>
        public void SaveData()
        {
            IErrors errors = new Errors();
            List<string> excludeProps = new List<string>() { "Settings", "Errors", "IsValid" };
            Mapper.MapToCsv<User>(User.GetAll(), "config/data/users.csv", errors, excludeProps);
            Mapper.MapToCsv<Page>(Page.GetAll(), "config/data/pages.csv", errors, excludeProps);
            Mapper.MapToCsv<Part>(Part.GetAll(), "config/data/parts.csv", errors, excludeProps);
            Mapper.MapToCsv<Link>(Link.GetAll(), "config/data/links.csv", errors, excludeProps);
            Mapper.MapToCsv<Widget>(Widget.GetAll(), "config/data/widgets.csv", errors, excludeProps);
            Mapper.MapToCsv<MenuEntry>(MenuEntry.GetAll(), "config/data/menuentrys.csv", errors, excludeProps);
            Mapper.MapToCsv<WidgetInstance>(WidgetInstance.GetAll(), "config/data/widgetinstances.csv", errors, excludeProps);
            Mapper.MapToCsv<Profile>(Profile.GetAll(), "config/data/profiles.csv", errors, excludeProps);
            Mapper.MapToCsv<Theme>(Theme.GetAll(), "config/data/themes.csv", errors, excludeProps);
            Mapper.MapToCsv<Comment>(Comment.GetAll(), "config/data/comments.csv", errors, excludeProps);
        }


        /// <summary>
        /// Confiugure modules
        /// </summary>
        public void ConfigureModules()
        {
            var models = GetModelDefs();
            foreach (var model in models)
                ModuleMap.Instance.Register(model.Model, model.Id);
            
            ModuleMap.Instance.Register(typeof(User), 0);
            ModuleMap.Instance.Register(typeof(Profile), 4);
            ModuleMap.Instance.Register(typeof(Widget), 5);
            ModuleMap.Instance.Register(typeof(Theme), 8);
            ModuleMap.Instance.Register(typeof(OptionDef), 13);
            ModuleMap.Instance.Register(typeof(MediaFolder),14);
            ModuleMap.Instance.Register(typeof(MediaFile), 15);
            ModuleMap.Instance.Register(typeof(WidgetInstance), 16);
        }


        /// <summary>
        /// Set up the Import / Export services for the entities that are importable/exportable.
        /// </summary>
        public void ConfigureImportExport()
        {            
            var io = ImportExports.Instance;
            io.Register<Post>(new ImportExportServiceEntityBased<Post>("ini"));
            io.Register<Page>(new ImportExportServiceEntityBased<Page>("ini"));
            io.Register<Part>(new ImportExportServiceEntityBased<Part>("ini"));
            io.Register<Link>(new ImportExportServiceEntityBased<Link>("ini,csv"));            
            io.Register<Event>(new ImportExportServiceEntityBased<Event>("ini"));
            io.Register<MenuEntry>(new ImportExportServiceEntityBased<MenuEntry>("ini,csv"));
            io.Register<WidgetInstance>(new ImportExportServiceEntityBased<WidgetInstance>("ini"));
        }


        /// <summary>
        /// Configure the location service to parsing city/city,state/city,country text combinations.
        /// </summary>
        public void ConfigureLocations()
        {
            Location.Init(new LocationService(() => EntityRegistration.GetRepository<Country>(),
                                              () => EntityRegistration.GetRepository<State>(),
                                              () => EntityRegistration.GetRepository<City>()));
        }


        /// <summary>
        /// Configures the dashboard with the Models that can be managed( Add,Edit,Index,Delete).
        /// Also, add links for configuration settings for each model.
        /// </summary>
        public void ConfigureConfigs()
        {
            // Settings by category.
            CMS.Configs.Register<PostSettings>(Post.Settings, "post", "Post", "Settings for Blog Posts");
            CMS.Configs.Register<EventSettings>(Event.Settings, "event", "Event", "Settings for Events");
            CMS.Configs.Register<GeneralSettings>(SiteSettings.Instance.Site, "general", "General", "Site Settings");
            CMS.Configs.Register<IEmailSettings>(SiteSettings.Instance.Email, "email", "Email", "Email Settings");
        }


        /// <summary>
        /// Configures the dashboard with the Models that can be managed( Add,Edit,Index,Delete).
        /// Also, add links for configuration settings for each model.
        /// </summary>
        public void ConfigureDashBoard()
        {
            var models = GetModelDefs();
            CMS.Dashboard.Models.AddRange(models);
            CMS.Dashboard.Sections.AddRange(new List<Menu>()
            {
                new Menu("System", isRolesEnabled: true, roles: "Admin") { Items = new List<MenuNode> {
                        new MenuNode("Summary",     navigateUrl: "/admin/console/getinfo?name=System"),
                        new MenuNode("Startup",     navigateUrl: "/admin/console/getinfo?name=Startup"),
                        new MenuNode("Widgets",     navigateUrl: "/admin/console/getinfo?name=Widgets"),
                        new MenuNode("Macros",      navigateUrl: "/admin/console/getinfo?name=Macros"),
                        new MenuNode("Cache",       navigateUrl: "/admin/cache/index"),
                        new MenuNode("Logs",        navigateUrl: "/admin/log/index"),
                        new MenuNode("Users",       navigateUrl: "/admin/user/manage"),
                        new MenuNode("Diagnostics", navigateUrl: "/admin/diagnostics/index"),
                        new MenuNode("Email",       navigateUrl: "/admin/console/email"),
                        new MenuNode("Queues",      navigateUrl: "/admin/queue/index"),
                        new MenuNode("Tasks",       navigateUrl: "/admin/task/index"),
                        new MenuNode("Flags",       navigateUrl: "/flag/manage")
                    }
                },
                new Menu("Settings", isRolesEnabled: true, roles: "Admin"),
                new Menu("Appearence", isRolesEnabled: true, roles: "Admin" ) { Items = new List<MenuNode>{
                        new MenuNode("Themes",      navigateUrl: "/theme/manage"),
                        new MenuNode("Layouts",     navigateUrl: "/theme/layouts"),
                        new MenuNode("Css",         navigateUrl: "/theme/EditCss"),
                        new MenuNode("Widgets",     navigateUrl: "/widget/manage")
                    }
                },
                new Menu("Media", isRolesEnabled: true, roles: "Admin" ) { Items = new List<MenuNode> {
                        new MenuNode("Manage",      navigateUrl: "/mediafolder/manage"),
                    }
                },
                new Menu("Location", isRolesEnabled: true, roles: "Admin") { Items = new List<MenuNode> {
                        new MenuNode("City",        navigateUrl:  "/admin/city/manage"),
                        new MenuNode("State",       navigateUrl:  "/admin/state/manage"),
                        new MenuNode("Country",     navigateUrl:  "/admin/country/manage")
                    }
                },
                new Menu("Misc", isRolesEnabled: true, roles: "*") { Items = new List<MenuNode> {
                        new MenuNode("Favorites",   isRolesEnabled: true, roles: "*",     navigateUrl: "/favorite/manage"),
                        new MenuNode("Feedback",    isRolesEnabled: true, roles: "Admin", navigateUrl: "/feedback/manage"),
                        new MenuNode("Comments",    isRolesEnabled: true, roles: "*",     navigateUrl: "/comment/manage")
                    }
                },
                new Menu("Tools", isRolesEnabled: true, roles: "*") { Items = new List<MenuNode> {
                        new MenuNode("Import",      navigateUrl: "/tools/import"),
                        new MenuNode("Export",      navigateUrl: "/tools/export")
                    }
                }
            });
            
            CMS.Dashboard.AdminRole = "Admin";

            // Add the configs to the menu.            
            CMS.Configs.ForEach((config) => 
                CMS.Dashboard.SectionNamed("Settings").Items.Add( 
                    new MenuNode(config.DisplayName,  navigateUrl: "/admin/config/Edit?configname=" + config.Id) ));

            CMS.Dashboard.Init(() => GetModelSettingsAsHelper());
        }


        /// <summary>
        /// Configures the search component to allow searching of internet or this site.
        /// </summary>
        public void ConfigureSearch()
        {
            string source = Config.Get<string>("Search", "Provider");
            string url = Config.Get<string>("Search", source + ".Url");
            string key = Config.Get<string>("Search", source + ".APIKey");
            var provider = new SearchEngineBing(new SearchSettings(){ Url = url, AppId = key });
            SearchEngine.Init(provider);
        }


        /// <summary>
        /// Configure all the available themes in the sytem.
        /// Each theme is associated w/ a MasterPage name, and has a reference image.
        /// Themes can be changed dynamically.
        /// </summary>
        public void ConfigureThemes()
        {
            List<Layout> layouts = new List<Layout>()
            {
                new Layout(){ Name = "Single Side Bar Left",     MasterName = "Site.SingleBarLeft",  ImagePage = "/Content/images/generic/layout_1bar_left.png" },
                new Layout(){ Name = "Single Side Bar Right",    MasterName = "Site.SingleBarRight", ImagePage = "/Content/images/generic/layout_1bar_right.png" },
                new Layout(){ Name = "Double Side Bar Left",     MasterName = "Site.DoubleBarLeft",  ImagePage = "/Content/images/generic/layout_2bar_left.png" },
                new Layout(){ Name = "Double Side Bar Left",     MasterName = "Site.DoubleBarRight", ImagePage = "/Content/images/generic/layout_2bar_right.png" },
                new Layout(){ Name = "Double Side Bar Opposite", MasterName = "Site",                ImagePage = "/Content/images/generic/layout.png" }
            };
            Theme.SetLayouts(layouts);
        }


        /// <summary>
        /// This configures processing queues.
        /// </summary>
        public void ConfigureQueueProcessing()
        {
            // Applying tags is queued up and handled. Instead of applying them for each post in realtime.
            Queues.AddProcessorFor<TagsEntry>(15, items => TagHelper.ProcessQueue(items));
        }


        /// <summary>
        /// This configures processing queues.
        /// </summary>
        public void ConfigureMaps()
        {
            string source = Config.Get<string>("Maps", "Provider");
            string url = Config.Get<string>("Maps", source + ".Url");
            string key = Config.Get<string>("Maps", source + ".APIKey");
            GeoProvider.Init(new GeoProvider() { Name = source, SourceUrl = url, ApiKey = key });
        }


        /// <summary>
        /// This handles the configuration settings for each model.
        /// They can be either saved/loaded from the datastore.
        /// </summary>
        /// <param name="ctxBag"></param>
        public void ConfigureSettings(IDictionary<string, object> ctxBag)
        {
            int count = ConfigItem.Count();
            IRepository<ConfigItem> repo = ctxBag["configRepo"] as IRepository<ConfigItem>;
            SiteSettings.Instance.Site.SetRepository(null, repo);
            SiteSettings.Instance.Posts.SetRepository(null, repo);
            SiteSettings.Instance.Events.SetRepository(null, repo);
            SiteSettings.Instance.Email = EmailHelper.GetSettings(Config.Current, "EmailSettings", () => new ComLib.Web.Lib.Settings.EmailSettings());
                
            // First time load -> nothing in database.
            if (count == 0)
            {                
                SiteSettings.Instance.Site.Title = Config.Get<string>("App", "name");
                SiteSettings.Instance.Site.Description = Config.Get<string>("App", "description");
                SiteSettings.Instance.Site.IsLogoEnabled = true;
                SiteSettings.Instance.Site.LogoPath = "/content/images/common/logo.gif";
                SiteSettings.Instance.Site.StatsEnabled = Config.Get<bool>("Analytics", "IsEnabled");
                SiteSettings.Instance.Site.StatsAccountId = Config.Get<string>("Analytics", "APIKey");
                SiteSettings.Instance.Site.Author = Config.Get<string>("App", "name");
                SiteSettings.Instance.Posts.Save();
                SiteSettings.Instance.Site.Save();
                
            }
            else
            {
                SiteSettings.Instance.Site.Load();
                SiteSettings.Instance.Posts.Load();
                SiteSettings.Instance.Events.Load();                
            }
        }


        /// <summary>
        /// Configures the task scheduler.
        /// </summary>
        public void ConfigureSchedules()
        {
            Scheduler.Schedule("apply-tags", new Trigger().Every(((int)20).Seconds()), true, () => Queues.Process<TagsEntry>());
            Scheduler.Schedule("send-emails", new Trigger().Every(((int)40).Seconds()), true, () => Notifier.Process());
            Scheduler.Schedule("widget-meta-reload", new Trigger().OnDemand(), () => AppConfigurator.ReloadWidgetMetaData());
        }


        /// <summary>
        /// Configure the notification emailer.
        /// </summary>
        public void ConfigureNotifications()
        {
            Notifier.Init(new EmailService(Config.Current, "EmailSettings"), new NotificationSettings());
            Notifier.Settings["website.name"] = Config.Get<string>("Notifications", "WebSite.Name");
            Notifier.Settings["website.url"] = Config.Get<string>("Notifications", "WebSite.Url");
            Notifier.Settings["website.urls.contactus"] = Config.Get<string>("Notifications", "WebSite.Urls.ContactUs");
            Notifier.Settings.EnableNotifications = Config.Get<bool>("Notifications", "IsEnabled");
            Notifier.Settings.TemplateFolderPath = HttpContext.Current.Server.MapPath("~/config/notifications/");
        }


        /// <summary>
        /// Configure the repositories.
        /// Use In-Memory repositories if faking the storage.
        /// </summary>
        /// <param name="useFake"></param>
        /// <param name="args"></param>
        public IDictionary<string, object> ConfigureRepositories(bool useRealData, IDictionary<string, object> repos)
        {
            UserSettings settings = new UserSettings();
            settings.UserNameRegEx = @"[a-zA-Z1-9\._]{3,15}";
            settings.PasswordRegEx = "[a-zA-Z1-9]{5,15}";

            if (useRealData)
                ConfigureRepositories(settings, repos);
            else
                ConfigureRepositoriesInMemory(settings, repos);

            return repos;
        }


        public void ConfigureHandlers()
        {
            // Set up overridable image handlers so that images from the database can be handled by the MediaHelper methods
            // Allow the ImageHandler to handle file-system based images.
            ComLib.Web.HttpHandlers.ImageHandler.InitExternalHandler(
                (ctx) => MediaHelper.CanHandleMediaFile(ctx), 
                (ctx) => MediaHelper.HandleMediaFile(ctx));

            CommonController.SetMediaHandler(CMS.Media);
            CommonController.SetSettingsHandler(() => SiteSettings.Instance.Site);
            CommonController.SetEntitySecurityHandler(() => GetModelSettingsAsHelper());
            CommonController.SetModelSettingsHandler(() =>
            {
                var settings = Cacher.Get<IDictionary>("model.settings", true, 300, false, () =>
                {
                    var doc = new IniDocument(HttpContext.Current.Server.MapPath("~/config/data/models.ini.config"), true);
                    return doc as IDictionary;
                });
                return settings;
            });
        }


        public static void ReloadWidgetMetaData()
        {
            var server = HttpContext.Current.Server;
            Resource.DeleteAll();
            OptionDef.DeleteAll();
            Widget.DeleteAll();
            Resource.Create(Mapper.MapConfigFile<Resource>(server.MapPath("~/config/data/resources.csv.config")), r => r.Language, r => r.ResourceType, r => r.Section, r => r.Key);
            OptionDef.Create(Mapper.MapConfigFile<OptionDef>(server.MapPath("~/config/data/optiondefs.csv.config")), o => o.Section, o => o.Key);
            Widget.Create(Mapper.MapConfigFile<Widget>(server.MapPath("~/config/data/widgetdefs.csv.config")), w => w.Name);            
        }


        private void ConfigureRepositories(UserSettings userSettings, IDictionary<string, object> ctx)
        {
            // Real Database setup.
            // 1. The Repository<T> base class will automatically default the IDatabase to Database class.
            // 2. The RepositoryFactory will configure the connection, since the last parameter is "true".                
            string con = Config.Get<string>("Database", "connectstr");
            var logrepo = new LogRepository(con) { TableName = "logs" };
            var configRepo = new ConfigItemRepository() { TableName = "configs" };
            ctx["logRepo"] = logrepo;
            ctx["configRepo"] = configRepo;

            LogEventEntity.Init(new EntityService<LogEventEntity>(logrepo, null, null), true);
            ConfigItem.Init(() => configRepo, true);
            ComLib.Account.User.Init(() => new UserService(), () => new UserRepository(), () => new UserValidator(), userSettings, true, null);
            WidgetInstance.Init(() => new WidgetInstanceRepository() { OnRowsMappedCallBack = new Action<IList<WidgetInstance>>(WidgetHelper.ReloadState) }, true);
            Widget.Init(() => new WidgetRepository(), true);
            MediaFolder.Init(() => new MediaFolderRepository(), true);
            MediaFile.Init(() => new MediaFileRepository(), true);
            Feedback.Init(() => new FeedbackRepository(), true);
            Favorite.Init(() => new FavoriteRepository(), true);
            OptionDef.Init(() => new OptionDefRepository(), true);
            Resource.Init(() => new ResourceRepository(), true);
            MenuEntry.Init(() => new MenuEntryRepository(), true);
            Profile.Init(() => new ProfileRepository(), true);
            Category.Init(() => new CategoryRepository(), true);
            Comment.Init(() => new CommentRepository(), true);
            Theme.Init(() => new ThemeRepository(), true);
            Link.Init(() => new LinkRepository(), true);
            Event.Init(() => new EventRepository() { OnRowsMappedCallBack = (events) => EventHelper.ApplyCategories(events) }, true);
            Post.Init(() => new PostRepository() { OnRowsMappedCallBack = (posts) => PostHelper.ApplyCategories(posts) }, Post.Settings, true);
            Page.Init(() => new PageRepository(), true);
            Part.Init(() => new PartRepository(), true);
            Flag.Init(() => new FlagRepository(), true);
            Tag.Init(() => new TagRepository(), true);
            
            // Location registration.
            EntityRegistration.Register<Country>(new LocationEntityService<Country>(new CountryRepository()), true);
            EntityRegistration.Register<State>(new LocationEntityService<State>(new StateRepository()), true);
            EntityRegistration.Register<City>(new LocationEntityService<City>(new CityRepository()), true);            
        }


        private void ConfigureRepositoriesInMemory(UserSettings userSettings, IDictionary<string, object> ctx)
        {
            // Log and Config need to be passed back to caller.
            var logrepo = new RepositoryInMemory<LogEventEntity>("Id,CreateDate,UserName,LogLevel") { TableName = "logs" };
            var configRepo = new RepositoryInMemory<ConfigItem>() { TableName = "configs" };
            var userRepo = new RepositoryInMemory<User>();
            ctx["logRepo"] = logrepo;
            ctx["configRepo"] = configRepo;

            LogEventEntity.Init(new EntityService<LogEventEntity>(logrepo, null, new EntitySettings<LogEventEntity>()));
            ConfigItem.Init(() => configRepo, true);            
            ComLib.Account.User.Init(() => new UserService(), () => userRepo, () => new UserValidator(), userSettings, true, null);
            WidgetInstance.Init(new RepositoryInMemory<WidgetInstance>() { OnRowsMappedCallBack = new Action<IList<WidgetInstance>>(WidgetHelper.ReloadState) }, true);
            Widget.Init(new RepositoryInMemory<Widget>(), true);
            MediaFolder.Init(new RepositoryInMemory<MediaFolder>(), true);
            MediaFile.Init(new RepositoryInMemory<MediaFile>(), true);
            Feedback.Init(new RepositoryInMemory<Feedback>(), true);
            Favorite.Init(new RepositoryInMemory<Favorite>(), true);
            OptionDef.Init(new RepositoryInMemory<OptionDef>(), true);
            Resource.Init(new RepositoryInMemory<Resource>(), true);
            MenuEntry.Init(new RepositoryInMemory<MenuEntry>(), true);
            Profile.Init(new RepositoryInMemory<Profile>(), true);
            Category.Init(new RepositoryInMemory<Category>(), true);
            Comment.Init(new RepositoryInMemory<Comment>(), true);
            Theme.Init(new RepositoryInMemory<Theme>(), true);
            Link.Init(new RepositoryInMemory<Link>(), true);
            Event.Init(new RepositoryInMemory<Event>("Id,CreateUser,Title,StartDate") { OnRowsMappedCallBack = (events) => EventHelper.ApplyCategories(events) }, true);
            Post.Init(new RepositoryInMemory<Post>() { OnRowsMappedCallBack = (posts) => PostHelper.ApplyCategories(posts) }, true);
            Page.Init(new RepositoryInMemory<Page>(), true);
            Part.Init(new RepositoryInMemory<Part>(), true);
            Flag.Init(new RepositoryInMemory<Flag>(), true);
            Tag.Init(new RepositoryInMemory<Tag>(), true);
            EntityRegistration.Register<Country>(new LocationEntityService<Country>(new RepositoryInMemory<Country>()), true);
            EntityRegistration.Register<State>(new LocationEntityService<State>(new RepositoryInMemory<State>()), true);
            EntityRegistration.Register<City>(new LocationEntityService<City>(new RepositoryInMemory<City>()), true);
        }


        /// <summary>
        /// Loads all the model definitions from both dlls and from models.ini.config file overrides.
        /// </summary>
        /// <returns></returns>
        private IList<ModelSettings> GetModelDefs()
        {
            var allmodels = Cacher.Get<IList<ModelSettings>>("ModelDefinitions", 300, () =>
            {
                string assemblyNames = System.Configuration.ConfigurationManager.AppSettings["assembliesForExtensions"];
                var models = ModelHelper.LoadAll(HttpContext.Current.Server.MapPath("~/config/data/Models.ini.config"), assemblyNames);
                return models;
            });
            return allmodels;
        }


        public IDictionary GetModelSettingsAsDictionary()
        {
            var settings = Cacher.Get<IDictionary>("model.settings", true, 300, false, () =>
            {
                var doc = new IniDocument(HttpContext.Current.Server.MapPath("~/config/data/models.ini.config"), true);
                return doc as IDictionary;
            });
            return settings;
        }



        /// <summary>
        /// Builds the entity security helper.
        /// </summary>
        /// <returns></returns>
        public EntitySettingsHelper GetModelSettingsAsHelper()
        {
            var helper = Cacher.Get<EntitySettingsHelper>("EntitySettingsHelper", 500, () =>
            {
                var models = GetModelDefs();
                var settings = new EntitySettingsHelper();
                var settingsMap = new Dictionary<Type, ModelSettings>();
                foreach (var model in models)
                    settingsMap[model.Model] = model;

                settings.Init(settingsMap);
                return settings;
            });
            return helper;
        }


        private void Test()
        {
            // PERFORMANCE ARTICLES:
            // 1. http://weblogs.asp.net/rashid/archive/2009/04/23/asp-net-mvc-view-location-and-performance-issue.aspx

            // SECURITY
            // 1. http://www.mikesdotnetting.com/Article/126/ASP.NET-MVC-Prevent-Image-Leeching-with-a-Custom-RouteHandler
            // 2. Do not allow form updatemodel to update id, createuser, updateuser, updatedate, createdate, version, versionrefid.

            // codecapers.com
            // http://blog.sb2.fr/post/2008/12/19/Reading-and-Writing-RSS-Feeds-Using-SystemServiceModelSyndication.aspx
            // http://www.learningjquery.com/2006/09/basic-show-and-hide
            // http://www.styleshout.com 
            // http://www.billsternberger.net/

            // Good tutorial on EditorTemplates
            // http://bradwilson.typepad.com/blog/2009/10/aspnet-mvc-2-templates-part-4-custom-object-templates.html

            // Dialog display.
            // http://towardsnext.wordpress.com/2009/04/17/file-upload-in-aspnet-mvc/
        }
    }
}
