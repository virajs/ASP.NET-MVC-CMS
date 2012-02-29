using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComLib;
using ComLib.Caching;
using ComLib.Entities;
using ComLib.LocationSupport;
using ComLib.Web.Lib.Core;


namespace ComLib.CMS.Controllers
{
    /// <summary>
    /// Location controller for getting cities, states, countries.
    /// </summary>
    public class LocationController : CommonController
    {
        /// <summary>
        /// Get all the countries in JSON format.
        /// </summary>
        /// <returns></returns>
        public ActionResult Countries()
        {
            var countries = Cacher.Get<IList<Country>>("countries_list", 300, () => Location.Countries.GetAll().Where(c => !c.IsAlias && c.IsActive).ToList());
            string[] countrynames = countries.Select<Country, string>(c => c.Name).ToArray();
            return Json(new { Success = true, Message = string.Empty, Data = countrynames }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <returns></returns>
        public ActionResult States(string countryname)
        {
            // The Location component can get the states for a specific country.
            BoolMessageItem<IList<State>> statesResult = Location.StatesFor(countryname);
            if (!statesResult.Success)
                return Json(new { Success = false, Message = statesResult.Message, Data = string.Empty }, JsonRequestBehavior.AllowGet);

            string[] statenames = statesResult.Item.Select<State, string>(s => s.Name).ToArray();
            return Json(new { Success = true, Message = string.Empty, Data = statenames }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get the states for the specified country
        /// </summary>
        /// <param name="countryname"></param>
        /// <returns></returns>
        public ActionResult Cities(string countryname, string statename)
        {
            BoolMessageItem<IList<City>> citiesResult = Location.CitiesFor(countryname, statename);
            if (!citiesResult.Success)
                return Json(new { Success = false, Message = citiesResult.Message, Data = string.Empty }, JsonRequestBehavior.AllowGet);

            string[] cities = citiesResult.Item.Select<City, string>(c => c.Name).ToArray();
            return Json(new { Success = true, Message = string.Empty, Data = cities }, JsonRequestBehavior.AllowGet);
        }
    }
}
