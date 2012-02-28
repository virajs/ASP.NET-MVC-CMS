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



namespace ComLib.Web.Modules.Profiles
{
    /// <summary>
    /// Profile entity.
    /// </summary>
    public partial class Profile : ActiveRecordBaseEntity<Profile>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Profile New()
        {
            Profile entity = new Profile(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set UserId
		/// </summary>
		public int UserId { get; set; }


		/// <summary>
		/// Get/Set UserName
		/// </summary>
		public string UserName { get; set; }


		/// <summary>
		/// Get/Set About
		/// </summary>
		public string About { get; set; }


		/// <summary>
		/// Get/Set FirstName
		/// </summary>
		public string FirstName { get; set; }


		/// <summary>
		/// Get/Set LastName
		/// </summary>
		public string LastName { get; set; }


		/// <summary>
		/// Get/Set Alias
		/// </summary>
		public string Alias { get; set; }


		/// <summary>
		/// Get/Set IsFeatured
		/// </summary>
		public bool IsFeatured { get; set; }


		/// <summary>
		/// Get/Set Email
		/// </summary>
		public string Email { get; set; }


		/// <summary>
		/// Get/Set WebSite
		/// </summary>
		public string WebSite { get; set; }


		/// <summary>
		/// Get/Set AddressDisplayLevel
		/// </summary>
		public string AddressDisplayLevel { get; set; }


		/// <summary>
		/// Get/Set ImageRefId
		/// </summary>
		public int ImageRefId { get; set; }


		/// <summary>
		/// Get/Set EnableDisplayOfName
		/// </summary>
		public bool EnableDisplayOfName { get; set; }


		/// <summary>
		/// Get/Set IsGravatarEnabled
		/// </summary>
		public bool IsGravatarEnabled { get; set; }


		/// <summary>
		/// Get/Set IsAddressEnabled
		/// </summary>
		public bool IsAddressEnabled { get; set; }


		/// <summary>
		/// Get/Set EnableMessages
		/// </summary>
		public bool EnableMessages { get; set; }


		/// <summary>
		/// Get/Set HasMediaFiles
		/// </summary>
		public bool HasMediaFiles { get; set; }


		/// <summary>
		/// Get/Set TotalMediaFiles
		/// </summary>
		public int TotalMediaFiles { get; set; }


		private Address _address;
		/// <summary>
		/// Get/Set Address
		/// </summary>
		public Address Address
		 { 
		 get { return _address;  }
		 set { _address = value; }
		 }




        
        
    }
}
