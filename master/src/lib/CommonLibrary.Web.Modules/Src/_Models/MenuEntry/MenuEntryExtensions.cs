using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Caching;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Modules.Pages;
using ComLib.ValidationSupport;


namespace ComLib.Web.Modules.MenuEntrys
{
    /// <summary>
    /// Settings specific for blog posts.
    /// </summary>
    public class MenuEntrySettings : EntitySettings<MenuEntry>
    {
        /// <summary>
        /// Set default values.
        /// </summary>
        public MenuEntrySettings()
        {
            CacheTime = 300;
            CacheOn = true;
        }


        public bool CacheOn { get; set; }
        public int CacheTime { get; set; }
    }



    public partial class MenuEntry : ActiveRecordBaseEntity<MenuEntry>, IEntity
    {
        /// <summary>
        /// Key used for caching the menuentry lookup.
        /// </summary>
        public const string CacheKeyForLookup = "menu_lookupbyname";
        public const string CacheKeyForFrontPages = "menu_Frontpages";


        /// <summary>
        /// Singleton settings.
        /// </summary>
        private static MenuEntrySettings _settings = new MenuEntrySettings();


        #region Public properties
        /// <summary>
        /// Whether or not this is a public menu item
        /// </summary>
        public bool IsPublic
        {
            get { return string.IsNullOrEmpty(Roles); }
        }


        /// <summary>
        /// Absolute Url.
        /// </summary>
        public string UrlAbsolute
        {
            get 
            {
                if (Url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    return Url;

                return "/" + Url; 
            }
        }
        #endregion


        /// <summary>
        /// Get validator for this.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                MenuEntry entity = (MenuEntry)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 50, results, "Name");
                Validation.IsStringLengthMatch(entity.Url, true, true, true, 1, 150, results, "Url");
                Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 50, results, "Description");
                Validation.IsStringLengthMatch(entity.Roles, true, false, true, -1, 50, results, "Roles");
                Validation.IsStringLengthMatch(entity.ParentItem, true, false, true, -1, 50, results, "ParentItem");
                Validation.IsNumericWithinRange(entity.RefId, false, false, -1, -1, results, "RefId");
                Validation.IsNumericWithinRange(entity.SortIndex, false, false, -1, -1, results, "SortIndex");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Life-cycle event callback.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            // Make the Parent emtpy string to make the searches easier.
            if (ParentItem == null)
                ParentItem = string.Empty;

            return base.OnBeforeSave(ctx);
        }
        

        /// <summary>
        /// Get the pages to show in the front/menu header.
        /// </summary>
        /// <returns></returns>
        public static IList<MenuEntry> FrontPages()
        {
            // Next version will have Query<T> so it's more convenient than
            // using <T> on each Where<T> And<T> OrderBy<T>            
            var items = Cacher.Get<IList<MenuEntry>>(CacheKeyForFrontPages, Settings.CacheOn, Settings.CacheTime.Seconds(), () =>
                Find(Query<MenuEntry>.New().Where(m => m.ParentItem).Is(string.Empty)
                                              .And(m => m.IsPublic).Is(true)
                                              .OrderBy(m => m.SortIndex)));
            return items;
        }


        /// <summary>
        /// Settings for this entity.
        /// </summary>
        public static new MenuEntrySettings Settings
        {
            get { return _settings; }
        }


        /// <summary>
        /// Create the menu entry from the page.
        /// </summary>
        /// <param name="page"></param>
        public static MenuEntry CreateFromPage(Page page)
        {
            string url = page.ActiveUrl;
            var menuItem = new MenuEntry() { Name = page.Title, Roles = string.Empty, SortIndex = 1, RefId = page.Id, Url = url };
            MenuEntry.Create(menuItem);
            return menuItem;
        }


        /// <summary>
        /// Create the menu entry from the page.
        /// </summary>
        /// <param name="page"></param>
        public static void UpdateFromPage(bool isPageInMenu, int pageId)
        {
            Page page = Page.Get(pageId);
            var items = MenuEntry.Find(Query<MenuEntry>.New().Where(m => m.RefId).Is(pageId));
            MenuEntry entry = items == null || items.Count == 0 ? null : items[0];            

            // Removing from menu.
            if (!isPageInMenu && entry != null)
            {
                MenuEntry.Delete(entry.Id);
            }
            // Creating menu
            else if (isPageInMenu && entry == null)
            {
                MenuEntry.CreateFromPage(page);
            }
            // updating menu.
            else if (isPageInMenu && entry != null)
            {
                entry.Name = page.Title;
                entry.RefId = page.Id;
                entry.Url = page.ActiveUrl;
                MenuEntry.Update(entry);
            }
        }


        /// <summary>
        /// Get the page from the menu item name.
        /// </summary>
        /// <param name="pagename"></param>
        /// <returns></returns>
        public static Page GetPage(string pagename)
        {
            if (!MenuEntry.Lookup.ContainsKey(pagename)) 
            {
                if (Page.Lookup.ContainsKey(pagename))
                    return Page.Lookup[pagename];
                return null;
            }
            MenuEntry entry = MenuEntry.Lookup[pagename];
            if (!Page.Lookup.ContainsKey(entry.RefId)) return null;

            Page refpage = Page.Lookup[entry.RefId];
            return refpage;
        }


        /// <summary>
        /// Lookup by both id and string.
        /// </summary>
        public static LookupMulti<MenuEntry> Lookup
        {
            get
            {
                // Cached lookup of menu items by id and name.
                var menulookup = Cacher.Get<LookupMulti<MenuEntry>>(CacheKeyForLookup,
                        MenuEntry.Settings.CacheOn, MenuEntry.Settings.CacheTime.Seconds(),
                        () => MenuEntry.Repository.ToLookUpMulti<string>(m => m.Url));
                return menulookup;
            }
        }
    }
}
