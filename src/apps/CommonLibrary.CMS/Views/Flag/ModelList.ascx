<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Flags.Flag>>" %>
<%@ Import Namespace="ComLib.Web.Lib.Models" %>

   <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>Ref Id</th>
            <th>Model</th>
            <th>Title</th>
            <th>Date Flagged</th>
        </tr>
    <% foreach (var item in Model.Items) { %>    
        <tr><% if (Model.ShowEditDelete)
               {%>
                <td>
                    <% Html.RenderPartial("Controls/EntityManage", new EntityListManageViewModel() { Id = item.Id, ViewInfo = Model }); %>
                </td>
            <% } %>
            <td>
                <%= Html.Encode(item.RefId) %>
            </td>
            <td>
                <%= Html.Encode(item.Model) %>
            </td>
            <td>
                <%= Html.Encode(item.Title) %><br />
                <a href="<%= Html.Encode(item.Url) %>"><%= Html.Encode(item.Url.TruncateWithText(50, "..."))%></a>
            </td>
            <td>
                <%= Html.Encode(item.FlaggedDate)%>
            </td>
        </tr>
    <% } %>
    </table>

