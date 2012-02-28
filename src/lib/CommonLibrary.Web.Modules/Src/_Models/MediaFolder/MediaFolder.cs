/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;



namespace ComLib.Web.Modules.Media
{
    /// <summary>
    /// MediaFolder entity.
    /// </summary>
    public partial class MediaFolder : ActiveRecordBaseEntity<MediaFolder>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static MediaFolder New()
        {
            MediaFolder entity = new MediaFolder(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppId
		/// </summary>
		public int AppId { get; set; }


		/// <summary>
		/// Get/Set Name
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Get/Set FullName
		/// </summary>
		public string FullName { get; set; }


		/// <summary>
		/// Get/Set DirectoryName
		/// </summary>
		public string DirectoryName { get; set; }


		/// <summary>
		/// Get/Set Extension
		/// </summary>
		public string Extension { get; set; }


		/// <summary>
		/// Get/Set Description
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Get/Set Length
		/// </summary>
		public int Length { get; set; }


		/// <summary>
		/// Get/Set LastWriteTime
		/// </summary>
		public DateTime LastWriteTime { get; set; }


		/// <summary>
		/// Get/Set SortIndex
		/// </summary>
		public int SortIndex { get; set; }


		/// <summary>
		/// Get/Set ParentId
		/// </summary>
		public int ParentId { get; set; }


		/// <summary>
		/// Get/Set FileType
		/// </summary>
		public int FileType { get; set; }


		/// <summary>
		/// Get/Set IsPublic
		/// </summary>
		public bool IsPublic { get; set; }


		/// <summary>
		/// Get/Set HasMediaFiles
		/// </summary>
		public bool HasMediaFiles { get; set; }


		/// <summary>
		/// Get/Set TotalMediaFiles
		/// </summary>
		public int TotalMediaFiles { get; set; }


		/// <summary>
		/// Get/Set IsExternalFile
		/// </summary>
		public bool IsExternalFile { get; set; }


		/// <summary>
		/// Get/Set ExternalFileSource
		/// </summary>
		public string ExternalFileSource { get; set; }


		/// <summary>
		/// Get/Set HasThumbnail
		/// </summary>
		public bool HasThumbnail { get; set; }




        
        
    }
}
