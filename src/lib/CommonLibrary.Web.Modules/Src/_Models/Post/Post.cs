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



namespace ComLib.Web.Modules.Posts
{
    /// <summary>
    /// Post entity.
    /// </summary>
    public partial class Post : ActiveRecordBaseEntity<Post>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Post New()
        {
            Post entity = new Post(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppId
		/// </summary>
		public int AppId { get; set; }


		/// <summary>
		/// Get/Set Title
		/// </summary>
		public string Title { get; set; }


		/// <summary>
		/// Get/Set Description
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Get/Set Content
		/// </summary>
		public string Content { get; set; }


		/// <summary>
		/// Get/Set PublishDate
		/// </summary>
		public DateTime PublishDate { get; set; }


		/// <summary>
		/// Get/Set IsPublished
		/// </summary>
		public bool IsPublished { get; set; }


		/// <summary>
		/// Get/Set IsPublic
		/// </summary>
		public bool IsPublic { get; set; }


		/// <summary>
		/// Get/Set CategoryId
		/// </summary>
		public int CategoryId { get; set; }


		/// <summary>
		/// Get/Set Tags
		/// </summary>
		public string Tags { get; set; }


		/// <summary>
		/// Get/Set Slug
		/// </summary>
		public string Slug { get; set; }


		/// <summary>
		/// Get/Set RefKey
		/// </summary>
		public string RefKey { get; set; }


		/// <summary>
		/// Get/Set IsFavorite
		/// </summary>
		public bool IsFavorite { get; set; }


		/// <summary>
		/// Get/Set IsCommentEnabled
		/// </summary>
		public bool IsCommentEnabled { get; set; }


		/// <summary>
		/// Get/Set IsCommentModerated
		/// </summary>
		public bool IsCommentModerated { get; set; }


		/// <summary>
		/// Get/Set IsRatable
		/// </summary>
		public bool IsRatable { get; set; }


		/// <summary>
		/// Get/Set CommentCount
		/// </summary>
		public int CommentCount { get; set; }


		/// <summary>
		/// Get/Set ViewCount
		/// </summary>
		public int ViewCount { get; set; }


		/// <summary>
		/// Get/Set AverageRating
		/// </summary>
		public int AverageRating { get; set; }


		/// <summary>
		/// Get/Set TotalLiked
		/// </summary>
		public int TotalLiked { get; set; }


		/// <summary>
		/// Get/Set TotalDisLiked
		/// </summary>
		public int TotalDisLiked { get; set; }


		/// <summary>
		/// Get/Set HasMediaFiles
		/// </summary>
		public bool HasMediaFiles { get; set; }


		/// <summary>
		/// Get/Set TotalMediaFiles
		/// </summary>
		public int TotalMediaFiles { get; set; }




        
        
    }
}
