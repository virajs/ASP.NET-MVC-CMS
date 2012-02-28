using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

using ComLib;
using ComLib.Entities;
using ComLib.Caching;
using ComLib.Extensions;
using ComLib.Data;
using ComLib.MapperSupport;
using ComLib.Web.HttpHandlers;


namespace ComLib.Web.Modules.Media
{
    /// <summary>
    /// Media file helper class.
    /// </summary>
    public class MediaFileMapper
    {
        /// <summary>
        /// Maps data from the media file edit form to the media file object.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static MediaFile MapFile(HttpRequestBase request)
        {
            return MapFile(request, string.Empty);
        }


        /// <summary>
        /// Map a Media File from the Form to an object.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fieldSuffix"></param>
        /// <returns></returns>
        public static MediaFile MapFile(HttpRequestBase request, string fieldSuffix)
        {
            var file = new MediaFile();
            MapFile(request, fieldSuffix, file);
            return file;
        }


        /// <summary>
        /// Maps data from the media file edit form to the media file object.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static void MapFile(HttpRequestBase request, string fieldSuffix, MediaFile file)
        {
            HttpPostedFileBase hpf = request.Files["file" + fieldSuffix];
            string externalfilename = request.Params["externalfile" + fieldSuffix];
            string filename = hpf.ContentLength == 0 ? externalfilename : hpf.FileName;
            file.Title = request.Params["title" + fieldSuffix];
            file.Description = request.Params["description" + fieldSuffix];
            file.SortIndex = ComLib.Extensions.NameValueExtensions.GetOrDefault<int>(request.Params, "SortIndex", file.SortIndex);
            file.IsPublic = true;
            if (file.LastWriteTime == DateTime.MinValue)
                file.LastWriteTime = DateTime.Now;

            // No Content?
            if (hpf.ContentLength == 0 && string.IsNullOrEmpty(externalfilename))
                return;

            // Get the file as a byte[]
            if (hpf.ContentLength > 0)
                file.Contents = ComLib.Web.WebUtils.GetContentOfFileAsBytes(hpf);

            // This will autoset the Name and Extension properties.
            file.FullNameRaw = filename;
            file.Length = hpf.ContentLength;
            
            // Set up the thumbnail.
            if (!file.IsExternalFile && file.IsImage)
                file.ToThumbNail(processLocalFileSystemFile: true);
        }


        /// <summary>
        /// Whether or not the user upload a file or specified an external file link.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="fieldSuffix"></param>
        /// <returns></returns>
        public static bool IsFileUploaded(HttpRequestBase request, string fieldSuffix)
        {
            HttpPostedFileBase hpf = request.Files["file" + fieldSuffix];
            string externalfilename = request.Params["externalfile" + fieldSuffix];

            // Both file and external file not supplied? 
            // Then it's an empty media file entry, skip this.
            if (hpf.ContentLength == 0 && string.IsNullOrEmpty(externalfilename))
                return false;
            return true;
        }


        /// <summary>
        /// Maps Form Collection data into MediaFile objects.
        /// </summary>
        /// <param name="request">Http Request containing the form data.</param>
        /// <param name="refid">Applicable refid to apply. This can either be the id of an event or the id of a media folder</param>
        /// <param name="isParentDirectoryId">Whether or not the refid represents a media folder</param>
        /// <returns></returns>
        public static IList<MediaFile> MapFiles(HttpRequestBase request, int refid, int refGroupId, bool isParentDirectoryId )
        {
            IList<MediaFile> files = new List<MediaFile>();
            for (int ndx = 0; ndx < request.Files.Count; ndx++)
            {
                if (IsFileUploaded(request, ndx.ToString()))
                {
                    var file = MapFile(request, ndx.ToString());
                    if (isParentDirectoryId) 
                        file.ParentId = refid;
                    else 
                    {   
                        file.RefId = refid;
                        file.RefGroupId = refGroupId;
                    }
                    files.Add(file);
                }
            }
            return files;
        }
    }
}
