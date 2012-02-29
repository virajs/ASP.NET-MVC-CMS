<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Parts.Part>>" %>


   <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>
                Title
            </th>
            <th>
                Description
            </th>
            <th>
                AccessRoles
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
                <%= Html.Encode(item.Title) %>
            </td>
            <td>
                <%= Html.Encode(item.Description) %>
            </td>
            <td>
                <%= Html.Encode(item.AccessRoles) %>
            </td>
            <td>
                <%= Html.Encode(item.IsPublic) %>
            </td>     
        </tr>    
    <% } %>
    </table>