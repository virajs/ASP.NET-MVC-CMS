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
using System.Data;
using System.Web;

using ComLib.Entities;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;
using ComLib.Data;


namespace ComLib.Web.Modules.Tags
{
    /// <summary>
    /// Tag entity.
    /// </summary>
    public partial class Tag : ActiveRecordBaseEntity<Tag>, IEntity
    {

        /// <summary>
        /// Tags can be associated w/ various groups.
        /// e.g. for blogposts, events, etc.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="maxTags"></param>
        /// <returns></returns>
        public static IList<TagSummary> GetByGroup(int groupId, int maxTags)
        {
            var repo = Repository;
            DataTable table = repo.Group(Query<Tag>.New().Where( t => t.GroupId).Is(groupId).Limit(maxTags), "Name");
            IList<TagSummary> tagSummaries = new List<TagSummary>();

            foreach (DataRow row in table.Rows)
            {
                tagSummaries.Add(new TagSummary() { Name = row["Name"].ToString(), Count = Convert.ToInt32(row["Count"]) });
            }
            return tagSummaries;
        }


        /// <summary>
        /// Gets the tags for the group and builds the tag cloud using the template.
        /// The template should have a ${size} and ${name} keywords that will be replaced
        /// by the size(1-6) and tagname respectively.
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="maxTags"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string BuildTagCloud(int groupId, int maxTags, string template)
        {
            IList<TagSummary> tagData = GetByGroup(groupId, maxTags);
            return BuildTagCloud(tagData, template);
        }


        /// <summary>
        /// Generate the tag clound using a template provided by caller.
        /// e.g. ( span id="weight${weight}" ${name} ${urlLink} ).
        /// </summary>
        /// <param name="tagData"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string BuildTagCloud(IList<TagSummary> tagData, string template)
        {
            // Need to get min/max.
            StringBuilder buffer = new StringBuilder();            
            if (tagData == null || tagData.Count == 0)
                return string.Empty;

            double min = tagData.Min(t => t.Count);
            double max = tagData.Max(t => t.Count);
            
            foreach (var tag in tagData)
            {
                // Get ratio of 1 tag in relation to max
                double ratio = ((double)tag.Count / max) * 100;
                int size = 0;

                if (ratio >= 98) size = 1;
                else if (ratio >= 80) size = 2;
                else if (ratio >= 60) size = 3;
                else if (ratio >= 40) size = 4;
                else if (ratio >= 20) size = 5;
                else if (ratio >= 5) size = 6;
                else size = 0;

                // Exclude very low weights.
                if (size > 0)
                {
                    string output = template.Replace("${size}", size.ToString());
                    string urltag = System.Web.HttpUtility.UrlEncode(tag.Name);
                    output = output.Replace("${urltagname}", urltag);
                    output = output.Replace("${tagname}", tag.Name);
                    buffer.Append(output);
                }
            }
            return buffer.ToString();
        }


        public class TagSummary
        {
            public string Name;
            public int Count;
        }
    }
}
