<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewPage<EntityListViewModel<ComLib.Web.Modules.Profiles.Profile>>" %>
<%@ Import Namespace="ComLib" %>

   <table>
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>
                Image
            </th>
            <th>
                Name
            </th>
            <th>
                About
            </th>
        </tr>

    <% foreach (var item in Model.Items) { %>
    
        <tr>
            <% if (Model.ShowEditDelete)
               {%>
                <td>
                    <%= Html.ActionLink("Edit", "Edit", new { id = item.Id })%> |
                    <%= Html.ActionLink("Delete", "Delete", new { id = item.Id })%>
                </td>
            <% } %>        
            <td>
                <img src="<%= item.ImageUrl %>" alt="<%= item.UserName %>" height="40px" width="40px" />
            </td>
            <td>
                <%= Html.Encode(item.UserName) %>
            </td>
            <td>
                <%= Html.Encode(item.About) %><br /><br />
                <a href="<%= Html.Encode(item.WebSite) %>"><%= Html.Encode(item.WebSite) %></a>
            </td>     
        </tr>    
    <% } %>
    </table>

