using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Account;


namespace ComLib.Web.Lib.Models
{
    
    public class LocationViewModel
    {
        public string JSObjectName = "location";
        public string CountriesDropDownId;
        public string StatesDropDownId;
        public string CitiesDropDownId;

        public string CountriesErrorDivId;
        public string StatesErrorDivId;
        public string CitiesErrorDivId;

        public string SelectedCountry;
        public string SelectedState;
        public string SelectedCity;

        private string _loadCallback;
        public string LoadCompleteCallback
        {
            get { return string.IsNullOrEmpty(_loadCallback) ? "null" : _loadCallback; }
            set { _loadCallback = value; }
        }
    }
}
