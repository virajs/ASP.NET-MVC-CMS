<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="ComLib.Web.Modules.Media" %>
<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
   <style type="text/css">
        .gallery { border:solid 1px black; width:400px; }
        .gallery .name { color:#333333; font-size:15; }
        .gallery .desc { color:#999999; font-size:13; }
        .gallery .images {}
        .gallery .images .detail {}
        .gallery .caption { color:#cccccc; font-size:11;}
        .gallery .info {font-family:Verdana; color:#999999; font-size:9px; font-weight:normal; } 
        .gallery .navarea {background-color:Black; }
        .gallery .navarea .inner {background-color:Black; margin-left:auto; margin-right:auto; }
        .gallery .navarea .inner .nav { } 
        .gallery .navarea .inner .nav a { color:White; text-decoration: none; font-size:13px; }
        .gallery .navarea .inner .nav a:link { color:White; text-decoration: none; }
        .gallery .navarea .inner .nav a:visited { color:White; text-decoration: none; }
        .gallery .navarea .inner .nav a:hover { color:White; text-decoration: none; }
        .gallery .navarea .inner .list { } 
        .gallery .navarea .inner .list a { margin:2px 2px 2px 2px; }
    </style>
   <script type="text/javascript" src="/scripts/app/gallery.js"></script>
   <script type="text/javascript">

       var _gallery = null;
       function TestGallery()
       {
            _gallery = new Gallery(gallerySampleData, {"DivId" : "gallery_01", "JSObjectName": "_gallery", "IsPaginated": true, "PageSize": 4});
            _gallery.Display();
       }

   </script>
   <input type="button" id="test_gallery" value="Show" onclick="TestGallery();" />
   <div id="gallery_01" class="gallery"></div>

    <%  /*
        Html.RenderPartial("~/views/shared/controls/ImageGallery.ascx", new ComLib.Web.Modules.Media.ImageGalleryViewModel()
        {
             NumberAcross = 8, FolderId = 1, Mode = MediaGalleryViewMode.FolderId, Format = ImageGalleryViewFormat.Table
        }); 
        */   
     %>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
</asp:Content>
