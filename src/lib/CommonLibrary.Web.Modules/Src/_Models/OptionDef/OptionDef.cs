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



namespace ComLib.Web.Modules.OptionDefs
{
    /// <summary>
    /// OptionDef entity.
    /// </summary>
    public partial class OptionDef : ActiveRecordBaseEntity<OptionDef>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static OptionDef New()
        {
            OptionDef entity = new OptionDef(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppId
		/// </summary>
		public int AppId { get; set; }


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
		/// Get/Set Values
		/// </summary>
		public string Values { get; set; }


		/// <summary>
		/// Get/Set DefaultValue
		/// </summary>
		public string DefaultValue { get; set; }


		/// <summary>
		/// Get/Set DisplayStyle
		/// </summary>
		public string DisplayStyle { get; set; }


		/// <summary>
		/// Get/Set IsRequired
		/// </summary>
		public bool IsRequired { get; set; }


		/// <summary>
		/// Get/Set IsBasicType
		/// </summary>
		public bool IsBasicType { get; set; }


		/// <summary>
		/// Get/Set SortIndex
		/// </summary>
		public int SortIndex { get; set; }




        
        
    }
}
