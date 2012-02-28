<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Maps.GeoAddressViewModel>" %>
<%@ Import Namespace="ComLib.Maps" %>
<%@ Import Namespace="ComLib.LocationSupport" %>
 
<%
    // Static google map.
    // http://maps.google.com/maps/api/staticmap?center=Berkeley,CA&zoom=14&size=400x400&sensor=false
    // BING: http://ecn.dev.virtualearth.net/mapcontrol/mapcontrol.ashx?v=6.2
    // GOOGLE: ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA
    // http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA
 
    bool isBing = (string.IsNullOrEmpty(Model.Provider.Name) || string.Compare(Model.Provider.Name, "bing", true) == 0);
    string javascriptMapName = isBing ? "Bing" : "Google";
%>


<%= Html.Javascript(Model.Provider.SourceUrl) %>
<%= Html.Javascript("/scripts/app/Maps." + Model.Provider.Name + ".js")%>
    
    
<script type="text/javascript">

    var mapLocation = "<%= Model.Address.FullAddress %>";
    
    $(document).ready(function() 
    {
        var map1 = new <%=javascriptMapName%>Map("map1");
        map1.Init();
        map1.Locate(mapLocation);
    });
</script>

<div id="map1" style="position:relative; width:<%= Model.Width %>px; height:<%= Model.Height %>px;"></div>
