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



namespace ComLib.Web.Modules.Faqs
{
    /// <summary>
    /// Faq entity.
    /// </summary>
    public partial class Faq : ActiveRecordBaseEntity<Faq>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Faq New()
        {
            Faq entity = new Faq(); 
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
		/// Get/Set Content
		/// </summary>
		public string Content { get; set; }


		/// <summary>
		/// Get/Set Group
		/// </summary>
		public string Group { get; set; }




        
        protected override IValidator GetValidatorInternal()
            {
                var val = new EntityValidator((validationEvent) =>
                {
                    int initialErrorCount = validationEvent.Results.Count;
                    IValidationResults results = validationEvent.Results;            
                    Faq entity = (Faq)validationEvent.Target;
				Validation.IsNumericWithinRange(entity.AppId, false, false, -1, -1, results, "AppId");
				Validation.IsStringLengthMatch(entity.Title, true, false, true, -1, 150, results, "Title" );
				Validation.IsStringLengthMatch(entity.Content, false, false, false, -1, -1, results, "Content" );
				Validation.IsStringLengthMatch(entity.Group, false, true, true, 1, 50, results, "Group" );

                    return initialErrorCount == validationEvent.Results.Count;
                });
                return val;
            }
    }
}
