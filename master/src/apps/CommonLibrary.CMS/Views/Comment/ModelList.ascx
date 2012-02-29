<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Comments.Comment>>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

   <table class="systemlist">
        <tr>            
            <th>User</th>
            <th>Content</th>
        </tr>

    <% foreach (var item in Model.Items) { %>
    
        <tr>
            <td class="up">
                <% if (!string.IsNullOrEmpty(item.Url))
                   { %>
                <a href="<%= item.Url %>"><%= Html.Encode(item.Name)%></a><br />
                <%}
                   else
                   { %>
                <%= Html.Encode(item.Name)%><br />
                <%} %>
                <% if (item.IsGravatarEnabled)
                   { %>
                      <img src="<%= item.ImageUrl %>" alt="user" /><br />
                <%} %>
                <% if (Model.ShowEditDelete)
                   {%>
                      <% Html.RenderPartial("Controls/EntityManage", new EntityListManageViewModel() { Id = item.Id, ViewInfo = Model, ShowCopy = false, ShowEdit = false }); %>                
                <% } %>                
            </td>
            <td class="up">
                <%= Html.Encode(item.Title) %><br /><br />
                <%= Html.Encode(item.Content) %><br /><br />
                <span class="bold"><%= Html.Encode(item.Email)%></span><br />
                <% if (!string.IsNullOrEmpty(item.Url)) { %><span class="bold"><%= Html.Encode(item.Url)%></span><br /><br /><%}%>                
            </td>
        </tr>
    
    <% } %>
    </table>

