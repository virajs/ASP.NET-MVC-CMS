using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

using ComLib;
using ComLib.Entities;
using ComLib.Caching;
using ComLib.Extensions;
using ComLib.Data;
using ComLib.MapperSupport;
using ComLib.Web.HttpHandlers;
using ComLib.Web.Lib.Core;


namespace ComLib.Web.Modules.Media
{
    /// <summary>
    /// Media file helper class.
    /// </summary>
    public class MediaHelper 
    {
        private static bool _enableLogging = false;


        /// <summary>
        /// Enable Logging.
        /// </summary>
        public static bool EnabledLogging
        {
            get { return _enableLogging; }
            set { _enableLogging = value; }
        }


        /// <summary>
        /// Can handle media file for transmitting media.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool CanHandleMediaFile(HttpContext context)
        {
            if (context.Request.Url.AbsolutePath.Contains("media"))
            {
                if(_enableLogging) Logging.Logger.Info("CanHandleMediaFile: TRUE : " + context.Request.Url.AbsolutePath);
                return true;
            }
            if (_enableLogging) Logging.Logger.Info("CanHandleMediaFile: FALSE : " + context.Request.Url.AbsolutePath);
            return false;
        }


        /// <summary>
        /// Handle the media file for http handling of image files.
        /// </summary>
        /// <param name="context"></param>
        public static void HandleMediaFile(HttpContext context)
        {
            string fileName = context.Request.Url.AbsolutePath;
            try
            {
                if (_enableLogging) Logging.Logger.Debug("HandleMediaFile: " + fileName);
                MediaFile file = GetMediaFileContents(fileName);
                if (file != null)
                {
                    if (_enableLogging) Logging.Logger.Debug("HandleMediaFile: file retrieved : name: " + file.Name + ", id: " + file.Id);
                    string extension = file.Extension.Substring(1);
                    string contentType = string.Empty;

                    if (string.Compare(extension, "JPG") == 0)
                        contentType = "image/jpeg";
                    else
                        contentType = "image/" + extension;

                    int hashCode = file.FullName.GetHashCode();
                    bool isThumbnail = fileName.Contains("thumbnail");
                    HandlerHelper.SetHeaders(context, contentType, null, null, hashCode);
                    byte[] buffer = isThumbnail ? file.ContentsForThumbNail : file.Contents;
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    if (_enableLogging) Logging.Logger.Info("HandleMediaFile: file NOT found : " + fileName);
                    context.Response.Redirect("/NotFound");
                }
            }
            catch (Exception ex)
            {
                if (_enableLogging) Logging.Logger.Error("Error handling media file", ex);
                context.Response.Redirect("/NotFound");
            }
        }


        /// <summary>
        /// Gets the media file represented by the url.
        /// </summary>
        /// <param name="url">e.g. /media/12/event-123.jpg</param>
        /// <returns></returns>
        public static MediaFile GetMediaFileContents(string url)
        {
            int ndxid = url.IndexOf("/media/") + 7;
            string id = url.Substring(ndxid, url.IndexOf("/", ndxid) - ndxid);
            int mediafileid = Convert.ToInt32(id);
            var file = MediaFile.Get(mediafileid);
            return file;
        }


        /// <summary>
        /// Create media files in the underlying datastore and associates them with the supplied entity id and entitytype.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelState"></param>
        /// <param name="id"></param>
        /// <param name="isParentDirectoryId"></param>
        /// <returns></returns>
        public static BoolMessageItem<IList<MediaFile>> CreateMediaFilesForEntity(HttpRequestBase request, ModelStateDictionary modelState, int entityId, string entityType)
        {
            int entityTypeId = ComLib.Web.Lib.Core.ModuleMap.Instance.GetId(entityType);
            return CreateMediaFiles(request, modelState, entityId, entityTypeId, false);
        }


        /// <summary>
        /// Create media files in the underlying datastore using the files supplied in the httprequest.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelState"></param>
        /// <param name="id"></param>
        /// <param name="isParentDirectoryId"></param>
        /// <returns></returns>
        public static BoolMessageItem<IList<MediaFile>> CreateMediaFiles(HttpRequestBase request, ModelStateDictionary modelState, int id,  int entityTypeId = 1, bool isParentDirectoryId = true)
        {
            IList<MediaFile> files = MediaFileMapper.MapFiles(request, id, entityTypeId, isParentDirectoryId);
            
            if (files.IsNullOrEmpty())
            {
                modelState.AddModelError("Files", "No media files uploaded.");
                return new BoolMessageItem<IList<MediaFile>>(null, false, "No files were uploaded");
            }
            else
            {
                var service = EntityRegistration.GetService<MediaFile>();
                int errorcount = 0;

                CreateMediaFiles(files, service, modelState, ref errorcount);
                if (errorcount == 0)
                {
                    return new BoolMessageItem<IList<MediaFile>>(files, true, string.Empty);
                }
                return new BoolMessageItem<IList<MediaFile>>(null, false, "Errors upload files");
            }
        }


