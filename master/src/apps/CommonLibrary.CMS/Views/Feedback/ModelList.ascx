<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Feedbacks.Feedback>>" %>
<%@ Import Namespace="ComLib" %>

   <table class="systemlist">
        <tr>
            <% if( Model.ShowEditDelete ){ %><th></th> <% } %>
            <th>
                Title
            </th>
            <th>
                Name/Email
            </th>
            <th>
                Content
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
                <%= Html.ActionLink(Html.Encode(StringHelpers.TruncateWithText(item.Title, 30, "...")), "Details", new { id = item.Id }) %>
            </td>
            <td>
                <%= Html.Encode(item.Name) %><br />
                <%= Html.Encode(item.Email) %>
            </td>
            <td>
                <%= Html.Encode(StringHelpers.TruncateWithText(item.Url, 50, "...")) %><br /><br />
                <%= Html.Encode(StringHelpers.TruncateWithText(item.Content, 50, "...")) %>
            </td>     
        </tr>    
    <% } %>
    </table>