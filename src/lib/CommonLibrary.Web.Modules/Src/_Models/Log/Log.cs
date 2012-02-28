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



namespace ComLib.Web.Modules.Logs
{
    /// <summary>
    /// Log entity.
    /// </summary>
    public partial class Log : ActiveRecordBaseEntity<Log>, IEntity
    {
        /// <summary>
        /// Creates a new instance of BlogPost and 
        /// initializes it with a validator and settings.
        /// </summary>
        /// <returns></returns>
        public static Log New()
        {
            Log entity = new Log(); 
            return entity;
        }       


        		/// <summary>
		/// Get/Set Application
		/// </summary>
		public string Application { get; set; }


		/// <summary>
		/// Get/Set Computer
		/// </summary>
		public string Computer { get; set; }


		/// <summary>
		/// Get/Set LogLevel
		/// </summary>
		public int LogLevel { get; set; }


		/// <summary>
		/// Get/Set Exception
		/// </summary>
		public string Exception { get; set; }


		/// <summary>
		/// Get/Set Message
		/// </summary>
		public string Message { get; set; }


		/// <summary>
		/// Get/Set UserName
		/// </summary>
		public string UserName { get; set; }




        
        protected override IValidator GetValidatorInternal()
            {
                var val = new EntityValidator((validationEvent) =>
                {
                    int initialErrorCount = validationEvent.Results.Count;
                    IValidationResults results = validationEvent.Results;            
                    Log entity = (Log)validationEvent.Target;
				Validation.IsStringLengthMatch(entity.Application, true, false, true, -1, 255, results, "Application" );
				Validation.IsStringLengthMatch(entity.Computer, true, false, true, -1, 255, results, "Computer" );
				Validation.IsNumericWithinRange(entity.LogLevel, false, false, -1, -1, results, "LogLevel");
				Validation.IsStringLengthMatch(entity.Exception, true, false, false, -1, -1, results, "Exception" );
				Validation.IsStringLengthMatch(entity.Message, true, false, false, -1, -1, results, "Message" );
				Validation.IsStringLengthMatch(entity.UserName, true, false, true, -1, 20, results, "UserName" );

                    return initialErrorCount == validationEvent.Results.Count;
                });
                return val;
            }
    }
}
