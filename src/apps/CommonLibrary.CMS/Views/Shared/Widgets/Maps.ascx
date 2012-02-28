<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.CMS.Models.Widgets.GeoMap>" %>
<%@ Import Namespace="ComLib.Maps" %>
<%@ Import Namespace="ComLib.LocationSupport" %>

<%
    bool isBing = (string.Compare(GeoProvider.Provider.Name, "bing", true) == 0);
    string javascriptMapName = isBing ? "Bing" : "Google";
%>
<%= Html.Javascript(GeoProvider.Provider.SourceUrl)%>
<%= Html.Javascript("/scripts/app/Maps." + GeoProvider.Provider.Name + ".js")%>
    
    
<script type="text/javascript">

    var mapLocation = "<%= Model.FullAddress %>";
    
    $(document).ready(function() 
    {
        var <%=Model.Name %> = new <%=javascriptMapName%>Map("<%=Model.Name %>");
        <%=Model.Name %>.Init();
        <%=Model.Name %>.Locate(mapLocation);
    });
</script>

<div id="<%=Model.Name %>" style="position:relative; width:<%= Model.Width %>px; height:<%= Model.Height %>px;"></div>
<%= Model.Content %>
