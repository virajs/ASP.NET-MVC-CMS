using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ComLib.Web.Lib.Services
{
    public interface IMediaService
    {
        /// <summary>
        /// Handles media file uploads.
        /// </summary>
        /// <param name="request">The http request containing the files to upload.</param>
        /// <param name="modelState">The model state to store errors.</param>
        /// <param name="refId">The id of the entity the media files that media files associated with.</param>
        /// <param name="entityGroup">The type name of entity the media files are associated with.</param>
        /// <returns></returns>
        bool Upload(HttpRequestBase request, ModelStateDictionary modelState, int refId, string entityGroup);


        /// <summary>
        /// Handles deleting media files by the reference entity id and the entity group.
        /// </summary>
        /// <param name="refId">The id of the entity the media files that should be deleted are associated with.</param>
        /// <param name="entityGroup">The type name of the entity the media files are associated with.</param>
        /// <returns></returns>
        bool Delete(int refId, string entityGroup);
    }
}
