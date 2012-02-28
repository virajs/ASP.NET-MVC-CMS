using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Caching;
using ComLib.Entities;
using ComLib.Extensions;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.MenuEntrys;


namespace ComLib.Web.Modules.Pages
{
    /// <summary>
    /// Settings specific for blog posts.
    /// </summary>
    public class PageSettings : EntitySettings<Page>
    {
        /// <summary>
        /// Set default values.
        /// </summary>
        public PageSettings()
        {
            CacheTime = 300;
            CacheOn = true;
        }


        public bool CacheOn { get; set; }
        public int CacheTime { get; set; }
    }



    [Model(Id = 1, Description = "Html Pages", IsExportable = true, IsImportable = true, FormatsForExport = "xml,ini", FormatsForImport = "xml,ini",
        RolesForCreate = "${Admin}", RolesForView = "?", RolesForIndex = "?", RolesForManage = "${Admin}", RolesForDelete = "${Admin}",
        RolesForImport = "${Admin}", RolesForExport = "${Admin}")]
    public partial class Page : ActiveRecordBaseEntity<Page>, IEntity
    {
        /// <summary>
        /// Key used for caching the menuentry lookup.
        /// </summary>
        public const string CacheKeyForLookup = "htmlpages_lookupid";


        protected static PageSettings _settings = new PageSettings();
 

        public string ActiveUrl
        {
            get
            {
                string url = string.Empty;

                if (string.IsNullOrEmpty(Slug))
                    url = ComLib.Web.UrlSeoUtils.BuildValidUrl(Title);
                else
                    url = ComLib.Web.UrlSeoUtils.BuildValidUrl(Slug);
                return url;
            }
        }


        /// <summary>
        /// Absolute Url.
        /// </summary>
        public string UrlAbsolute
        {
            get { return "/" + ActiveUrl; }
        }


        /// <summary>
        /// Settings for this entity.
        /// </summary>
        public static new PageSettings Settings
        {
            get { return _settings; }
        }


        /// <summary>
        /// Get all the pages that are "FrontPages" and should appear on the home page
        /// in the menu / footer.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Page> FrontPages()
        {
            var frontpages = Cacher.Get<IEnumerable<Page>>("htmlpages_all", 300, () => GetAll().Where<Page>(page => page.IsFrontPage));
            return frontpages;
        }


        /// <summary>
        /// Get all the pages that are "FrontPages" and should appear on the home page
        /// in the menu / footer.
        /// </summary>
        /// <returns></returns>
        public static IList<MenuEntry> FrontPagesAsMenuEntrys()
        {
            var frontpages = Cacher.Get<IEnumerable<Page>>("htmlpages_all", 300, () => GetAll().Where<Page>(page => page.IsFrontPage));
            IList<MenuEntry> menuitems = new List<MenuEntry>();
            foreach (Page page in frontpages)
                menuitems.Add(new MenuEntry() { Name = page.Title, Roles = string.Empty, SortIndex = 20, RefId = page.Id, Url = page.ActiveUrl });

            return menuitems;
        }


        /// <summary>
        /// Lookup of pages by id only.
        /// </summary>
        public static LookupMulti<Page> Lookup
        {
            get
            {
                // Cached lookup of page items by id only.
                var pagelookup = Cacher.Get<LookupMulti<Page>>(CacheKeyForLookup,
                        Web.Modules.Pages.Page.Settings.CacheOn, Web.Modules.Pages.Page.Settings.CacheTime.Seconds(),
                        () => ComLib.Web.Modules.Pages.Page.Repository.ToLookUpMulti<string>(m => m.Slug));
                return pagelookup;
            }
        }
    }
}
