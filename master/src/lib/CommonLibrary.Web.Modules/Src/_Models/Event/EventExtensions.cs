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
using System.Text.RegularExpressions;

using ComLib.Entities;
using ComLib.Extensions;
using ComLib.Patterns;
using ComLib.Data;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;
using ComLib.Types;
using ComLib.Feeds;
using ComLib.Web.Lib;
using ComLib.Web.Modules.Categorys;
using ComLib.Web.Modules.Tags;
using ComLib.Web.Modules.Media;
using ComLib.Web.Lib.Core;

namespace ComLib.Web.Modules.Events
{
    /// <summary>
    /// Settings specific for blog posts.
    /// </summary>
    public class EventSettings : EntitySettings<Event>
    {
        /// <summary>
        /// Set default values.
        /// </summary>
        public EventSettings()
        {
            CacheTime = 300;
            CacheOnArchives = true;
            CacheOnRecent = true;
            IndexPageSize = 15;
            EnableFreeformLocations = false;
            DescriptionLength = 180;
        }


        public bool CacheOnArchives { get; set; }
        public bool CacheOnRecent { get; set; }
        public int CacheTime { get; set; }
        public int IndexPageSize { get; set; }
        public bool EnableFreeformLocations { get; set; }
        public int DescriptionLength { get; set; }
    }


    
    public class CostType 
    { 
        public const int DoNotKnow = -1;
        public const int Varies = -2;
        public const int Free = 0;
        public const int HasFee = 1 ;
    }



    public enum SkillOptions
    {
        All = 0,
        Beginner = 1,
        Intermediate = 2,
        Advanced = 3
    }


    public enum AgeOptions
    {
        All = 0,
        Babies = 1,
        Kids = 2,
        Teens = 3,
        YoungAdults = 4,
        Adults = 5,
        Seniors = 6
    }


    /// <summary>
    /// Event entity.
    /// </summary>
    public partial class Event : ActiveRecordBaseEntity<Event>, IEntity, IPublishable, IEntityClonable, IEntityMediaSupport, IEntityActivatable
    {        
        /// <summary>
        /// Singleton settings.
        /// </summary>
        private static EventSettings _settings = new EventSettings();
        private static readonly string[] _skillOptions = new string[] 
                { SkillOptions.All.ToString(), 
                  SkillOptions.Beginner.ToString(), 
                  SkillOptions.Intermediate.ToString(), 
                  SkillOptions.Advanced.ToString() };

        private static readonly string[] _ageOptions = new string[] 
                { AgeOptions.All.ToString(), 
                  AgeOptions.Babies.ToString(), 
                  AgeOptions.Kids.ToString(), 
                  AgeOptions.Teens.ToString(), 
                  AgeOptions.YoungAdults.ToString(), 
                  AgeOptions.Adults.ToString(), 
                  AgeOptions.Seniors.ToString() };

        public const int SeatsUnlimited = -1;

        /// <summary>
        /// Settings for this entity.
        /// </summary>
        public static new EventSettings Settings
        {
            get { return _settings; }
        }

        
        /// <summary>
        /// Get all the possible skill options.
        /// </summary>
        public static string[] SelectionsForSkill
        {
            get { return _skillOptions; }
        }


        /// <summary>
        /// Get all the possible skill options.
        /// </summary>
        public static string[] SelectionsForAge
        {
            get { return _ageOptions; }
        }

        /// <summary>
        /// Initialize.
        /// </summary>
        public Event()
        {
            _address = new Address();
        }


        #region Properties
        /// <summary>
        /// Gets the publish date.
        /// Used for RSS / Feed generation.
        /// </summary>
        /// <value>The publish date.</value>
        public DateTime PublishDate { get { return UpdateDate; } }


        /// <summary>
        /// Url for the slug. e.g. /my-latest-blog-post-with-different-url-than-default-title-48
        /// </summary>
        public string SlugUrl
        {
            get
            {                
                return ComLib.Web.UrlSeoUtils.BuildValidUrl(Title) + "-" + Id;
            }
        }
        

