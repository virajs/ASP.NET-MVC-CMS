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



namespace ComLib.Web.Modules.Pages
{
    /// <summary>
    /// Page entity.
    /// </summary>
    public partial class Page : ActiveRecordBaseEntity<Page>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Page New()
        {
            Page entity = new Page(); 
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
		public int IsPublished { get; set; }


		/// <summary>
		/// Get/Set Keywords
		/// </summary>
		public string Keywords { get; set; }


		/// <summary>
		/// Get/Set Slug
		/// </summary>
		public string Slug { get; set; }


		/// <summary>
		/// Get/Set AccessRoles
		/// </summary>
		public string AccessRoles { get; set; }


		/// <summary>
		/// Get/Set Parent
		/// </summary>
		public int Parent { get; set; }


		/// <summary>
		/// Get/Set IsPublic
		/// </summary>
		public bool IsPublic { get; set; }


		/// <summary>
		/// Get/Set IsFrontPage
		/// </summary>
		public bool IsFrontPage { get; set; }




        
        protected override IValidator GetValidatorInternal()
            {
                var val = new EntityValidator((validationEvent) =>
                {
                    int initialErrorCount = validationEvent.Results.Count;
                    IValidationResults results = validationEvent.Results;            
                    Page entity = (Page)validationEvent.Target;
				Validation.IsNumericWithinRange(entity.AppId, false, false, -1, -1, results, "AppId");
				Validation.IsStringLengthMatch(entity.Title, false, true, true, 1, 100, results, "Title" );
				Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 100, results, "Description" );
				Validation.IsStringLengthMatch(entity.Content, false, false, false, -1, -1, results, "Content" );
				Validation.IsNumericWithinRange(entity.IsPublished, false, false, -1, -1, results, "IsPublished");
				Validation.IsStringLengthMatch(entity.Keywords, true, false, true, -1, 80, results, "Keywords" );
				Validation.IsStringLengthMatch(entity.Slug, true, false, true, -1, 150, results, "Slug" );
				Validation.IsStringLengthMatch(entity.AccessRoles, true, false, true, -1, 50, results, "AccessRoles" );
				Validation.IsNumericWithinRange(entity.Parent, false, false, -1, -1, results, "Parent");

                    return initialErrorCount == validationEvent.Results.Count;
                });
                return val;
            }
    }
}
