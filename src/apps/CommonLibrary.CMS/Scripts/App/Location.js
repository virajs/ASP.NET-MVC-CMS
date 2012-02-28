/* *********************************************************************
* Author: Kishore Reddy
* Application: CommonLibrary.NET CMS
* Url: http://commonlibrarynetcms.codeplex.com
* Description: Javascript class for an AJAX based city,state,country component.
********************************************************************** */

///<summary>Initializes a new instance of the javascript Search class.
///</summary>
///<param name="countriesDropdownId">The id of the countries downdown list.</param>
///<param name="statesDropdownId">The id of the states dropdown list.</param>
///<param name="selectedCountry">The country to load in the ui.</param>
///<param name="selectedState">The state to load in the ui.</param>
///<param name="onLoadCompleteCallback">The callback function to call when the data is loaded.</param>
function LocationControl(countriesDropdownId, statesDropdownId, citiesDropdownId, selectedCountry, selectedState, selectedCity, countriesDivErrorId, statesDivErrorId, citiesDivErrorId, onLoadCompleteCallback) {
    // Data for page and for calculations.
    this.countriesDropDownId = countriesDropdownId;
    this.statesDropDownId = statesDropdownId;
    this.citiesDropDownId = citiesDropdownId;
    this.selectedCountry = selectedCountry;
    this.selectedState = selectedState;
    this.selectedCity = selectedCity;
    this.countriesDivErrorId = countriesDivErrorId;
    this.statesDivErrorId = statesDivErrorId;
    this.citiesDivErrorId = citiesDivErrorId;
    this.onLoadComplete = onLoadCompleteCallback;
}


///<summary> Performs a websearch in the background via ajax, gets back json data
/// representing the search results and then displays them on the page.
/// This supports paging of the results. However, unlike using a regular pager, the page
/// number must be entered in the text box. This is the current implementation for the time being.
/// This uses 3 variables ( keywords, searchtype, pagenumber ).
///</summary>
LocationControl.prototype.DoLoad = function () {
    var url = "/location/countries";
    var optionsId = this.countriesDropDownId;
    var location = this;

    $.getJSON(url, function (result) {
        var msg = "";
        $('#' + location.countriesDivErrorId).html("");
        if (result.Success) {
            var optionscontrol = document.getElementById(optionsId);
            optionscontrol.options.length = 0;

            // Now populate the new options.
            for (i = 0; i < result.Data.length; i++) {
                var optionval = result.Data[i];
                optionscontrol.options.add(new Option(optionval, optionval));
            }

            if (location.selectedCountry && location.selectedCountry != "") {
                $("#" + location.countriesDropDownId).val(location.selectedCountry);
            }
            location.LoadStates();
        }
        else {
            msg = '<span class="error">' + result.Message + "</span>";
            $('#' + location.countriesDivErrorId).html(msg);
        }
    });
}


///<summary>Loads a dropdown using json data from the url supplied.</summary>
///<param name="dependentOptionsId">The id of the options/drop-down to use</param>
///<param name="url">The url for the ajax fetch</param>
///<param name="optionsId">The id of the options/dropdown.</param>
///<param name="divIdForError">The id of the div to display an error if load fails.</param>
LocationControl.prototype.LoadStates = function () {
    var country = $('#' + this.countriesDropDownId + '>option:selected').text();
    var url = "/location/states?countryname=" + encodeURI(country);
    var optionsId = this.statesDropDownId;
    var divIdError = this.statesDivErrorId;
    var selectedVal = this.selectedState;
    var location = this;
    $.getJSON(url, function (result) {
        var msg = "";
        $('#' + divIdError).html("");
        if (result.Success) {
            var optionscontrol = document.getElementById(optionsId);

            optionscontrol.options.length = 0;

            // Now populate the new options.
            for (i = 0; i < result.Data.length; i++) {
                var optionval = result.Data[i];
                optionscontrol.options.add(new Option(optionval, optionval));
            }
            if (selectedVal && selectedVal != "") {
                $("#" + optionsId).val(selectedVal);
            }
            location.LoadCities();
        }
        else {
            msg = '<span class="error">' + result.Message + "</span>";
            $('#' + divIdForError).html(msg);
        }
    });
}


///<summary>Loads a dropdown using json data from the url supplied.</summary>
///<param name="dependentOptionsId">The id of the options/drop-down to use</param>
///<param name="url">The url for the ajax fetch</param>
///<param name="optionsId">The id of the options/dropdown.</param>
///<param name="divIdForError">The id of the div to display an error if load fails.</param>
LocationControl.prototype.LoadCities = function () {
    var country = $('#' + this.countriesDropDownId + '>option:selected').text();
    var state = $('#' + this.statesDropDownId + '>option:selected').text();
    var url = "/location/cities?countryname=" + encodeURI(country) + "&statename=" + encodeURI(state);
    $('#' + this.citiesDivErrorId).html("");
    LoadDropDown(url, this.citiesDropDownId, this.citiesDivErrorId, this.selectedCity);
}