        /// <summary>
        /// Year that the event was published.
        /// </summary>
        public int Year { get { return PublishDate.Year; } }


        /// <summary>
        /// Month that the event was published.
        /// </summary>
        public int Month { get { return PublishDate.Month; } }


        /// <summary>
        /// Day that the event was published.
        /// </summary>
        public int Day { get { return PublishDate.Day; } }


        /// <summary>
        /// Activate.
        /// </summary>
        public bool IsActive { get { return IsPublished; } set { IsPublished = value; } }
        #endregion


        #region IPublishable
        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>The author.</value>
        public string Author { get { return UpdateUser; } }


        /// <summary>
        /// Gets the relative link.
        /// </summary>
        /// <value>The relative link.</value>
        public string UrlRelative
        {
            get
            {
                return SlugUrl;
            }
        }


        /// <summary>
        /// Gets the absolute link.
        /// </summary>
        /// <value>The relative link.</value>
        public string UrlAbsolute
        {
            get
            {
                return "/" + SlugUrl;
            }
        }


        /// <summary>
        /// Gets the GUID.
        /// </summary>
        /// <value>The GUID.</value>
        public string GuidId
        {
            get { return UrlAbsolute; }
        }
        #endregion


        #region Validation
        /// <summary>
        /// Get the validator for validating this entity.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                IValidationResults results = validationEvent.Results;
                int initialErrorCount = results.Count;
                Event entity = (Event)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Title, false, true, true, 1, 150, results, "Title");
                Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, _settings.DescriptionLength, results, "Description");
                Validation.IsStringLengthMatch(entity.Content, false, true, false, 1, -1, results, "Content");
                Validation.IsStringRegExMatch(entity.Email, true, RegexPatterns.Email, results, "Email");
                Validation.IsStringRegExMatch(entity.Phone, true, RegexPatterns.PhoneUS, results, "Phone");
                Validation.IsStringRegExMatch(entity.Url, true, RegexPatterns.Url, results, "Url");
                Validation.IsStringLengthMatch(entity.Tags, true, false, true, -1, 80, results, "Tags");
                
                // Check the start date.
                if (!IsPersistant())
                {
                    Validation.IsDateWithinRange(entity.StartDate, true, true, DateTime.Today, DateTime.Today.AddDays(180), results, "StartDate");                    
                }

                // Check the end date is greater.
                Validation.IsDateWithinRange(entity.EndDate, true, true, entity.StartDate, entity.StartDate.AddDays(180), results, "EndDate");                

                // Check the time.
                Validation.IsNumericWithinRange(entity.StartTime, true, true, 0, 2400, results, "StartTime");
                if(entity.StartTime > 0 && entity.EndTime > 0 )
                    Validation.IsNumericWithinRange(entity.EndTime, true, true, StartTime, 2400, results, "EndTime");

                if (!Settings.EnableFreeformLocations && !IsOnline)
                {
                    IValidator locationValidator = new LocationValidator(this.Address, false);
                    locationValidator.Validate(results);
                }
                return initialErrorCount == results.Count;
            });
            return val;
        }
        #endregion


        #region Import/Export Setters
        /// <summary>
        /// Whether or not this event takes place online.
        /// </summary>
        public bool IsOnline
        {
            get { return _address.IsOnline; }
            set { _address.IsOnline = value; }
        }


        /// <summary>
        /// Get / set the category.
        /// </summary>
        public string CategoryName { get; set; }


        /// <summary>
        /// Get / set the category.
        /// </summary>
        public Category Category { get; set; }


        /// <summary>
        /// Used for reflection based import. Set the start/end times as a string time range. e.g. 11:30am - 2:20pm
        /// </summary>
        public string Times
        {
            get
            {
                if(IsAllTimes) return "All Times";

                if (EndTime <= 0)
                    return StartTime.TimeWithSuffix();

                return StartTime.TimeWithSuffix() + " - " + EndTime.TimeWithSuffix();
            }
            set
            {
                string val = value;
                if (string.IsNullOrEmpty(val)) return;
                val = val.Trim().ToLower();

                if (val.IsNotApplicableValue())
                {
                    StartTime = 0;
                    EndTime = 0;
                    return;
                }
                TimeParseResult result = TimeHelper.ParseStartEndTimes(val);
                if (result.IsValid)
                {
                    StartTime = result.Start.ToMilitaryInt();
                    if (result.End != TimeSpan.MaxValue && result.End != TimeSpan.MinValue)
                        EndTime = result.End.ToMilitaryInt();
                }
                else Errors.Add("Start/End times are incorrect");
            }
        }


        private bool _isAllTimes;
        /// <summary>
        /// Whether or not the time for class is all/various times.
        /// </summary>
        public bool IsAllTimes
        {
            get
            {
                if (StartTime <= 0 && EndTime <= 0)
                    return true;

                return _isAllTimes;
            }
            set { _isAllTimes = value; }
        }


        /// <summary>
        /// Sets the starts.
        /// </summary>
        /// <value>The starts.</value>
        public string Starts
        {
            set
            {
                if (string.IsNullOrEmpty(value) || value.IsNotApplicableValue())
                {
                    StartTime = 0;
                    return;
                }
                var result = TimeHelper.Parse(value);
                if (result.Success)
                    StartTime = result.Item.ToMilitaryInt();
                else
                    Errors.Add("Start Time", "Incorrect format");
            }
            get
            {
                if (IsAllTimes) return "All Times";

                return StartTime.TimeWithSuffix();
            }
        }


        /// <summary>
        /// Sets the Ends.
        /// </summary>
        /// <value>The Ends.</value>
        public string Ends
        {
            set
            {
                if (string.IsNullOrEmpty(value) || value.IsNotApplicableValue())
                {
                    EndTime = 0;
                    return;
                }
                var result = TimeHelper.Parse(value);
                if (result.Success)
                    EndTime = result.Item.ToMilitaryInt();
                else
                    Errors.Add("End Time", "Incorrect format");
            }
            get
            {
                return EndTime.TimeWithSuffix();
            }
        }


        private string AgePatternAndOver = @"^(?<age>[0-9]*)\s*and\s*over$";
        private string AgePatternAndUnder = @"^(?<age>[0-9]*)\s*and\s*under$";
        private string AgePatternTo = @"^(?<agefrom>[0-9]*)\s*to\s*(?<ageto>[0-9]*)$";
        private int MaxAge = 100;

        /// <summary>
        /// Get / set the age range.
        /// </summary>
        public string Ages
        {
            set
            {                
                string val = value;
                if (string.IsNullOrWhiteSpace(val))
                {
                    SetAgeToNotApplicable();
                    return;
                }
                val = val.ToLower().Trim();
                
                if (string.IsNullOrEmpty(val) || val == "any" || val == "all" || val == "everyone" || val == "anyone")
                {
                    SetAgeToNotApplicable();
                    return;
                }
                if ( val == "babies")           { SetAgeRange(0, 4);    return; }
                else if (val == "kids")         { SetAgeRange(5, 12);   return; }
                else if (val == "teens")        { SetAgeRange(13, 17);  return; }
                else if (val == "young adults") { SetAgeRange(18, 22);  return; }
                else if (val == "youngadults" ) { SetAgeRange(18, 22);  return; }
                else if (val == "adults")       { SetAgeRange(23, 64);  return; }
                else if (val == "seniors")      { SetAgeRange(65, 100); return; }
                
                val = val.Replace("up", "over");
                val = val.Replace("below", "under");
                val = val.Replace("-", "to");

                // Handle <age> "and over".
                Match match = Regex.Match(val, AgePatternAndOver);
                if (match.Success)
                {
                    IsAgeApplicable = true;
                    AgeFrom = Convert.ToInt32(match.Groups["age"].Value);
                    AgeTo = MaxAge;
                    return;
                }

                // Handle <age> "and under".
                match = Regex.Match(val, AgePatternAndUnder);
                if (match.Success)
                {
                    IsAgeApplicable = true;
                    AgeFrom = 0;
                    AgeTo = Convert.ToInt32(match.Groups["age"].Value);
                    return;
                }

                // Handle <age1> to <age2>
                match = Regex.Match(val, AgePatternTo);
                if (match.Success)
                {
                    IsAgeApplicable = true;
                    AgeFrom = Convert.ToInt32(match.Groups["agefrom"].Value);
                    AgeTo = Convert.ToInt32(match.Groups["ageto"].Value);
                    return;
                }
            }
            get
            {
                if (!IsAgeApplicable) return "All";
                int ageTo = AgeTo;
                int ageFrom = AgeFrom;
                if (ageFrom == 0 && ageTo == 4) return "babies";
                if (ageFrom == 5 && ageTo == 12) return "kids";
                if (ageFrom == 13 && ageTo == 17) return "teens";
                if (ageFrom == 18 && ageTo == 22) return "youngadults";
                if (ageFrom == 23 && ageTo == 64) return "adults";
                if (ageFrom == 65 && ageTo == MaxAge) return "seniors";

                if (ageTo == MaxAge)
                    return AgeFrom + " and over";
                if (ageFrom == 0)
                    return ageTo + " and under";

                return ageFrom + " to " + ageTo;
            }
        }


        /// <summary>
        /// Used for reflection based Import.
        /// </summary>
        public string Costs
        {
            set
            {
                string val = value;
                if (string.IsNullOrEmpty(val))
                {
                    Cost = CostType.DoNotKnow;
                    return;
                }
                val = val.Trim().ToLower();
                if (val.StartsWith("$") && val.Length > 1)
                    val = val.Substring(1);

                if (val == "do not know" || val == "don't know" || val == "unknown" )
                {
                    Cost = CostType.DoNotKnow;
                    return;
                }
                if (val == "varies" || val == "depends" || val == "differs")
                {
                    Cost = CostType.Varies;
                    return;
                }
                if( val == "zero" || val == "free" || val == "nothing" ||
                    val == "none" || val == "0" || val == "na" || val == "n/a" || val == "n.a" || val == "n.a." )
                {
                    Cost = CostType.Free;
                    return;
                }
                double finalcost = 0;
                if (!double.TryParse(val, out finalcost))
                {
                    this.Errors.Add("Costs", "Value supplied for cost is not a number : " + val);
                    return;
                }
                Cost = StringExtensions.ToDouble(val);
            }
            get
            {
                if (Cost == CostType.DoNotKnow) return "Don't know";
                if (Cost == CostType.Varies) return "Varies";
                if (Cost == CostType.Free) return "Free";
                return "$" + Cost;
            }
        }


        /// <summary>
        /// Gets/Sets the skill using a text value.
        /// </summary>
        /// <value>The skill text.</value>
        public string SkillText
        {
            set
            {
                string val = value;
                if (string.IsNullOrEmpty(val))
                {
                    Skill = (int)SkillOptions.All;
                    return;
                }
                val = val.Trim().ToLower();
                if (val.Length == 0) return;

                if (val == "any" || val == "all")
                {
                    Skill = (int)SkillOptions.All;
                    return;                        
                }
                if (val == "beginner" || val == "novice" || val == "newbie")
                {
                    Skill = (int)SkillOptions.Beginner;
                    return;
                }
                if (val == "intermediate" || val == "middle" || val == "medium")
                {
                    Skill = (int)SkillOptions.Intermediate;
                    return;
                }
                if (val == "advanced" || val == "expert" || val == "hard")
                {
                    Skill = (int)SkillOptions.Advanced;
                    return;
                }
                this.Errors.Add("SkillText", "Unknown skill supplied : " + val);                
            }
            get
            {
                if (Skill == 0) return "All";
                if (Skill == 1) return "Beginner";
                if (Skill == 2) return "Intermediate";
                if (Skill == 3) return "Advanced";
                return "All";
            }
        }


        /// <summary>
        /// Get / Set the seating values using string value.
        /// e.g. "unlimited | na | n/a | 25 etc .
        /// </summary>
        public string Seating
        {
            get
            {
                if (Seats == SeatsUnlimited) return "N/A";

                return Seats.ToString();
            }
            set
            {
                string val = value;
                if (string.IsNullOrEmpty(val))
                    return;
                val = val.Trim().ToLower();
                if (val == "unlimited" || val == "na" || val == "n.a." || val == "n/a" || val == "n\\a" || val == "n.a")
                {
                    Seats = SeatsUnlimited;
                    return;
                }
                int numberSeats = 0;
                if (Int32.TryParse(val, out numberSeats))
                {
                    Seats = numberSeats;
                }
                else
                {
                    Errors.Add("Seating", "Invalid value for seating : " + value);
                }
            }
        }


        /// <summary>
        /// Gets/sets the address using a single line of text.
        /// </summary>
        public string Location
        {
            get { return _address == null ? string.Empty : _address.FullAddress; }
            set 
            {
                if (_address == null) 
                    _address = new Address();

                _address.FullAddress = value;
            }
        }
        #endregion


        #region Life-Cycle Events
        /// <summary>
        /// Handle life-cycle event.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            if (StartDate == DateTime.MinValue)
                StartDate = DateTime.Now;
            if (EndDate == DateTime.MinValue || EndDate == DateTime.MaxValue)
                EndDate = DateTime.Now;

            this.Description = this.Content.Truncate(_settings.DescriptionLength);
            string tagsfinal = Tags;
            if (!string.IsNullOrEmpty(tagsfinal))
            {
                tagsfinal = tagsfinal.Replace("&", " ");
                tagsfinal = tagsfinal.Replace(",", " ");
                Tags = tagsfinal;
            }
            ToDo.Implement(ToDo.Priority.Normal, "Kishore", "IsPublished not supported for events.", () => { this.IsPublished = true; this.IsPublic = true; });

            // Form fields might set the country values etc. after the isonline is mapped.
            if (IsOnline)
                _address.IsOnline = true;

            // Set the category id.
            if(!string.IsNullOrEmpty(CategoryName))
            {
                var lookup = Categorys.Category.LookupFor<Event>();
                this.CategoryId = lookup != null && lookup.ContainsKey(CategoryName) ? lookup[CategoryName].Id : 0;
            }
            if (!Settings.EnableFreeformLocations && !IsOnline)
            {
                // Apply the country/state ids.
                IList<string> errors = new List<string>();
                LocationHelper.ApplyCountry(this.Address, ComLib.LocationSupport.Location.CountriesLookup, errors);
                LocationHelper.ApplyState(this.Address, ComLib.LocationSupport.Location.StatesLookup, errors);
                LocationHelper.ApplyCity(this.Address, ComLib.LocationSupport.Location.CitiesLookup, ComLib.LocationSupport.Location.StatesLookup, ComLib.LocationSupport.Location.CountriesLookup);
                foreach (var error in errors) this.Errors.Add(error);
            }
            return true;
        }


        /// <summary>
        /// After this is created/updated, handle the tags.
        /// </summary>
        public override void OnAfterSave()
        {
            // Queue up the processing of tags.
            int groupId = ModuleMap.Instance.GetId(typeof(Event));
            Queue.Queues.Enqueue<TagsEntry>(new TagsEntry() { GroupId = groupId, RefId = Id, Tags = Tags });
        }
        #endregion


        private void SetAgeToNotApplicable()
        {
            IsAgeApplicable = false;
            AgeFrom = 0;
            AgeTo = 0;
        }


        private void SetAgeRange(int from, int to)
        {
            IsAgeApplicable = true;
            AgeFrom = from;
            AgeTo = to;
        }
    }
}
