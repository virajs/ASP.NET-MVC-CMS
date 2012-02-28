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

using ComLib;
using ComLib.Web.Lib.Core;
using ComLib.Entities;
using ComLib.Extensions;
using ComLib.Caching;
using ComLib.Data;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;


namespace ComLib.Web.Modules.Widgets
{
    
    /// <summary>
    /// WidgetInstance entity.
    /// </summary>
    public partial class WidgetInstance : ActiveRecordBaseEntity<WidgetInstance>, IEntity, IEntitySortable, IEntityActivatable, IEntityClonable
    {
        public const string CacheKeyForLookup = "widgetinstance_lookup";
        

        /// <summary>
        /// Reference to the definition for this widget.
        /// </summary>
        public Widget Definition { get; set; }

        
        /// <summary>
        /// Whether or not this can render html itself or uses a control to do it.
        /// </summary>
        public virtual bool IsSelfRenderable { get { return false; } }


        /// <summary>
        /// Move this instance to a new zone and/or change sort index.
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="newSortIndex"></param>
        /// <param name="changeZone"></param>
        /// <param name="changeSortIndex"></param>
        public void Move(string newZone, int newSortIndex, bool changeZone, bool changeSortIndex)
        {
            if (changeZone && !string.IsNullOrEmpty(newZone))
                Zone = newZone;

            if (changeSortIndex)
                SortIndex = newSortIndex;

            Update();
        }


        /// <summary>
        /// Activate this instance.
        /// </summary>
        public virtual void Activate()
        {
            IsActive = true;
            Update();
        }


        /// <summary>
        /// Deactivate this widget.
        /// </summary>
        public virtual void Deactivate()
        {
            IsActive = false;
            Update();
        }


        /// <summary>
        /// Tag to embed the widget into an htmlpage.
        /// The html page parses out the $[name attributes/] and renders the widget.
        /// </summary>
        public virtual string EmbedTag
        {
            get
            {
                if (!this.IsSelfRenderable)
                    return "N/A";
                return string.Format(@"$[widget id=""{0}"" /]", this.Id);
            }
        }


        /// <summary>
        /// Renders this instance as Html.
        /// </summary>
        /// <returns></returns>
        public virtual string Render()
        {
            return "<div></div>";
        }


        /// <summary>
        /// Load the state into the properties.
        /// </summary>
        public virtual void LoadState()
        {
            if (this.IsPersistant())
            {
                if (this.Definition == null)
                    this.Definition = Widget.Lookup[this.DefName];
            
                var def = this.Definition;            
                SimpleStateHelper.LoadState(this, StateData, def.IncludeProperties, def.ExcludeProperties, def.StringClobProperties);
            }
        }


        /// <summary>
        /// Saves all the extended settings into a simple multi-line key/value pair.
        /// This also handles the case for stringclobs.
        /// </summary>
        public virtual void SaveState()
        {
            if (this.Definition == null)
                this.Definition = Widget.Lookup[this.DefName];

            var def = this.Definition;            
            StateData = SimpleStateHelper.CreateState(this, def.IncludeProperties, def.ExcludeProperties, def.StringClobProperties);
        }


        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                WidgetInstance entity = (WidgetInstance)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Header, false, true, true, 1, 150, results, "Header");
                Validation.IsStringLengthMatch(entity.Zone, false, true, true, 1, 50, results, "Zone");
                Validation.IsStringLengthMatch(entity.Roles, true, false, true, -1, 50, results, "Roles");

                Validation.ValidateObject(this, results);
                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Lookup by both id
        /// </summary>
        public static IDictionary<int, WidgetInstance> Lookup
        {
            get
            {
                // Cached lookup of menu items by id and name.
                var lookup = Repository.ToLookUp();
                return lookup;
            }
        }


        #region Life-Cycle Events
        /// <summary>
        /// Called when [before save].
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool  OnBeforeSave(object ctx)
        {
            if (this.Definition == null)
            {
                this.Definition = Widget.Lookup[this.DefName];
            }
            this.Zone = this.Zone.Trim();
            SaveState();
            SetUpWidgetDefId(this.DefName, this);
            return true;
        }
        #endregion


        #region Public static Helper Methods
        /// <summary>
        /// Create a new widget instance and associate it w/ its widget.
        /// </summary>
        /// <param name="widgetTitle"></param>
        /// <param name="instance"></param>
        public static void Create(IList<WidgetInstance> instances, bool checkIfPresent)
        {
            foreach (var instance in instances)
                Create(instance.DefName, instance, checkIfPresent);
        }


        /// <summary>
        /// Render utility method.
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public string TryRender(Func<string> renderer, string errorMessage = "")
        {   
            string html = string.Empty;
            if (string.IsNullOrEmpty(errorMessage))
                errorMessage = string.Format("Unable to render widget: {0}, {1}, {2}", this.Id, this.DefName, this.GetType().Name);

            try
            {
                string cachekey = "W_" + this.DefName + "_" + this.Id;
                bool cacheEnabled = false;
                int cacheTime = 10;
                if (this is IEntityCachable)
                {
                    IEntityCachable wc = (IEntityCachable)this;
                    cacheEnabled = wc.CacheEnabled;
                    cacheTime = wc.CacheTime;
                }
                html = Cacher.Get<string>(cachekey, cacheEnabled, cacheTime.Time(), renderer);
            }
            catch (Exception ex)
            {   
                Logging.Logger.Error(errorMessage, ex);
                html = errorMessage;
            }
            return html;
        }


