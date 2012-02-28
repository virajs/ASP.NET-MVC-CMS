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



namespace ComLib.Web.Modules.Tags
{
    /// <summary>
    /// Tag entity.
    /// </summary>
    public partial class Tag : ActiveRecordBaseEntity<Tag>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Tag New()
        {
            Tag entity = new Tag(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set AppId
		/// </summary>
		public int AppId { get; set; }


		/// <summary>
		/// Get/Set GroupId
		/// </summary>
		public int GroupId { get; set; }


		/// <summary>
		/// Get/Set Name
		/// </summary>
		public string Name { get; set; }


		/// <summary>
		/// Get/Set RefId
		/// </summary>
		public int RefId { get; set; }




        
        protected override IValidator GetValidatorInternal()
            {
                var val = new EntityValidator((validationEvent) =>
                {
                    int initialErrorCount = validationEvent.Results.Count;
                    IValidationResults results = validationEvent.Results;            
                    Tag entity = (Tag)validationEvent.Target;
				Validation.IsNumericWithinRange(entity.AppId, false, false, -1, -1, results, "AppId");
				Validation.IsNumericWithinRange(entity.GroupId, false, false, -1, -1, results, "GroupId");
				Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 20, results, "Name" );
				Validation.IsNumericWithinRange(entity.RefId, false, false, -1, -1, results, "RefId");

                    return initialErrorCount == validationEvent.Results.Count;
                });
                return val;
            }
    }
}
