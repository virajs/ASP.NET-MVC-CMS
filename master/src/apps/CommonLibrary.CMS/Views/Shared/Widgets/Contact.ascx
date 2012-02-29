<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.CMS.Models.Widgets.Contact>" %>
<%@ Import Namespace="ComLib.Web.Modules.Links" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%@ Import Namespace="ComLib.Data" %>


<a href="<%= Model.Url %>"><span class="wtitle"><%= Model.ContactHeader %></span></a><br />
<%= Model.ContactName %><br />
<%= Model.Email %><br />
<%= Model.Phone %><br />

<% if (Model.IsLocationApplicable)
   { %>
    <br />
    <%= Model.Street%><br />
    <%= Model.City%>, <%= Model.Zip%><br />
    <%= Model.State%><br />
    <%= Model.Country%><br />    
<% } %>

