<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MediaFilesViewModel>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>


    <a href="<%=Model.UrlCreate %>" id="createlink" class="actionbutton">Add Files</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <% if(Model.Mode == MediaGalleryViewMode.FolderId){ %>
        <a href="<%=Model.UrlBack %>"   id="A3" class="actionbutton">Back</a>&nbsp;&nbsp;&nbsp;&nbsp;
    <%} %>
    <% if(Model.Mode == MediaGalleryViewMode.Entity){ %>
        <a href="<%=Model.UrlEdit %>"   id="A1" class="actionbutton">Back to Edit</a>&nbsp;&nbsp;&nbsp;&nbsp;
        <a href="<%=Model.UrlBack %>"   id="A2" class="actionbutton">Back to Details</a>&nbsp;&nbsp;&nbsp;&nbsp;
    <%} %>
    <br /><br />
    
    <table class="systemlist">
    <tr>
        <th></th>
        <th></th>
        <th style="width:160px">Name</th>
        <th>Type</th>
        <th>Size</th>  
    </tr>
    <%  foreach (var file in Model.Items) { %>
        <tr>
            <td>
                <% Html.RenderPartial("~/views/shared/Controls/EntityManage.ascx", new EntityListManageViewModel() {  Id = file.Id, ViewInfo = Model, ShowCopy = false }); %>
            </td>
            <td><% if(file.HasThumbnail){ %>
                <img src="<%= file.AbsoluteUrlThumbnail %>" alt="<%= file.Description %>" />
                <%} %>
            </td>
            <td><%= Html.ActionLink(file.Name, "Edit", "MediaFile", new { id = file.Id }, null)%> <br />
                <%= Html.Encode(file.AbsoluteUrl) %><br /><br /></td>
            <td><%= Html.Encode(file.Extension) %></td>
            <td><%= file.LengthInK%> K</td>
        </tr>
    <% } %>
    </table>