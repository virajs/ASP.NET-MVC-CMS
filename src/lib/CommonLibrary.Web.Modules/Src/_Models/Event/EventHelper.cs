using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ComLib.LocationSupport;
using ComLib.Extensions;
using ComLib.Data;
using ComLib.Account;
using ComLib.Authentication;
using ComLib.Web.Templating;
using ComLib.Web.Modules.Profiles;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Extensions;

namespace ComLib.Web.Modules.Events
{
    /// <summary>
    /// Helper class for events.
    /// </summary>
    public class EventHelper
    {
        /// <summary>
        /// Applies cateog
        /// </summary>
        /// <param name="events"></param>
        public static void ApplyCategories(IList<Event> events)
        {
            if (events == null || events.Count == 0) return;

            var lookup = Categorys.Category.LookupFor<Event>();
            foreach (var ev in events)
                ev.Category = lookup[ev.CategoryId];
        }



        /// <summary>
        /// Build a key / value pair representing the field and value for location searches.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static KeyValuePair<string, object> BuildCondition(LocationLookUpResult result)
        {
            if (result.IsLookUpByZip)
                return new KeyValuePair<string, object>("Zip", result.Zip);

            if (result.IsLookUpByCity)
                return new KeyValuePair<string, object>("CityId", result.CityId);

            if (result.IsLookUpByState)
                return new KeyValuePair<string, object>("StateId", result.StateId);

            return new KeyValuePair<string, object>("CountryId", result.CountryId);
        }


        public static string BuildContent(Tag tag)
        {
            var settings = CMS.CMS.EntitySettings;
            var helper = new EntityHelper<Event>(settings);
            var result = helper.Details(tag.Id);
            var model = EntityViewHelper.BuildViewModelForDetails(result, "Event", "details", settings);

            return "";

        }
    }
}
