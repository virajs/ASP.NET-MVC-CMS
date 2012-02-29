<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Events.Event>" %>
<%@ Import Namespace="ComLib.Web.Modules.Events" %>   
<%@ Import Namespace="ComLib.Web.Modules.Media" %>   
<%@ Import Namespace="ComLib.Web.Lib" %>   

<% if( !Event.Settings.EnableFreeformLocations) { %>       
<% Html.RenderPartial("controls/Location", new LocationViewModel()
        {
            JSObjectName = "eventlocation",
            CountriesDropDownId = "Address_Country",
            StatesDropDownId = "Address_State",
            CitiesDropDownId = "Address_City",
            CountriesErrorDivId = "countries_result",
            StatesErrorDivId = "states_result",
            CitiesErrorDivId = "cities_result",
            SelectedCountry = Model.Address == null ? "United States" : Model.Address.Country,
            SelectedState = Model.Address == null ? "New York" : Model.Address.State,
            SelectedCity = Model.Address == null ? "New York" : Model.Address.City
        }); %> 
<% } %>
<table>
    <tr>
        <td colspan="2">
            <%= Html.ResourceFor(model => model.Title) %>
            <%= Html.TextBoxFor(model => model.Title, new { @class = "wide" })%><br />
            <%= Html.ValidationMessageFor(model => model.Title) %>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <%= Html.ResourceFor(model => model.Category) %>
            <%= ComLib.Web.Modules.Categorys.CategoryHelper.BuildCategoriesFor<Event>("CategoryId", "&nbsp;&nbsp;&nbsp;&nbsp;")%>
            <%= Html.ValidationMessageFor(model => model.CategoryId)%><br />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <%= Html.ResourceFor(model => model.Description) %>
            <%= Html.TextBoxFor(model => model.Description, new { @class = "wide" })%><br />
            <%= Html.ValidationMessageFor(model => model.Description)%>
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <%= Html.ResourceFor(model => model.Content) %>
            <%= Html.TextAreaFor(model => model.Content, new { @class = "text-area" })%><br />
            <%= Html.ValidationMessageFor(model => model.Content)%><br /><br />
        </td>
    </tr>
    <tr>
        <td colspan="2"><h3>Date / Time</h3></td>         
    </tr>
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.StartDate) %>
            <%= Html.CalendarTextBox("StartDate", Model.StartDate.ToShortDateString())%>
            <br /><span class="example">e.g. dd/mm/yyyy - <%= DateTime.Today.ToShortDateString() %></span><br />
            <%= Html.ValidationMessageFor(model => model.StartDate) %><br /><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Starts) %>
            <%= Html.TextBoxFor(model => model.Starts) %>
            <br /><span class="example">e.g. 9am | 11:30 am | 1:30pm </span><br />
            <%= Html.ValidationMessageFor(model => model.Starts) %><br /><br />
        </td>
    </tr>
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.EndDate) %>
            <%= Html.CalendarTextBox("EndDate", Model.EndDate.ToShortDateString())%>
            <br /><span class="example">e.g. dd/mm/yyyy - <%= DateTime.Today.AddDays(10).ToShortDateString() %></span><br />
            <%= Html.ValidationMessageFor(model => model.EndDate) %><br /><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Ends) %>
            <%= Html.TextBoxFor(model => model.Ends) %>
            <br /><span class="example">e.g. 9am | 11:30 am | 1:30pm </span><br />
            <%= Html.ValidationMessageFor(model => model.Ends) %><br /><br />
        </td>           
    </tr>
    <tr>
        <td><%= Html.ResourceFor("Is Online") %>
            <%= Html.CheckBoxFor(model => model.IsOnline, new { onclick = "OnIsOnlineChanged();" })%><br />
        </td>
        <td><%= Html.ResourceFor("Is All Times") %>
            <%= Html.CheckBoxFor(model => model.IsAllTimes, new { onclick = "OnIsAllTimesChanged();" })%><br /><br />
        </td>
    </tr>
    <tr>
        <td colspan="2"><h3>Location</h3></td>
    </tr>
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.Address.Venue) %>
            <%= Html.TextBoxFor(model => model.Address.Venue)%>
            <br /><span class="example">e.g. Javits Convention Center</span><br />
            <%= Html.ValidationMessageFor(model => model.Address.Venue)%><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Address.Street) %>
            <%= Html.TextBoxFor(model => model.Address.Street)%>
            <br /><span class="example">e.g. 100 Broadway</span><br />
            <%= Html.ValidationMessageFor(model => model.Address.Street)%><br />
        </td>                        
    </tr>
    <%  // Use drop-downs for country/state or allow user to put in their own?
        if(Event.Settings.EnableFreeformLocations){ %>
    <tr>
        <td><%= Html.ResourceFor(model => model.Address.Country) %>
            <%= Html.TextBoxFor(model => model.Address.Country)%>
            <br /><span class="example">e.g. USA</span>
            <%= Html.ValidationMessageFor(model => model.Address.Country)%>
        </td>       
        <td><%= Html.ResourceFor(model => model.Address.State) %>            
            <%= Html.TextBoxFor(model => model.Address.State)%>
            <br /><span class="example">e.g. New York</span><br />
            <%= Html.ValidationMessageFor(model => model.Address.State)%>
        </td>                  
    </tr>
    <tr>
         <td><%= Html.ResourceFor(model => model.Address.City) %>
            <%= Html.TextBoxFor(model => model.Address.City)%>
            <br /><span class="example">e.g. Queens</span>
            <%= Html.ValidationMessageFor(model => model.Address.City)%>
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Address.Zip) %>
            <%= Html.TextBoxFor(model => model.Address.Zip)%>
            <br /><span class="example">e.g. 10017</span>
            <%= Html.ValidationMessageFor(model => model.Address.Zip)%><br /><br />
        </td>       
    </tr>
    <% }
       else { %>
    <tr>
        <td><%= Html.ResourceFor(model => model.Address.Country) %>
            <%= Html.DropDownList("Address.Country", Html.ToDropDownList("", new string[] { "" }), new { @onchange = "eventlocation.LoadStates();" })%>
            <br /><span class="example">e.g. USA</span><br />
            <%= Html.ValidationMessageFor(model => model.Address.Country)%>
        </td>       
        <td><%= Html.ResourceFor(model => model.Address.State) %>            
            <%= Html.DropDownList("Address.State", Html.ToDropDownList("", new string[] { "" }), new { @onchange = "eventlocation.LoadCities();" })%>
            <br /><span class="example">e.g. New York</span><br />
            <%= Html.ValidationMessageFor(model => model.Address.State)%><br />
        </td>                  
    </tr>   
    <tr>
         <td>
            <%= Html.ResourceFor(model => model.Address.City) %>
            <%= Html.DropDownList("Address.City", Html.ToDropDownList("", new string[] { "" }))%>
            <br /><span class="example">e.g. Queens</span>
            <%= Html.ValidationMessageFor(model => model.Address.City)%>
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Address.Zip) %>
            <%= Html.TextBoxFor(model => model.Address.Zip)%>
            <br /><span class="example">e.g. 10017</span>
            <%= Html.ValidationMessageFor(model => model.Address.Zip)%><br /><br />
        </td>       
    </tr>
    <% } %>   
    <tr>
        <td colspan="2"><h3>Details</h3></td>
    </tr>
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.Email) %>
            <%= Html.TextBoxFor(model => model.Email) %><br />
            <%= Html.ValidationMessageFor(model => model.Email) %><br /><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Url) %>
            <%= Html.TextBoxFor(model => model.Url) %>
            <br /><span class="example">e.g. http://www.google.com</span>
            <%= Html.ValidationMessageFor(model => model.Url) %><br /><br />
        </td>
    </tr>
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.Costs) %>
            <%= Html.TextBoxFor(model => model.Costs)%><br />
            <span class="example">free | $25 | unknown | varies</span>
            <%= Html.ValidationMessageFor(model => model.Costs)%><br /><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.SkillText, "Skill") %>
            <%= Html.DropDownList("SkillText", Html.ToDropDownList(Model.SkillText, Event.SelectionsForSkill)) %>
            <br /><span class="example">all | beginner | intermediate | advanced</span>
            <%= Html.ValidationMessageFor(model => model.SkillText)%><br /><br />
        </td>
    </tr>  
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.Phone) %>
            <%= Html.TextBoxFor(model => model.Phone) %><br />
            <span class="example">e.g. 123-456-7890</span>
            <%= Html.ValidationMessageFor(model => model.Phone) %><br /><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Tags) %>
            <%= Html.TextBoxFor(model => model.Tags)%><br />
            <span class="example">multiple words separated by space <br />e.g. ( business stocks invest )</span>
            <%= Html.ValidationMessageFor(model => model.Tags)%><br /><br />
        </td>
    </tr>  
    <tr>
        <td>
            <%= Html.ResourceFor(model => model.Ages) %>
            <%= Html.TextBoxFor(model => model.Ages)%><br />
            <span class="example">e.g. 21 and over | 10 and under<br /> | all | 25 - 35</span>
            <%= Html.ValidationMessageFor(model => model.Ages)%><br /><br />
        </td>
        <td>
            <%= Html.ResourceFor(model => model.Seating) %>
            <%= Html.TextBoxFor(model => model.Seating)%><br />
            <span class="example">e.g. 25 | N/A | unlimited</span>
            <%= Html.ValidationMessageFor(model => model.Seating)%><br /><br />
        </td>
    </tr>                   
    <% if (Model.IsPersistant())
       { %>  
        <tr><td colspan="2"><% Html.RenderMediaUpload(Model, 1, Model.IsPersistant(), true); %><br /><br /></td></tr>
    <% } %>
