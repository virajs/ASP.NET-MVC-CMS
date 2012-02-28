<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Dashboard.Master" Inherits="System.Web.Mvc.ViewPage<IList<ComLib.Caching.CacheItemDescriptor>>" %>
<%@ Import Namespace="ComLib.Caching" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Cache</h2><br />
    
    <%= Html.ActionLink("Refresh", "Index", null, new { @class = "actionlink" })%> 
    <%= Html.ActionLink("Remove All", "RemoveAll", null, new { @class = "actionlink" })%><br /><br />
    <table class="systemlist">
        <tr>
            <th>Action</th>
            <th>Key</th>
            <th>Type</th>
        </tr>
    <% foreach (var item in Model)
       { %>
    
        <tr>
            <td><a href="/Admin/cache/Remove?cacheKey=<%=item.Key%>"><img src="/content/images/generic/item_delete.png" alt="delete" /></a>
            </td>
            <td>
                <%= Html.Encode(item.Key) %>
            </td>
            
            <td>
                <%= Html.Encode(item.ItemType) %>
            </td>
        </tr>
    <%} %>
    </table>
</asp:Content>
