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



namespace ComLib.Web.Modules.Flags
{
    /// <summary>
    /// Flag entity.
    /// </summary>
    public partial class Flag : ActiveRecordBaseEntity<Flag>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Flag New()
        {
            Flag entity = new Flag(); 
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
		/// Get/Set RefId
		/// </summary>
		public int RefId { get; set; }


		/// <summary>
		/// Get/Set FlagType
		/// </summary>
		public int FlagType { get; set; }


		/// <summary>
		/// Get/Set Model
		/// </summary>
		public string Model { get; set; }


		/// <summary>
		/// Get/Set Url
		/// </summary>
		public string Url { get; set; }


		/// <summary>
		/// Get/Set FlaggedByUser
		/// </summary>
		public string FlaggedByUser { get; set; }


		/// <summary>
		/// Get/Set FlaggedDate
		/// </summary>
		public DateTime FlaggedDate { get; set; }




        
        
    }
}
