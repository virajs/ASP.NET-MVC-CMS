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



namespace ComLib.Web.Modules.Widgets
{
    /// <summary>
    /// Widget entity.
    /// </summary>
    public partial class Widget : ActiveRecordBaseEntity<Widget>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Widget New()
        {
            Widget entity = new Widget(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set Name
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Get/Set Description
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Get/Set FullTypeName
		/// </summary>
		public string FullTypeName { get; set; }


		/// <summary>
		/// Get/Set Path
		/// </summary>
		public string Path { get; set; }


		/// <summary>
		/// Get/Set Version
		/// </summary>
		public string Version { get; set; }


		/// <summary>
		/// Get/Set Author
		/// </summary>
		public string Author { get; set; }


		/// <summary>
		/// Get/Set AuthorUrl
		/// </summary>
		public string AuthorUrl { get; set; }


		/// <summary>
		/// Get/Set Email
		/// </summary>
		public string Email { get; set; }


		/// <summary>
		/// Get/Set Url
		/// </summary>
		public string Url { get; set; }


		/// <summary>
		/// Get/Set IncludeProperties
		/// </summary>
		public string IncludeProperties { get; set; }


		/// <summary>
		/// Get/Set ExcludeProperties
		/// </summary>
		public string ExcludeProperties { get; set; }


		/// <summary>
		/// Get/Set StringClobProperties
		/// </summary>
		public string StringClobProperties { get; set; }


		/// <summary>
		/// Get/Set PathToEditor
		/// </summary>
		public string PathToEditor { get; set; }


		/// <summary>
		/// Get/Set DeclaringType
		/// </summary>
		public string DeclaringType { get; set; }


		/// <summary>
		/// Get/Set DeclaringAssembly
		/// </summary>
		public string DeclaringAssembly { get; set; }


		/// <summary>
		/// Get/Set SortIndex
		/// </summary>
		public int SortIndex { get; set; }


		/// <summary>
		/// Get/Set IsCacheable
		/// </summary>
		public bool IsCacheable { get; set; }


		/// <summary>
		/// Get/Set IsEditable
		/// </summary>
		public bool IsEditable { get; set; }




        
        
    }
}
