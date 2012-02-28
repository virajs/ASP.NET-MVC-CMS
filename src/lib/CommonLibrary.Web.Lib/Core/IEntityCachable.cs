using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Core
{
    public interface IEntityCachable
    {
        /// <summary>
        /// Whether or not cach
        /// </summary>
        bool CacheEnabled { get; set; }


        /// <summary>
        /// Cache time in seconds.
        /// </summary>
        int CacheTime { get; set; }
    }
}
