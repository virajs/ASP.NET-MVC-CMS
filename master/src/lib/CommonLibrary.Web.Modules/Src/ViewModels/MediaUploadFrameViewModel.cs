using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Modules.Media
{
    public class MediaUploadFrameViewModel : MediaUploadViewModel
    {
        public string SourceUrl { get; set; }
        public string FormAction { get; set; }
        /// <summary>
        /// yes, no, auto
        /// </summary>
        public string ScrollMode { get; set; }
        public bool IsFolderMode { get; set; }
        public int RefId { get; set; }
        public string ModelName { get; set; }
        public int Height { get; set; }
        public string JavascriptCallback { get; set; }
        public bool IsUploadMode { get; set; }
        public string UploadResultMessage { get; set; }
        public bool RunJavascriptCallBack { get; set; }
        public bool ShowManageLink { get; set; }

        public MediaUploadFrameViewModel() { }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="frameSourceUrl"></param>
        /// <param name="numberOfFileUploads"></param>
        public MediaUploadFrameViewModel(string frameSourceUrl, int numberOfFileUploads)
        {
            SourceUrl = frameSourceUrl;
            NumberOfUploadsAllowed = numberOfFileUploads;
        }


        /// <summary>
        /// The source url for the frame.
        /// </summary>
        public string FullFrameSourceUrl
        {
            get
            {
                string action = this.IsFolderMode ? "&formAction=CreateInFolder" : "&formAction=CreateForEntity";
                string folderParams = this.IsFolderMode 
                                    ? "&isFolderMode=true&refId=" + this.RefId
                                    : "&isFolderMode=false&refId=" + this.RefId + "&modelName=" + this.ModelName;

                string format = "{0}?&numberOfUploadsAllowed={1}&showDetailUI={2}&width={3}&height={4}&jscallback={5}";
                string url = string.Format(format, this.SourceUrl, this.NumberOfUploadsAllowed, this.ShowDetailUI.ToString().ToLower(), this.Width, this.Height, this.JavascriptCallback);
                url += action + folderParams;
                return url;
            }
        }


        /// <summary>
        /// Form post action
        /// </summary>
        public string ActionUrl
        {
            get
            {
                int refgroup = this.IsFolderMode ? 0 : ComLib.Web.Lib.Core.ModuleMap.Instance.GetId(this.ModelName);
                string action = this.IsFolderMode ? "CreateInFolder" : "CreateForEntity";
                string queryParams = this.IsFolderMode
                                    ? "/" + this.RefId
                                    : "/" + this.RefId + "?refgroup=" + refgroup;

                string url = "/mediafile/" + action + queryParams;
                return url;
            }
        }
    }
}
