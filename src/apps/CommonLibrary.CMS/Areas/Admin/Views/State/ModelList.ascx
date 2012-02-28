<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.LocationSupport.State>>" %>
<%@ Import Namespace="ComLib.LocationSupport" %>

<%= Html.ActionLink("Add", "Create", "State", null, new { @class = "actionbutton" })%><br /><br />


   <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>Name</th>
            <th>Active</th>
            <th>Alias</th>
            <th>Country</th>
        </tr>
    <% foreach (var item in Model.Items) { %>    
        <tr><% if (Model.ShowEditDelete)
               {%>
                <td>
                    <% Html.RenderPartial("~/views/shared/Controls/EntityManage.ascx", new EntityListManageViewModel() { Id = item.Id, ViewInfo = Model }); %>
                </td>
            <% } %>
            <td>
                <%= Html.Encode(item.Name) %>
            </td>
            <td>
                <%= Html.Encode(item.IsActive)%>
            </td>
            <td>
                <%= Html.Encode(item.IsAlias)%>
            </td>
            <td>
                <%= Html.Encode(item.CountryName) %>
            </td>
        </tr>
    <% } %>
    </table>

