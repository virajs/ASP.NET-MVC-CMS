<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Categorys.Category>>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

   <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>Name</th>
            <th>Group</th>
            <th>ParentId</th>
            <th>Sort#</th>
        </tr>
    <% foreach (var item in Model.Items) { %>    
        <tr><% if (Model.ShowEditDelete)
               {%>
                <td>
                    <% Html.RenderPartial("Controls/EntityManage", new EntityListManageViewModel() { Id = item.Id, ViewInfo = Model }); %>
                </td>
            <% } %>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.Group) %>
            </td>
            <td>
                <%= Html.Encode(item.ParentId) %>
            </td>
            <td>
                <%= Html.Encode(item.SortIndex)%>
            </td>
        </tr>
    <% } %>
    </table>

