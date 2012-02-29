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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;

using ComLib.Entities;
using ComLib.ValidationSupport;
using ComLib.Data;
using ComLib.Caching;
using ComLib.Extensions;
using ComLib.Web.Lib.Attributes;


namespace ComLib.Web.Modules.Themes
{
    /// <summary>
    /// Settings specific for blog posts.
    /// </summary>
    public class ThemeSettings : EntitySettings<Theme>
    {
        /// <summary>
        /// Set default values.
        /// </summary>
        public ThemeSettings()
        {
            CacheTime = 300;
            CacheOn = true;
        }


        public bool CacheOn { get; set; }
        public int CacheTime { get; set; }
    }



    public class Layout
    {
        public string Name;
        public string MasterName;
        public string ImagePage;
    }



    /// <summary>
    /// Theme entity.
    /// </summary>
    [Model(Id = 101, DisplayName = "Theme", Description = "Theme", IsPagable = true, IsSystemModel = true,
        IsExportable = true, IsImportable = true, FormatsForExport = "xml,ini", FormatsForImport = "xml,ini",
        RolesForCreate = "${Admin}", RolesForView = "?", RolesForIndex = "?", RolesForManage = "${Admin}",
        RolesForDelete = "${Admin}", RolesForImport = "${Admin}", RolesForExport = "${Admin}")]
    public partial class Theme : ActiveRecordBaseEntity<Theme>, IEntity
    {
        /// <summary>
        /// Singleton settings.
        /// </summary>
        private static ThemeSettings _settings = new ThemeSettings();


        /// <summary>
        /// List of available layouts.
        /// </summary>
        private static List<Layout> _availableLayouts;
        private static IDictionary<string, Layout> _availableLayoutsMap;
        private static ReadOnlyCollection<Layout> _availableLayoutsReadonly;
        private static IDictionary<string, Layout> _availableLayoutsMapReadonly;
        

        /// <summary>
        /// Sets the layouts.
        /// </summary>
        /// <param name="layouts">The layouts.</param>
        public static void SetLayouts(List<Layout> layouts)
        {
            _availableLayouts = layouts;
            _availableLayoutsMap = layouts.ToDictionary((layout) => layout.MasterName);
            _availableLayoutsReadonly = new ReadOnlyCollection<Layout>(_availableLayouts);
            _availableLayoutsMapReadonly = new ComLib.Collections.DictionaryReadOnly<string, Layout>(_availableLayoutsMap);
        }

        

        /// <summary>
        /// Gets the validator for this entity.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Theme entity = (Theme)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 150, results, "Name");
                Validation.IsStringLengthMatch(entity.Description, false, true, true, 1, 150, results, "Description");
                Validation.IsStringLengthMatch(entity.Path, false, true, true, 1, 150, results, "Path");
                Validation.IsStringLengthMatch(entity.Layouts, false, true, true, 1, 150, results, "Layouts");
                Validation.IsStringLengthMatch(entity.Author, false, true, true, 1, 150, results, "Author");
                Validation.IsNumericWithinRange(entity.Version, false, false, -1, -1, results, "Version");
                Validation.IsStringRegExMatch(entity.Email, false, RegexPatterns.Email, results, "Email");
                Validation.IsStringRegExMatch(entity.Url, true, RegexPatterns.Url, results, "Url");
                
                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Gets a value indicating whether this instance has restricted layouts.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has restricted layouts; otherwise, <c>false</c>.
        /// </value>
        public bool IsLayoutChangeable
        {
            get
            {
                bool isEmpty = string.IsNullOrEmpty(this.Layouts);

                if (isEmpty) return true;
                if (!isEmpty && string.Compare(Layouts, "all", true) == 0) return true;
                if (!isEmpty && Layouts.Contains(",")) return true;

                return false;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has restricted layouts.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has restricted layouts; otherwise, <c>false</c>.
        /// </value>
        public bool IsSingleLayout
        {
            get { return !IsLayoutChangeable; }
        }


        /// <summary>
        /// Gets the layouts map.
        /// </summary>
        /// <value>The layouts map.</value>
        public IDictionary<string, Layout> LayoutsMap
        {
            get { return _availableLayoutsMapReadonly; }
        }


        /// <summary>
        /// Gets a value indicating whether this instance has restricted layouts.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has restricted layouts; otherwise, <c>false</c>.
        /// </value>
        public ReadOnlyCollection<Layout> LayoutsList
        {
            get
            {
                // Only 1 layout available.
                if (IsSingleLayout)
                    return new ReadOnlyCollection<Layout>(new List<Layout>(){ _availableLayoutsMap[Layouts] });

                // All layouts possible.
                if (string.Compare("all", Layouts, true) == 0)
                    return _availableLayoutsReadonly;

                // Specific layouts possible.
                List<Layout> available = new List<Layout>();
                string[] names = Layouts.Split(',');
                foreach (string name in names)
                {
                    if (_availableLayoutsMap.ContainsKey(name))
                    {
                        available.Add(_availableLayoutsMap[name]);
                    }
                }
                return new ReadOnlyCollection<Layout>(available);
            }
        }


        /// <summary>
        /// The full path to the css file.
        /// </summary>
        public string FullCssPath
        {
            get { return Path + "/theme.css"; }
        }


        /// <summary>
        /// Settings for this entity.
        /// </summary>
        public static new ThemeSettings Settings
        {
            get { return _settings; }
        }


        /// <summary>
        /// Lookup by both id and string.
        /// </summary>
        public static LookupMulti<Theme> Lookup
        {
            get
            {
                // Cached lookup of menu items by id and name.
                var menulookup = Cacher.Get<LookupMulti<Theme>>("Theme_lookupbyname",
                        Theme.Settings.CacheOn, Theme.Settings.CacheTime.Seconds(),
                        () => Theme.Repository.ToLookUpMulti<string>(m => m.Name));
                return menulookup;
            }
        }


        /// <summary>
        /// Activates the specified theme.
        /// </summary>
        /// <param name="themeName">Name of the theme.</param>
        public static void Activate(string themeName)
        {
            var newTheme = Theme.Repository.First(Query<Theme>.New().Where(t => t.Name).Is(themeName));
            if (newTheme == null) throw new ArgumentException("Unknown theme : " + themeName);
            
            Activate(newTheme);
        }


        /// <summary>
        /// Activates the theme specified by id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Activate(int id)
        {
            var newTheme = Theme.Lookup[id];
            if (newTheme == null) throw new ArgumentException("Unknown theme by id: " + id);
            
            Activate(newTheme);
        }


        /// <summary>
        /// Activates the specified new theme.
        /// </summary>
        /// <param name="newTheme">The new theme.</param>
        public static void Activate(Theme newTheme)
        {            
            Theme last = GetActiveTheme();
            
            // Update current.
            newTheme.IsActive = true;
            newTheme.Update();

            // Update the last one.
            last.IsActive = false;
            last.Update();
        }


        /// <summary>
        /// Gets the active theme.
        /// </summary>
        /// <returns></returns>
        public static Theme GetActiveTheme()
        {
            Theme theme = Repository.First(Query<Theme>.New().Where(t => t.IsActive).Is(true));
            return theme;
        }


        /// <summary>
        /// Gets the current theme from the HttpContext.Items if present, otherwise from the database.
        /// </summary>
        public static Theme Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return GetActiveTheme();

                var theme = HttpContext.Current.Items["theme"] as Theme;
                if (theme == null)
                    return GetActiveTheme();

                return theme;
            }
        }
    }
}
