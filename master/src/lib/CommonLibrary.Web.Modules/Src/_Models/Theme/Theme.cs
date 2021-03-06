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



namespace ComLib.Web.Modules.Themes
{
    /// <summary>
    /// Theme entity.
    /// </summary>
    public partial class Theme : ActiveRecordBaseEntity<Theme>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Theme New()
        {
            Theme entity = new Theme(); 
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
		/// Get/Set Description
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Get/Set Path
		/// </summary>
		public string Path { get; set; }


		/// <summary>
		/// Get/Set Layouts
		/// </summary>
		public string Layouts { get; set; }


		/// <summary>
		/// Get/Set Zones
		/// </summary>
		public string Zones { get; set; }


		/// <summary>
		/// Get/Set SelectedLayout
		/// </summary>
		public string SelectedLayout { get; set; }


		/// <summary>
		/// Get/Set IsActive
		/// </summary>
		public bool IsActive { get; set; }


		/// <summary>
		/// Get/Set Author
		/// </summary>
		public string Author { get; set; }


		/// <summary>
		/// Get/Set Version
		/// </summary>
		public string Version { get; set; }


		/// <summary>
		/// Get/Set Email
		/// </summary>
		public string Email { get; set; }


		/// <summary>
		/// Get/Set Url
		/// </summary>
		public string Url { get; set; }


		/// <summary>
		/// Get/Set SortIndex
		/// </summary>
		public int SortIndex { get; set; }




        
        
    }
}
