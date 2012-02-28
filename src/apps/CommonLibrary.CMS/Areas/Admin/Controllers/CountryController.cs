using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;

using ComLib;
using ComLib.Entities;
using ComLib.LocationSupport;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;
using ComLib.Caching;


namespace ComLib.CMS.Areas.Admin
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    [AdminAuthorization]
    public class CountryController : JsonController<Country>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            base.Init();
            DashboardLayout(true);
            InitColumns(
               columnNames: new List<string>() { "Id", "Active", "Alias", "Name" },
               columnProps: new List<Expression<Func<Country, object>>>() { a => a.Id, a => a.IsActive, a => a.IsAlias, a => a.Name },
               columnWidths: null);
        }
    }


    
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    [AdminAuthorization]
    public class StateController : JsonController<State>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            base.Init();
            DashboardLayout(true);
            InitColumns(
               columnNames: new List<string>() { "Id", "Active", "Is Alias", "Name", "Abbr", "Country" },
               columnProps: new List<Expression<Func<State, object>>>() { a => a.Id, a => a.IsActive, a => a.IsAlias,  a => a.Name, a => a.Abbreviation, a => a.CountryName },
               columnWidths: null);
        }
    }



    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    [AdminAuthorization]
    public class CityController : JsonController<City>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {   
            base.Init();
            DashboardLayout(true);
            InitColumns(
               columnNames: new List<string>() { "Id", "Active", "Alias", "Popular", "Name", "State", "Country" },
               columnProps: new List<Expression<Func<City, object>>>() { a => a.Id, a => a.IsActive, a => a.IsAlias, a => a.IsPopular, a => a.Name, a => a.StateName, a => a.CountryName },
               columnWidths: null);
        }
    }
}
