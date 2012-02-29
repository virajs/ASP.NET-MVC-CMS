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



namespace ComLib.Web.Modules.Resources
{
    /// <summary>
    /// Resource entity.
    /// </summary>
    public partial class Resource : ActiveRecordBaseEntity<Resource>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Resource New()
        {
            Resource entity = new Resource(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppName
		/// </summary>
		public string AppName { get; set; }


		/// <summary>
		/// Get/Set Language
		/// </summary>
		public string Language { get; set; }


		/// <summary>
		/// Get/Set ResourceType
		/// </summary>
		public string ResourceType { get; set; }


		/// <summary>
		/// Get/Set Section
		/// </summary>
		public string Section { get; set; }


		/// <summary>
		/// Get/Set Key
		/// </summary>
		public string Key { get; set; }


		/// <summary>
		/// Get/Set ValType
		/// </summary>
		public string ValType { get; set; }


		/// <summary>
		/// Get/Set Name
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Get/Set Description
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// Get/Set Example
		/// </summary>
		public string Example { get; set; }




        
        
    }
}
