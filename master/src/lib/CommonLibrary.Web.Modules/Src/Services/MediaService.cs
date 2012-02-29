using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Web.Lib;
using ComLib.Web.Lib.Services;
using ComLib.Web.Modules.Media;

namespace ComLib.Web.Modules.Handlers
{
    /// <summary>
    /// Media handler to upload files and delete files from the system.
    /// </summary>
    public class MediaService : IMediaService
    {
        /// <summary>
        /// Handles media file uploads.
        /// </summary>
        /// <param name="request">The http request containing the files to upload.</param>
        /// <param name="modelState">The model state to store errors.</param>
        /// <param name="refId">The id of the entity the media files that media files associated with.</param>
        /// <param name="entityGroup">The type of entity the media files are associated with.</param>
        /// <returns></returns>
        public bool Upload(System.Web.HttpRequestBase request, System.Web.Mvc.ModelStateDictionary modelState, int id, string entityGroup)
        {
            return MediaHelper.CreateMediaFilesForEntity(request, modelState, id, entityGroup).Success;
        }


        /// <summary>
        /// Handles deleting media files by the reference entity id and the entity group.
        /// </summary>
        /// <param name="refId">The id of the entity the media files that should be deleted are associated with.</param>
        /// <param name="entityGroup">The type of the entity the media files are associated with.</param>
        /// <returns></returns>
        public bool Delete(int refId, string entityGroup)
        {
            return MediaHelper.DeleteFilesForEntity(refId, entityGroup);
        }
    }
}
