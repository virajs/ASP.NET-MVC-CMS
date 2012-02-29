<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.CMS.Models.Widgets.Links>" %>
<%@ Import Namespace="ComLib.Web.Modules.Links" %>
<%@ Import Namespace="ComLib.Web.Modules.Widgets" %>
<%@ Import Namespace="ComLib.Data" %>
<%
    IQuery<Link> criteria = Query<Link>.New().Where(l => l.Group).Is(DataUtils.Encode(Model.Group)).OrderBy(l => l.SortIndex).Limit(Model.NumberOfEntries);
    var items = Link.Find(criteria);
%>
    
    <ul>
    <% foreach (var item in items) { %>        
        <li><a href="<%= Html.Encode(item.Url) %>" target="_blank"><%= Html.Encode(item.Name) %></a></li>
    <% } %>
    </ul>