</table> 
<script type="text/javascript">
    // Set up the location fields.
    $(document).ready(function () {
        OnIsOnlineChanged();
        OnIsAllTimesChanged();
        $("#CategoryId").val('<%= Model.CategoryId %>');
    });


    function OnIsAllTimesChanged() {
        if ($('#IsAllTimes').is(':checked')) {
            $('#Starts').attr('disabled', 'disabled');
            $('#Ends').attr('disabled', 'disabled');
            $('#Starts').val('N/A');
            $('#Ends').val('N/A');
        }
        else {
            $('#Starts').removeAttr('disabled');
            $('#Ends').removeAttr('disabled');
        }
    }

    function OnIsOnlineChanged() {
        if ($('#IsOnline').is(':checked')) {
            $('#Address_Venue').attr('disabled', 'disabled');
            $('#Address_Country').attr('disabled', 'disabled');
            $('#Address_State').attr('disabled', 'disabled');
            $('#Address_City').attr('disabled', 'disabled');
            $('#Address_Zip').attr('disabled', 'disabled');
            $('#Address_Street').attr('disabled', 'disabled');
        }
        else {
            $('#Address_Venue').removeAttr('disabled');
            $('#Address_Country').removeAttr('disabled');
            $('#Address_State').removeAttr('disabled');
            $('#Address_City').removeAttr('disabled');
            $('#Address_Zip').removeAttr('disabled');
            $('#Address_Street').removeAttr('disabled');
        }
    }
</script>
