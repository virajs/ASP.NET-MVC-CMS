<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <input type="button" value="Load countries" onclick="cmslocationstart();" />
    <input type="button" value="Load states" onclick="cmslocation.LoadStates();" />
    <input type="button" value="Load cities" onclick="cmslocation.LoadCities();" />
    <% Html.RenderPartial("controls/Location", new LocationViewModel()
        {
            JSObjectName = "cmslocation",
            CountriesDropDownId = "countries",
            StatesDropDownId = "states",
            CitiesDropDownId = "cities",
            CountriesErrorDivId = "countries_result",
            StatesErrorDivId = "states_result",
            CitiesErrorDivId = "cities_result",
            SelectedCountry = "United States",
            SelectedState = "New York",
            SelectedCity = "Albany"
        }); %>

    <div id="countries_area">
        <span id="countries_result"></span>
        <select id="countries"></select>
    </div>
    <div id="states_area">
        <span id="states_result"></span>
        <select id="states"></select>
    </div>
    <div id="cities_area">
        <span id="cities_result"></span>
        <select id="cities"></select>
    </div>
</asp:Content>
