using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// The search widget does not have specific properties so far. This is because all it does is 
    /// render some html/javascript which takes in the keyword to search, and also whether to search 
    /// the web or just the site.
    /// </summary>
    [Widget(Name = "Search", IsCachable = true, IsEditable = true, SortIndex = 5, Path="Widgets/Search")]
    public class SearchWidget : WidgetInstance
    {
    }
}
