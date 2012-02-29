<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Favorites.Favorite>>" %>
<%@ Import Namespace="ComLib" %>

   <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>Type</th>
            <th>Title</th>
            <th>Added On</th>
        </tr>

    <% foreach (var item in Model.Items) { %>
    
        <tr>
            <% if (Model.ShowEditDelete)
               {%>
                <td>
                    <% Html.RenderPartial("Controls/EntityManage", new EntityListManageViewModel() { Id = item.Id, ViewInfo = Model, ShowEdit = false, ShowCopy = false }); %>
                </td>
            <% } %>
            <td>
                <%= Html.Encode(item.Model) %>
            </td>
            <td>
                <a href="<%=item.Url%>" ><%= Html.Encode(StringHelpers.TruncateWithText(item.Title, 50, "...")) %></a>                
            </td> 
            <td><%= item.CreateDate.ToShortDateString() %></td>   
        </tr>    
    <% } %>
    </table>