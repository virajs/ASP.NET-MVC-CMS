/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: ? 2009 Kishore Reddy
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
using System.IO;
using System.Web;

using ComLib.Data;
using ComLib.Entities;
using ComLib.Extensions;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;
using ComLib.Web.Lib.Core;


namespace ComLib.Web.Modules.Media
{
    public partial class MediaFolder : ActiveRecordBaseEntity<MediaFolder>, IEntity, IEntityMediaSupport
    {
        /// <summary>
        /// Length in kilobytes
        /// </summary>
        public int LengthInK { get { return Length == 0 ? 0 : Length / 1000; } }

        
        /// <summary>
        /// Perform actions before saving to database.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            if (this.LastWriteTime == DateTime.MinValue)
                LastWriteTime = DateTime.Now;
            return true;
        }


        /// <summary>
        /// Set up various properties before saving.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeDelete(object ctx)
        {
            var query = Query<MediaFile>.New().Where(m => m.ParentId).Is(Id);
            MediaFile.Repository.Delete(query);
            return true;
        }
    }
}