        /// <summary>
        /// Create a new widget instance and associate it w/ its widget.
        /// </summary>
        /// <param name="widgetTitle"></param>
        /// <param name="instance"></param>
        public static void Create(string widgetTitle, WidgetInstance instance, bool checkIfPresent)
        {
            SetUpWidgetDefId(widgetTitle, instance);
            if (!checkIfPresent)
                Create(instance);
            else
            {
                Create(new List<WidgetInstance>() { instance }, i => i.Header, i => i.WidgetId);
            }
        }


        /// <summary>
        /// Sets up widget id.
        /// </summary>
        /// <param name="definitionName">Name of the definition.</param>
        /// <param name="instance">The instance.</param>
        private static void SetUpWidgetDefId(string definitionName, WidgetInstance instance)
        {
            var lookup = Widget.Lookup;
            if (!lookup.ContainsKey(definitionName))
                throw new ArgumentException("Unknown widget with title : " + definitionName);

            // Set the widget id.
            instance.WidgetId = lookup[definitionName].Id;
        }
        #endregion
    }



    /// <summary>
    /// Widget extensions.
    /// </summary>
    public partial class Widget : ActiveRecordBaseEntity<Widget>, IEntity
    {
        /// <summary>
        /// Names of all the distinct widgets.
        /// </summary>
        public static IList<string> Names
        {
            get
            {
                var all = Cacher.Get<IList<Widget>>("widgets_all", 30, () => GetAll());
                var names = Cacher.Get<IList<string>>("widgets_names", 30, () =>
                {
                    IList<string> widgetnames = new List<string>();
                    foreach (var widget in all)
                        widgetnames.Add(widget.Name);

                    return widgetnames;
                });
                return names;
            }
        }


        /// <summary>
        /// Lookup by both id and string Title
        /// </summary>
        public static LookupMulti<Widget> Lookup
        {
            get
            {
                LookupMulti<Widget> widgetlookup = null;
                if(Cacher.Contains("widget_lookupbyname"))
                {
                    widgetlookup = Cacher.Get<LookupMulti<Widget>>("widget_lookupbyname");

                    if (widgetlookup != null && (widgetlookup.Lookup1.Count != 0 && widgetlookup.Lookup2.Count != 0))                        
                        return widgetlookup;
                }

                // Still empty
                Cacher.Remove("widget_lookupbyname");
                    
                // Cached lookup of items by id and name.
                widgetlookup = Cacher.Get<LookupMulti<Widget>>("widget_lookupbyname", 
                         true, ((int)30).Seconds(), () => Widget.Repository.ToLookUpMulti<string>(m => m.Name));

                
                return widgetlookup;
            }
        }


        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Widget entity = (Widget)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Name, true, false, true, -1, 150, results, "Name");
                Validation.IsStringLengthMatch(entity.FullTypeName, true, false, true, -1, 150, results, "FullTypeName");
                Validation.IsStringLengthMatch(entity.Path, true, false, true, -1, 150, results, "Path");
                Validation.IsStringLengthMatch(entity.Version, true, false, true, -1, 20, results, "Version");
                Validation.IsStringLengthMatch(entity.Author, true, false, true, -1, 20, results, "Author");
                Validation.IsStringRegExMatch(entity.AuthorUrl, true, RegexPatterns.Url, results, "AuthorUrl");
                Validation.IsStringRegExMatch(entity.Email, false, RegexPatterns.Email, results, "Email");
                Validation.IsStringRegExMatch(entity.Url, false, RegexPatterns.Url, results, "Url");
                Validation.IsStringLengthMatch(entity.IncludeProperties, false, false, false, -1, -1, results, "IncludeProperties");
                Validation.IsStringLengthMatch(entity.ExcludeProperties, false, false, false, -1, -1, results, "ExcludeProperties");
                Validation.IsStringLengthMatch(entity.StringClobProperties, false, false, false, -1, -1, results, "StringClobProperties");
                Validation.IsNumericWithinRange(entity.SortIndex, false, false, -1, -1, results, "SortIndex");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }
    }



    public partial class WidgetInstanceRowMapper
    {
        /// <summary>
        /// Initialize the factory method to create the appropriate derived widget class.
        /// </summary>
        public WidgetInstanceRowMapper()
        {
            _entityFactoryMethod = (ctx) =>
            {
                IDataReader reader = ctx as IDataReader;
                var defname = reader["DefName"] == DBNull.Value ? string.Empty : reader["DefName"].ToString();
                WidgetInstance entity = string.IsNullOrEmpty(defname) ? WidgetInstance.New() : WidgetHelper.Create(defname);
                return entity;
            };
        }
    }
}
