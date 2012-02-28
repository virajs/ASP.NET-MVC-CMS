<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Pages.Page>>" %>

    <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>            
            <th>
                Title
            </th>
            <th>
                Slug
            </th>
            <th>
                AccessRoles
            </th>
            <th>
                Parent
            </th>
            <th>
                IsPublic
            </th>
        </tr>

    <% foreach (var item in Model.Items) { %>
    
        <tr>
            <% if (Model.ShowEditDelete)
               {%>
                <td>
                    <% Html.RenderPartial("Controls/EntityManage", new EntityListManageViewModel() { Id = item.Id, ViewInfo = Model }); %>
                </td>
            <% } %>
            <td>
                <%= Html.Encode(item.Title) %><br /> 
            </td>
            <td>
                <%= Html.Encode(item.Slug) %>
            </td>
            <td>
                <%= Html.Encode(item.AccessRoles) %>
            </td>
            <td>
                <%= Html.Encode(item.Parent) %>
            </td>
            <td>
                <%= Html.Encode(item.IsPublic) %>
            </td>     
        </tr>    
    <% } %>
    </table>