        private static void CreateMediaFiles(IList<MediaFile> files, IEntityService<MediaFile> service, ModelStateDictionary modelState, ref int errorcount)
        {
            foreach (var mediafile in files)
            {
                // Save the entity.                
                service.Create(mediafile);
                errorcount += mediafile.Errors.Count;
                if (mediafile.Errors.Count > 0)
                    mediafile.Errors.EachFull(error => modelState.AddModelError("", error));
            }
        }


        /// <summary>
        /// Create thumbnails for the files specified and puts them as byte[] into the ContentsThumbnail properties.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static void CreateThumbNailsFor(IList<MediaFile> files, bool handleFileSystemFiles = false)
        {
            if (files == null || files.Count == 0)
                return;

            foreach (var file in files)
            {
                if (!file.IsExternalFile && file.IsImage)
                    file.ToThumbNail(processLocalFileSystemFile: handleFileSystemFiles);
            }
            return;
        }


        /// <summary>
        /// Delete files associated w/ the specified entity.
        /// </summary>
        /// <param name="refId"></param>
        /// <param name="entityGroup"></param>
        public static bool DeleteFilesForEntity(int refId, string entityGroup)
        {
            int entityGroupid = ModuleMap.Instance.GetId(entityGroup);
            var service = EntityRegistration.GetService<MediaFile>();
            bool result = true;
            try
            {
                service.Repository.Delete(Query<MediaFile>.New().Where(m => m.RefGroupId).Is(entityGroupid).And(m => m.RefId).Is(refId));
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        ///@Example:
        ///  var imageData = 
        ///  {
        ///      Name  : "Testing of images",
        ///      Description: "This is the description of the group of images",
        ///      Links : ["/images/sample_1.jpg","/images/sample_2.jpg","/images/sample_3.jpg","/images/sample_4.jpg",
        ///               "/images/sample_5.jpg","/images/sample_6.jpg","/images/sample_7.jpg","/images/sample_8.jpg"],
        ///      Captions  : ["image 1", "image 2", "image 3", "image 4", "image 5", "image 6", "image 7", "image 8"],
        ///      ThumbNails: [],
        ///      Dims  : ["100wX100h","100wX100h","100wX100h","100wX100h","100wX100h","100wX100h","100wX100h","100wX100h"]
        ///  };
        public static string ToJsonForImageGallery(string variableName, MediaFolder folder, IList<MediaFile> allFiles, string owner)
        {
            var buffer = new StringBuilder(); 

            // only include ones that are public.
            var files = (from file in allFiles where file.IsPublic || string.Compare(file.CreateUser, owner) == 0 select file).ToList();
            var links = files.JoinDelimited(",", (file) => "\"" + file.AbsoluteUrl + "\"");
            links = links.Replace("\\", "/");
            var titles = files.JoinDelimited(",", (file) => ComLib.Web.Helpers.JsonHelper.EscapeString(file.Title, true) );
            var captions = files.JoinDelimited(",", (file) => ComLib.Web.Helpers.JsonHelper.EscapeString(file.Description, true));
            var dims = files.JoinDelimited(",", (file) => "[" + file.Width + "," + file.Height + "]");
            var thumbnails = "";
            var name = folder == null ? string.Empty : folder.Name;
            var desc = folder == null ? string.Empty : folder.Description;
            buffer.Append("var " + variableName + " = { " + Environment.NewLine);
            buffer.Append("Name : \"" + name + "\"," + Environment.NewLine);
            buffer.Append("Description : \"" + desc + "\"," + Environment.NewLine);
            buffer.Append("Titles: [" + titles + "]," + Environment.NewLine);
            buffer.Append("Links : [" + links + "]," + Environment.NewLine);
            buffer.Append("Captions : [" + captions + "]," + Environment.NewLine);
            buffer.Append("ThumbNails : [" + thumbnails + "]," + Environment.NewLine);
            buffer.Append("Dims : [" + dims + "]" + Environment.NewLine);
            buffer.Append("}; " + Environment.NewLine);
            return buffer.ToString();
        }
    }
}
