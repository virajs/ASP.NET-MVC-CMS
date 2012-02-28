using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.Web.Mvc.Html;


using ComLib.Authentication;
using ComLib.Entities;
using ComLib.Web.Lib.Core;
using ComLib.Web.Modules.Media;


namespace ComLib.Web.Lib.Extensions
{
    public static class HtmlHelperMediaExtensions
    {
        /// <summary>
        /// Render the media upload control ( an iframe using the media/upload.aspx ) file.
        /// </summary>
        /// <param name="html">Html helper.</param>
        /// <param name="model">The entity/model.</param>
        /// <param name="numberOfUploads">Total upload to display in the control.</param>
        /// <param name="isFolderMode">Whether or not this a folder mode</param>
        /// <param name="refId">Id of the folder if folder mode, otherwise id of the entity.</param>
        /// <param name="showDetailUI">Whether or not to show the detailed upload UI.</param>
        /// <param name="height">Height in pixels of the control.</param>
        /// <param name="width">Width in pixels of the control.</param>
        /// <param name="javascriptCallbackOnLoadComplete">A client side javascript function to call when the files are uploaded.</param>
        public static void RenderMediaUpload(this HtmlHelper html, IEntity model, int numberOfUploads, bool showManageLink = false, bool showDetailUI = true, string scrollMode = "no", int height = 600, int width = 320, string javascriptCallbackOnLoadComplete = "")
        {
            // Validate before rendering the media upload.
            if (model == null) return;
            if (!(model is IEntityMediaSupport)) return;
            if (!model.IsPersistant()) return;
            if (!Auth.IsUser(model.CreateUser)) return;
            bool isFolderMode = model is MediaFolder;

            if(height == 600 && numberOfUploads > 1)
                height = 600 * numberOfUploads;

            html.RenderPartial("Controls/MediaUploadFrame",
                  new MediaUploadFrameViewModel()
                  {
                      SourceUrl = "/mediafile/upload",
                      ScrollMode = scrollMode,
                      NumberOfUploadsAllowed = numberOfUploads,
                      ModelName = model.GetType().Name,
                      IsFolderMode = isFolderMode,
                      RefId = model.Id,
                      ShowDetailUI = showDetailUI,
                      Height = height,
                      Width = width,
                      ShowManageLink = showManageLink,
                      JavascriptCallback = javascriptCallbackOnLoadComplete
                  }); 
        }


        /// <summary>
        /// Renders the media gallery.
        /// </summary>
        /// <param name="html">The html helper</param>
        /// <param name="model">The model to render the media gallery for.</param>
        /// <param name="numberAcross">Number of image to display accross</param>
        /// <param name="format">Table mode or list </param>
        /// <param name="javascriptCallbackOnLoadComplete"></param>
        public static void RenderMediaGallery(this HtmlHelper html, IEntity model, int numberAcross = 8, ImageGalleryViewFormat format = ImageGalleryViewFormat.Table, string javascriptCallbackOnLoadComplete = "")
        {
            // Validate before rendering the media upload.
            if (model == null) return;
            if (!(model is IEntityMediaSupport)) return;
            var mediaGalleryViewMode = model is MediaFolder ? MediaGalleryViewMode.FolderId : MediaGalleryViewMode.Entity;

            html.RenderPartial("~/views/shared/controls/ImageGallery.ascx", new ComLib.Web.Modules.Media.ImageGalleryViewModel()
            {
                NumberAcross = numberAcross,
                RefId = model.Id,
                RefGroupId = ModuleMap.Instance.GetId(model.GetType()),
                Mode = mediaGalleryViewMode,
                Format = ImageGalleryViewFormat.Table,
                EnableEdit = true
            });
        }
    }
}
