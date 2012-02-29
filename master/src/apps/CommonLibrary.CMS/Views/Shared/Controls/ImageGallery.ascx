<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Media.ImageGalleryViewModel>" %>
<%@ Import Namespace="ComLib" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>
<%@ Import Namespace="ComLib.Data" %>

<%
    // Similar to http://galleria.aino.se/
    // http://speckyboy.com/2009/06/03/15-amazing-jquery-image-galleryslideshow-plugins-and-tutorials/
    // 1. Get all the images.
    // 2. How many to display accross?
    // 3. Determine the edit images url.
    // What should the edit url be if enabled editing of files?
    // Get all images matching query.
    var images = MediaFile.Find(Model);
    int numAcross = Model.NumberAcross <= 1 ? 4 : Model.NumberAcross;
    string editUrl = Model.Mode == MediaGalleryViewMode.FolderId
                   ? "/mediafile/managebyfolder/" + Model.FolderId
                   : "/mediafile/managebyrefid/" + Model.RefId + "?refgroup=" + Model.RefGroupId;
    int numImages = images == null ? 0 : images.Count;
    MediaFolder folder = null;
    if (Model.Mode == MediaGalleryViewMode.FolderId)
    {
        folder = MediaFolder.Get(Model.FolderId);
    }
    var jsonForJSGallery = MediaHelper.ToJsonForImageGallery("_galleryData", folder, images, ComLib.Authentication.Auth.UserShortName);
 %>
 <% if(Model.ShowHeader){ %><h3>Image Gallery</h3><br /><%} %>

 <div id="gallery">
    <% if(Model.EnableEdit){ %><div id="imageEditArea"><input type="button" class="action" value="Edit Images" onclick="window.location = '<%= editUrl %>';" /></div><br /><br /><%} %>
    <% if(numImages == 0) { %><div>There are no images in this group.</div> <% } 
       else { %>
    <div id="gallery_01" class="gallery"></div>
    <%} %>
 </div>

 <script type="text/javascript">
     <%= jsonForJSGallery %>

     // Using the Javascript image gallery component located at /scripts/gallery.js
     <% if(images != null && images.Count > 0 ) { %>
     $(document).ready(function () {
         _gallery = new Gallery(_galleryData, {"DivId" : "gallery_01", "JSObjectName": "_gallery", "IsPaginated": true, "PageSize": 5});
         _gallery.Display(); 
     });
     <%} %>
 </script>
 <%= Html.Javascript("/scripts/app/Gallery.js") %>