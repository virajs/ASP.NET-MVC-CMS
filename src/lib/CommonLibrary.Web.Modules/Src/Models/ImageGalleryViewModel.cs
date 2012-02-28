using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComLib.Web.Modules.Media
{
    public enum ImageGalleryViewFormat { List, Table };


    public class ImageGalleryViewModel
    {
        public ImageGalleryViewFormat Format = ImageGalleryViewFormat.Table;
        public int NumberAcross;
        public int PageSize;
        public int PageIndex;
        public int RefGroupId;
        public int RefId;
        public int AlbumId;
        public int AlbumName;
        public int UserId;
    }
}