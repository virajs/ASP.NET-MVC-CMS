using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComLib.Web.Modules.Media
{
    public enum ImageGalleryViewFormat { List, Table };
    public enum MediaGalleryViewMode { FolderId, Entity }
   

    public class ImageGalleryViewModel
    {
        public ImageGalleryViewFormat Format = ImageGalleryViewFormat.Table;
        public MediaGalleryViewMode Mode = MediaGalleryViewMode.FolderId;
        public bool ShowHeader = true;
        public bool EnableEdit; 
        public int NumberAcross;
        public int RefGroupId;
        public int RefId;
        public int FolderId;
    }



    public class MediaFilesViewModel : ComLib.Web.Lib.Models.EntityListViewModel<MediaFile>
    {
        public MediaGalleryViewMode Mode = MediaGalleryViewMode.FolderId;
        public MediaFolder Folder; 
        public int FolderId;
        public int EntityId;
        public int EntityGroup;  
    }
}