using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using ComLib;
using ComLib.Entities;
using ComLib.Caching;
using ComLib.Extensions;
using ComLib.Data;
using ComLib.MapperSupport;
using ComLib.Web.HttpHandlers;
using ComLib.Web.Modules.Pages;


namespace ComLib.Web.Modules.MenuEntrys
{
    /// <summary>
    /// Media file helper class.
    /// </summary>
    public class MenuEntryHelper
    {
        /// <summary>
        /// Clears the cache that stores items used to build the menu.
        /// </summary>
        public static void ClearCacheForMenu()
        {
            Cacher.Remove(MenuEntry.CacheKeyForLookup);
            Cacher.Remove(MenuEntry.CacheKeyForFrontPages);
            Cacher.Remove(Page.CacheKeyForLookup);
        }
    }
}
