<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.MenuEntrys.MenuEntry>>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

    <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>
                Name
            </th>
            <th>
                Desc
            </th>
            <th>
                Roles
            </th>
            <th>
                Public
            </th>
            <th>
                Rerouted
            </th>
            <th>
                Sort#
            </th>
            <th>
                Url
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
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.Description) %>
            </td>
            <td>
                <%= Html.Encode(item.Roles) %>
            </td>
            <td>
                <%= Html.Encode(item.IsPublic) %>
            </td>
            <td>
                <%= Html.Encode(item.IsRerouted) %>
            </td>
            
            <td>
                <%= Html.Encode(item.SortIndex) %>
            </td>     
            <td>
                <%= Html.Encode(item.Url) %>
            </td>     
        </tr>    
    <% } %>
    </table>
