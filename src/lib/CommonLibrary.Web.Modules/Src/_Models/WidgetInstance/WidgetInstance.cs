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
    /// WidgetInstance entity.
    /// </summary>
    public partial class WidgetInstance : ActiveRecordBaseEntity<WidgetInstance>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static WidgetInstance New()
        {
            WidgetInstance entity = new WidgetInstance(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppId
		/// </summary>
		public int AppId { get; set; }


		/// <summary>
		/// Get/Set WidgetId
		/// </summary>
		public int WidgetId { get; set; }


		/// <summary>
		/// Get/Set Header
		/// </summary>
		public string Header { get; set; }


		/// <summary>
		/// Get/Set Zone
		/// </summary>
		public string Zone { get; set; }


		/// <summary>
		/// Get/Set DefName
		/// </summary>
		public string DefName { get; set; }


		/// <summary>
		/// Get/Set Roles
		/// </summary>
		public string Roles { get; set; }


		/// <summary>
		/// Get/Set StateData
		/// </summary>
		public string StateData { get; set; }


		/// <summary>
		/// Get/Set SortIndex
		/// </summary>
		public int SortIndex { get; set; }


		/// <summary>
		/// Get/Set IsActive
		/// </summary>
		public bool IsActive { get; set; }




        
        
    }
}
