using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Feeds;
using ComLib.Entities;
using ComLib.Web.Modules.Events;


namespace ComLib.Web.Modules.ViewModels
{
    public class RecentViewModel
    {
        public string JSObjectName = "recentItems";
        public string DivId;
        public bool IsVertical = true;
        public bool IsRefreshable = false;
        public int RefreshRate = 15;
        public int PageSize = 10;
        public string TopLevelCssClass = "posts";
        public string Url;        
    }


    public class RecentEventsViewModel
    {
        public PagedList<Event> Items;
    }
}
