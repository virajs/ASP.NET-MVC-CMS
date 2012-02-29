<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EntityListViewModel<ComLib.Web.Modules.Media.MediaFolder>>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>

    <%= Html.ActionLink("Add Folder", "Create", "MediaFolder", null, new { @class = "actionbutton" })%>
    <br /><br />

    <table class="systemlist">
    <tr>
        <th></th>
        <th style="width:160px">Name</th>
        <th>Updated</th>
        <th>Size</th>
        <th></th>        
    </tr>
    <%  foreach (var folder in Model.Items) { %>
        <tr>
            <td>
                <% Html.RenderPartial("~/views/shared/Controls/EntityManage.ascx", new EntityListManageViewModel() { Id = folder.Id, ViewInfo = Model, ShowCopy = false }); %>
            </td>
            <td><%= Html.ActionLink(folder.Name, "Details", "MediaFolder", new { id = folder.Id }, null)%></td>
            <td><%= folder.UpdateDate.ToShortDateString() %></td>
            <td><%= folder.LengthInK %> K</td>
            <td><%= Html.ActionLink("Add", "CreateInFolder", "MediaFile", new { id = folder.Id }, null)%>  | 
                 <%= Html.ActionLink("Manage", "ManageByFolder", "MediaFile", new { id = folder.Id }, null)%></td>
        </tr>
    <% } %>
    </table>