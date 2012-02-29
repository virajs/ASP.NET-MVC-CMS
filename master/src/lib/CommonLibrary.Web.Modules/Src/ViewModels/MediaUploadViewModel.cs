using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComLib.Web.Modules.Media
{
    public class MediaUploadViewModel
    {
        public int NumberOfUploadsAllowed { get; set; }
        public bool AllowExternalFiles { get; set; }
        public bool ShowDetailUI { get; set; }
        public int Width { get; set; }


        public MediaUploadViewModel()
        {
            NumberOfUploadsAllowed = 5;
            ShowDetailUI = false;
            AllowExternalFiles = true;
        }
    }
}