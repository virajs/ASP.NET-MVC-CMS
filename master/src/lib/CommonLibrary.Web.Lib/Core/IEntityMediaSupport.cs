using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Core
{
    public interface IEntityMediaSupport
    {
        /// <summary>
        /// Whether or not this entity has media
        /// </summary>
        bool HasMediaFiles { get; set; }


        /// <summary>
        /// Total number of media files for this entity.
        /// </summary>
        int TotalMediaFiles { get; set; }
    }
}
