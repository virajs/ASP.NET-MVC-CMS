<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ComLib.Web.Modules.Media.MediaFolder>" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>

<h3>Image Gallery : <%= Html.Encode(Model.Name) %></h3><br /><br />

<% Html.RenderPartial("~/views/shared/controls/ImageGallery.ascx", new ComLib.Web.Modules.Media.ImageGalleryViewModel()
{
     ShowHeader = false,   NumberAcross = 8, FolderId = Model.Id, Mode = MediaGalleryViewMode.FolderId, Format = ImageGalleryViewFormat.Table
}); %>      
