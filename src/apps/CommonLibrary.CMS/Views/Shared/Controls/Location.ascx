<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<LocationViewModel>" %>
<script src="/scripts/app/location.js" type="text/javascript"></script>  
<script type="text/javascript">
    var <%=Model.JSObjectName %> = null;
    /// Loads the comments on page load.
    $(document).ready(function() {
         <%=Model.JSObjectName %> = new LocationControl('<%= Model.CountriesDropDownId %>', '<%= Model.StatesDropDownId %>', '<%= Model.CitiesDropDownId %>', '<%=Model.SelectedCountry %>', '<%=Model.SelectedState %>', '<%=Model.SelectedCity %>', '<%=Model.CountriesErrorDivId %>', '<%=Model.StatesErrorDivId %>', '<%=Model.CitiesErrorDivId %>', <%=Model.LoadCompleteCallback %>);
         <%=Model.JSObjectName %>.DoLoad();
    });
</script>
