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



namespace ComLib.LocationSupport
{
    /// <summary>
    /// City entity.
    /// </summary>
    public partial class City : ActiveRecordBaseEntity<City>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static City New()
        {
            City entity = new City(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set StateId
		/// </summary>
		public int StateId { get; set; }


		/// <summary>
		/// Get/Set StateName
		/// </summary>
		public string StateName { get; set; }


		/// <summary>
		/// Get/Set ParentId
		/// </summary>
		public int ParentId { get; set; }


		/// <summary>
		/// Get/Set IsPopular
		/// </summary>
		public bool IsPopular { get; set; }


		/// <summary>
		/// Get/Set CountryId
		/// </summary>
		public int CountryId { get; set; }


		/// <summary>
		/// Get/Set CountryName
		/// </summary>
		public string CountryName { get; set; }


		/// <summary>
		/// Get/Set Name
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Get/Set Abbreviation
		/// </summary>
		public string Abbreviation { get; set; }


		/// <summary>
		/// Get/Set AliasRefName
		/// </summary>
		public string AliasRefName { get; set; }


		/// <summary>
		/// Get/Set IsActive
		/// </summary>
		public bool IsActive { get; set; }


		/// <summary>
		/// Get/Set IsAlias
		/// </summary>
		public bool IsAlias { get; set; }


		/// <summary>
		/// Get/Set AliasRefId
		/// </summary>
		public int AliasRefId { get; set; }




        
        
    }
}
