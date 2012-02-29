<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Media.MediaFile>" %>

<% if (ViewData["isediting"] == null )
   { %>
<% Html.RenderPartial("~/views/shared/controls/mediaupload.ascx", new ComLib.Web.Modules.Media.MediaUploadViewModel() { ShowDetailUI = true }); %>
<% }
   else { %>
<table>
       <tr>
            <td style="vertical-align:top">                
                <label for="Description">Update File</label>
                <input type="file" name="file" id="file" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <label for="Description">Existing File Reference</label>
                <input type="text" id="externalfile" value="<%= Model.IsExternalFile ? Model.FullName : Model.IsFileSystemFile ? Model.FullName : string.Empty %>" /><br />
                <span class="example">http://www.flickr.com/johndoe/green-earth.jpg</span><br /><br />
            </td>
       </tr>
       <tr>
            <td style="vertical-align:top">
                <label for="Description">Description</label>             
                <%= Html.TextBox("Description", Model.Description) %>
            </td>
       </tr>
       <tr>
            <td style="vertical-align:top">
                <label for="IsPublic">Is Public</label>             
                <%= Html.CheckBox("IsPublic", Model.IsPublic) %>
            </td>
       </tr>
       <tr>
            <td style="vertical-align:top">
                <label for="SortIndex">Order #</label> 
                <%= Html.TextBox("SortIndex", Model.SortIndex )%><br />
                <span class="example">1 or 5 ( used for sorting )</span><br /><br />
            </td>
       </tr>  
       <tr>
            <td colspan="2">
                <label for="Perma Link">Perma Link</label>
                <%= Html.Encode(Model.AbsoluteUrl) %>
            </td>
       </tr>     
    </table>
   <% } %>