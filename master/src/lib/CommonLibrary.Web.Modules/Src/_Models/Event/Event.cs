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



namespace ComLib.Web.Modules.Events
{
    /// <summary>
    /// Event entity.
    /// </summary>
    public partial class Event : ActiveRecordBaseEntity<Event>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Event New()
        {
            Event entity = new Event(); 
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
		/// Get/Set StartDate
		/// </summary>
		public DateTime StartDate { get; set; }


		/// <summary>
		/// Get/Set EndDate
		/// </summary>
		public DateTime EndDate { get; set; }


		/// <summary>
		/// Get/Set StartTime
		/// </summary>
		public int StartTime { get; set; }


		/// <summary>
		/// Get/Set EndTime
		/// </summary>
		public int EndTime { get; set; }


		/// <summary>
		/// Get/Set IsFeatured
		/// </summary>
		public bool IsFeatured { get; set; }


		/// <summary>
		/// Get/Set IsTravelRequired
		/// </summary>
		public bool IsTravelRequired { get; set; }


		/// <summary>
		/// Get/Set IsConference
		/// </summary>
		public bool IsConference { get; set; }


		/// <summary>
		/// Get/Set Cost
		/// </summary>
		public double Cost { get; set; }


		/// <summary>
		/// Get/Set Skill
		/// </summary>
		public int Skill { get; set; }


		/// <summary>
		/// Get/Set Seats
		/// </summary>
		public int Seats { get; set; }


		/// <summary>
		/// Get/Set IsAgeApplicable
		/// </summary>
		public bool IsAgeApplicable { get; set; }


		/// <summary>
		/// Get/Set AgeFrom
		/// </summary>
		public int AgeFrom { get; set; }


		/// <summary>
		/// Get/Set AgeTo
		/// </summary>
		public int AgeTo { get; set; }


		/// <summary>
		/// Get/Set Email
		/// </summary>
		public string Email { get; set; }


		/// <summary>
		/// Get/Set Phone
		/// </summary>
		public string Phone { get; set; }


		/// <summary>
		/// Get/Set Url
		/// </summary>
		public string Url { get; set; }


		/// <summary>
		/// Get/Set Tags
		/// </summary>
		public string Tags { get; set; }


		/// <summary>
		/// Get/Set RefKey
		/// </summary>
		public string RefKey { get; set; }


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
		/// Get/Set TotalBookMarked
		/// </summary>
		public int TotalBookMarked { get; set; }


		/// <summary>
		/// Get/Set TotalAbuseReports
		/// </summary>
		public int TotalAbuseReports { get; set; }


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
