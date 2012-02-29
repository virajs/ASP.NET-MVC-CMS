using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Models;
using ComLib.CodeGeneration;


namespace ComLib.Web.Modules
{
    public class WebModels
    {
        private ConnectionInfo _conn;


        public WebModels() : this(ConnectionInfo.Default)
        {            
        }


        public WebModels(ConnectionInfo conn)
        {
            _conn = conn;
        }
        

        public ModelContext GetModelContext()
        {
            return new ModelContext(GetModelContainer());
        }


        /// <summary>
        /// Gets the model container.
        /// </summary>
        /// <returns></returns>
        public ModelContainer GetModelContainer()
        {
            var location2 = @"f:\business\CMS\Comlib.mvc\";
            var comlib2 = @"f:\business\CommonLibrary.NET\CommonLibraryNet_LATEST";

            var location = @"C:\Dev\business\cms\Commonlibrary.CMS_CMS\";
            var comlib = @"C:\Dev\business\CommonLibrary.NET\CommonLibraryNet_LATEST";

            // Settings for the Code model builders.
            ModelBuilderSettings settings = new ModelBuilderSettings()
            {               
                ModelCodeLocation = location + @"\src\lib\CommonLibrary.Web.Modules\Src\_Models\",
                ModelInstallLocation = location + @"\Install\",
                ModelCodeLocationTemplate = comlib + @"\src\Lib\CommonLibrary.NET\CodeGen\Templates\Default",
                ModelDbStoredProcTemplates = comlib + @"\src\Lib\CommonLibrary.NET\CodeGen\Templates\DefaultSql",
                DbAction_Create = DbCreateType.DropCreate,
                Connection = _conn,
                AssemblyName = "CommonLibrary.Extensions"
            };

            ModelContainer models = new ModelContainer()
            {
                Settings = settings,
                ExtendedSettings = new Dictionary<string, object>() { },

                // Model definition.
                AllModels = new List<Model>()
                {
                    new Model("ModelBase")
                            .AddProperty<int>( "Id").Required.Key
                            .AddProperty<DateTime>( "CreateDate").Required
                            .AddProperty<DateTime>( "UpdateDate").Required
                            .AddProperty<string>( "CreateUser").Required.MaxLength("20")
                            .AddProperty<string>( "UpdateUser").Required.MaxLength("20")
                            .AddProperty<string>( "UpdateComment").Required.MaxLength("150"),
                    
                    new Model("Address")
                            .AddProperty<string>("Street").Range("-1", "60")
                            .AddProperty<string>("City").Range("-1", "30")
                            .AddProperty<string>("State").Range("-1", "20")
                            .AddProperty<string>("Country").Range("-1", "20")
                            .AddProperty<string>("Zip").Range("-1", "10")
                            .AddProperty<int>("CityId")
                            .AddProperty<int>("StateId")
                            .AddProperty<int>("CountryId")
                            .AddProperty<bool>("IsOnline").Mod,
                    
                    new Model("User")
                            .BuildTable("Users").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Users")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("UserName").Required.Range("3", "20").NoCode
                            .AddProperty<string>("UserNameLowered").Required.Range("3", "20").NoCode
                            .AddProperty<string>("Email").Required.Range("7", "50").NoCode
                            .AddProperty<string>("EmailLowered").Required.Range("7", "50").NoCode
                            .AddProperty<string>("Password").Required.Range("5", "100").NoCode
                            .AddProperty<string>("Roles").Range("0", "50")
                            .AddProperty<string>("MobilePhone").Range("10", "20")
                            .AddProperty<string>("SecurityQuestion").MaxLength("150")
                            .AddProperty<string>("SecurityAnswer").MaxLength("150")
                            .AddProperty<string>("Comment").MaxLength("50")
                            .AddProperty<bool>("IsApproved")
                            .AddProperty<bool>("IsLockedOut")
                            .AddProperty<string>("LockOutReason").MaxLength("50")
                            .AddProperty<DateTime>("LastLoginDate").Required
                            .AddProperty<DateTime>("LastPasswordChangedDate").Required
                            .AddProperty<DateTime>("LastPasswordResetDate").Required
                            .AddProperty<DateTime>("LastLockOutDate").Required.Mod,

                    new Model("Log")                        
                            .BuildCode().BuildTable("Logs").BuildInstallSqlFile()                            
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Logs")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("Application").MaxLength("255")
                            .AddProperty<string>("Computer").MaxLength("255")
                            .AddProperty<int>("LogLevel")
                            .AddProperty<StringClob>("Exception").Range("-1", "-1")
                            .AddProperty<StringClob>("Message").Range("-1", "-1")
                            .AddProperty<string>("UserName").MaxLength("20").Mod,                            
                    
                    new Model("Config")                        
                            .BuildTable("Configs").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Configs")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("App").MaxLength("30")
                            .AddProperty<string>("Name").MaxLength("30")
                            .AddProperty<string>("Section").MaxLength("50")
                            .AddProperty<string>("Key").MaxLength("50")
                            .AddProperty<StringClob>("Val").Range("-1", "-1")
                            .AddProperty<string>("ValType").MaxLength("20").Mod,                            
                    
                    new Model("Page")
                            .BuildCode().BuildTable("Pages").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Pages")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "100")
                            .AddProperty<string>("Description").Range("-1", "100")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<int>("IsPublished")
                            .AddProperty<string>("Keywords").Required.Range("-1", "80")
                            .AddProperty<string>("Slug").Range("-1", "150")
                            .AddProperty<string>("AccessRoles").Range("-1", "50")
                            .AddProperty<int>("Parent")
                            .AddProperty<bool>("IsPublic")
                            .AddProperty<bool>("IsFrontPage").Mod,

                    new Model("Part")
                            .BuildCode().BuildTable("Parts").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Parts")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "100")
                            .AddProperty<string>("Description").Range("-1", "100")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<string>("AccessRoles").Range("-1", "50")
                            .AddProperty<string>("RefTag").Range("-1", "40")
                            .AddProperty<bool>("IsPublic").Mod,

                   new Model("Post")
                            .BuildCode().BuildTable("Posts").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Posts")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "150")
                            .AddProperty<string>("Description").Required.Range("-1", "200")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<DateTime>("PublishDate")
                            .AddProperty<bool>("IsPublished")  
                            .AddProperty<bool>("IsPublic")                          
                            .AddProperty<int>("CategoryId")
                            .AddProperty<string>("Author").NoCode.NotPersisted
                            .AddProperty<string>("Tags").Range("-1", "80")                          
                            .AddProperty<string>("Slug").Range("-1", "150")
                            .AddProperty<string>("RefKey").Range("-1", "20")
                            .AddProperty<bool>("IsFavorite")
                            .AddProperty<bool>("IsCommentEnabled")
                            .AddProperty<bool>("IsCommentModerated")
                            .AddProperty<bool>("IsRatable")
                            .AddProperty<int>("Year").GetterOnly.NoCode
                            .AddProperty<int>("Month").GetterOnly.NoCode
                            .AddProperty<int>("CommentCount")
                            .AddProperty<int>("ViewCount")
                            .AddProperty<int>( "AverageRating")
                            .AddProperty<int>( "TotalLiked")
                            .AddProperty<int>( "TotalDisLiked")
                            .AddProperty<bool>("HasMediaFiles")
                            .AddProperty<int>("TotalMediaFiles").Mod,

                    new Model("Event")
                            .BuildCode().BuildTable("Events").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Events")
                            .InheritsFrom("ModelBase")
                            .HasComposition("Address")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "150")
                            .AddProperty<string>("Description").Required.Range("-1", "200")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")
                            .AddProperty<DateTime>("PublishDate").NoCode.NotPersisted
                            .AddProperty<bool>("IsPublished")
                            .AddProperty<bool>("IsPublic")
                            .AddProperty<int>("CategoryId")
                            .AddProperty<DateTime>("StartDate").Required
                            .AddProperty<DateTime>("EndDate").Required
                            .AddProperty<int>("StartTime")
                            .AddProperty<int>("EndTime")
                            .AddProperty<int>("Year").GetterOnly.NoCode
                            .AddProperty<int>("Month").GetterOnly.NoCode
                            .AddProperty<int>("Day").GetterOnly.NoCode
                            .AddProperty<bool>("IsFeatured")
                            .AddProperty<bool>("IsTravelRequired")
                            .AddProperty<bool>("IsConference")
                            .AddProperty<bool>("IsAllTimes").NoCode
                            .AddProperty<double>("Cost")
                            .AddProperty<int>("Skill")
                            .AddProperty<int>("Seats")
                            .AddProperty<bool>("IsAgeApplicable")
                            .AddProperty<int>("AgeFrom")
                            .AddProperty<int>("AgeTo")
                            .AddProperty<string>("Email").Range("-1", "30").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("Phone").Range("-1", "14").RegExConst("RegexPatterns.PhoneUS")
                            .AddProperty<string>("Url").Range("-1", "150").RegExConst("RegexPatterns.Url")
                            .AddProperty<string>("Tags").Range("-1", "80")
                            .AddProperty<string>("RefKey").Range("-1", "20")
                            .AddProperty<int>( "AverageRating")
                            .AddProperty<int>( "TotalLiked")
                            .AddProperty<int>( "TotalDisLiked")
                            .AddProperty<int>( "TotalBookMarked")
                            .AddProperty<int>( "TotalAbuseReports")                            
                            .AddProperty<bool>("HasMediaFiles")
                            .AddProperty<int>("TotalMediaFiles").Mod,

                    new Model("Link")
                            .BuildCode().BuildTable("Links").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Links")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Name").Required.Range("1", "100")
                            .AddProperty<string>("Group").Required.Range("1", "100")
                            .AddProperty<string>("Url").Range("-1", "150").RegEx("RegexPatterns.Url")
                            .AddProperty<string>("Description").Range("-1", "50")
                            .AddProperty<int>("SortIndex").Required.Range("-1", "100000").Mod,

                    new Model("Tag")
                            .BuildCode().BuildTable("Tags").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Tags")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<int>("GroupId")
                            .AddProperty<string>("Name").Range("1", "20")
                            .AddProperty<int>("RefId").Mod,
                    
                    new Model("Feedback")
                            .BuildCode().BuildTable("Feedbacks").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Feedbacks")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Range("-1", "150")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")                            
                            .AddProperty<string>("Group").Required.Range("1", "50")
                            .AddProperty<string>("Name").Range("-1", "20")
                            .AddProperty<string>("Email").Range("-1", "30").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("Url").Range("-1", "150").RegExConst("RegexPatterns.Url")
                            .AddProperty<bool>("IsApproved").Mod,

                    new Model("Faq")
                            .BuildCode().BuildTable("Faqs").BuildInstallSqlFile()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Faqs")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Range("-1", "150")
                            .AddProperty<StringClob>("Content").Required.Range("-1", "-1")                            
                            .AddProperty<string>("Group").Required.Range("1", "50").Mod,
                    
                    new Model("Profile")
                            .BuildCode().BuildTable("Profiles").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Profiles")
                            .InheritsFrom("ModelBase")                            
                            .HasComposition("Address")
                            .AddProperty<int>("UserId")
                            .AddProperty<string>("UserName").Range("-1", "20")
                            .AddProperty<StringClob>("About").Required.Range("-1", "-1") 
                            .AddProperty<string>("FirstName").Range("-1", "20")
                            .AddProperty<string>("LastName").MaxLength("20")   
                            .AddProperty<string>("Alias").MaxLength("50")
                            .AddProperty<bool>("IsFeatured")
                            .AddProperty<string>("Email").Range("-1", "50").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("WebSite").Range("-1", "150").RegExConst("RegexPatterns.Url")
                            .AddProperty<string>("ImageUrl").Range("-1", "150").RegExConst("RegexPatterns.Url").NoCode
                            .AddProperty<string>("AddressDisplayLevel").Range("-1", "10")
                            .AddProperty<int>("ImageRefId")
                            .AddProperty<bool>("EnableDisplayOfName")
                            .AddProperty<bool>("IsGravatarEnabled")
                            .AddProperty<bool>("IsAddressEnabled")
                            .AddProperty<bool>("EnableMessages")                            
                            .AddProperty<bool>("HasMediaFiles")
                            .AddProperty<int>("TotalMediaFiles").Mod,

                    new Model("Widget")
                            .BuildCode().BuildTable("Widgets").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Widgets")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("Name").Range("-1", "150")
                            .AddProperty<StringClob>("Description").Range("-1", "-1")
                            .AddProperty<string>("FullTypeName").Range("-1", "150")
                            .AddProperty<string>("Path").Range("-1", "150")
                            .AddProperty<string>("Version").MaxLength("20")
                            .AddProperty<string>("Author").MaxLength("20")
                            .AddProperty<string>("AuthorUrl").MaxLength("150").RegExConst("RegexPatterns.Url")
                            .AddProperty<string>("Email").Range("-1", "30").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("Url").Range("-1", "150").RegExConst("RegexPatterns.Url")
                            .AddProperty<StringClob>("IncludeProperties").Required.Range("-1", "-1") 
                            .AddProperty<StringClob>("ExcludeProperties").Required.Range("-1", "-1") 
                            .AddProperty<StringClob>("StringClobProperties").Required.Range("-1", "-1") 
                            .AddProperty<string>("PathToEditor").Range("-1", "150")
                            .AddProperty<string>("DeclaringType").Range("-1", "150")
                            .AddProperty<string>("DeclaringAssembly").Range("-1", "150")
                            .AddProperty<int>("SortIndex")
                            .AddProperty<bool>("IsCacheable")
                            .AddProperty<bool>("IsEditable").Mod,   
                    
                    // There should be a WidgetSettings class but for the sake of simplicity for this 1st version.
                    // the settings will be stored into an string "Args" field and parsed.
                    // Will handle the settings as a patch or something.
                    new Model("WidgetInstance")
                            .BuildCode().BuildTable("WidgetInstances").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Widgets")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<int>("WidgetId")                           
                            .AddProperty<string>("Header").Range("-1", "150")
                            .AddProperty<string>("Zone").MaxLength("50")
                            .AddProperty<string>("DefName").Range("1", "30")                            
                            .AddProperty<string>("Roles").MaxLength("50")                          
                            .AddProperty<StringClob>("StateData").Range("-1", "-1")
                            .AddProperty<int>("SortIndex")
                            .AddProperty<bool>("IsActive").Mod,
                    
                    new Model("MenuEntry")
                            .BuildCode().BuildTable("MenuEntrys").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.MenuEntrys")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Name").Required.Range("1", "50")
                            .AddProperty<string>("Url").Range("-1", "150")
                            .AddProperty<string>("Description").Range("-1", "50")
                            .AddProperty<string>("Roles").Range("-1", "50")
                            .AddProperty<string>("ParentItem").MaxLength("50")
                            .AddProperty<int>("RefId")
                            .AddProperty<bool>("IsRerouted")
                            .AddProperty<bool>("IsPublic").NoCode.GetterOnly
                            .AddProperty<int>("SortIndex").Required.Range("-1", "100000").Mod,

                    new Model("Theme")
                            .BuildCode().BuildTable("Themes").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Themes")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Name").Required.Range("1", "50")
                            .AddProperty<string>("Description").Required.Range("-1", "150")
                            .AddProperty<string>("Path").Range("-1", "150")
                            .AddProperty<string>("Layouts").MaxLength("100")
                            .AddProperty<string>("Zones").MaxLength("50")
                            .AddProperty<string>("SelectedLayout").MaxLength("30")
                            .AddProperty<bool>("IsActive")
                            .AddProperty<string>("Author").MaxLength("20")
                            .AddProperty<string>("Version").MaxLength("20")
                            .AddProperty<string>("Email").Range("-1", "30").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("Url").Range("-1", "150").RegExConst("RegexPatterns.Url")
                            .AddProperty<int>("SortIndex").Required.Range("-1", "100000").Mod,

                    new Model("Flag")
                            .BuildCode().BuildTable("Flags").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Flags")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Title").Required.Range("1", "150")
                            .AddProperty<int>("RefId")
                            .AddProperty<int>("FlagType")
                            .AddProperty<string>("Model").MaxLength("30")
                            .AddProperty<string>("Url").MaxLength("200")
                            .AddProperty<string>("FlaggedByUser").MaxLength("20")
                            .AddProperty<DateTime>("FlaggedDate").Mod,

                    new Model("Favorite")
                            .BuildCode().BuildTable("Favorites").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Favorites")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<int>("UserId")
                            .AddProperty<string>("Title").Required.Range("1", "150")
                            .AddProperty<int>("RefId")
                            .AddProperty<string>("Model").MaxLength("30")
                            .AddProperty<string>("Url").MaxLength("200").Mod,

                    new Model("Comment")
                            .BuildCode().BuildTable("Comments").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Comments")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<int>("GroupId")
                            .AddProperty<int>("RefId")
                            .AddProperty<string>("Title").Required.Range("1", "50")
                            .AddProperty<StringClob>("Content").Range("-1", "-1")
                            .AddProperty<int>("UserId")
                            .AddProperty<string>("Name").MaxLength("30")
                            .AddProperty<string>("Email").Required.MaxLength("150").RegExConst("RegexPatterns.Email")
                            .AddProperty<string>("Url").MaxLength("150").RegExConst("RegexPatterns.Url")
                            .AddProperty<string>("ImageUrl").MaxLength("150")
                            .AddProperty<bool>("IsGravatarEnabled")
                            .AddProperty<bool>("IsApproved")
                            .AddProperty<int>("Rating").Mod,

                    new Model("Resource")                        
                            .BuildCode().BuildTable("Resources").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Resources")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("AppName").MaxLength("30")
                            .AddProperty<string>("Language").MaxLength("20")
                            .AddProperty<string>("ResourceType").MaxLength("20")
                            .AddProperty<string>("Section").MaxLength("50")
                            .AddProperty<string>("Key").MaxLength("50")                            
                            .AddProperty<string>("ValType").MaxLength("20")
                            .AddProperty<string>("Name").MaxLength("80")
                            .AddProperty<string>("Description").MaxLength("200")
                            .AddProperty<string>("Example").MaxLength("50").Mod, 

                     new Model("OptionDef")                        
                            .BuildCode().BuildTable("OptionDefs").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.OptionDefs")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Section").MaxLength("50")
                            .AddProperty<string>("Key").MaxLength("50")
                            .AddProperty<string>("ValType").MaxLength("20")
                            .AddProperty<StringClob>("Values").Range("-1", "-1")
                            .AddProperty<string>("DefaultValue").MaxLength("200")
                            .AddProperty<string>("DisplayStyle").MaxLength("20")
                            .AddProperty<bool>("IsRequired")
                            .AddProperty<bool>("IsBasicType")
                            .AddProperty<int>("SortIndex").Mod,

                    new Model("MediaFile")
                            .BuildCode().BuildTable("MediaFiles").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Media")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Name").Required.Range("1", "50")
                            .AddProperty<string>("FullName").Range("1", "200").NoCode
                            .AddProperty<string>("DirectoryName").Required.Range("1", "25")
                            .AddProperty<string>("Extension").Required.Range("1", "5")
                            .AddProperty<string>("Title").Required.Range("1", "100")                            
                            .AddProperty<string>("Description").Required.Range("1", "200")
                            .AddProperty<DateTime>("LastWriteTime")
                            .AddProperty<int>("Length")
                            .AddProperty<Image>("Contents").Required.Range("-1", "-1").NoCode
                            .AddProperty<Image>("ContentsForThumbNail").Required.Range("-1", "-1")
                            .AddProperty<int>("Height")
                            .AddProperty<int>("Width")
                            .AddProperty<int>("SortIndex")
                            .AddProperty<int>("RefGroupId")
                            .AddProperty<int>("RefId")                       
                            .AddProperty<int>("RefImageId")
                            .AddProperty<int>("ParentId")
                            .AddProperty<int>("FileType")
                            .AddProperty<bool>("IsPublic")
                            .AddProperty<bool>("IsExternalFile")
                            .AddProperty<string>("ExternalFileSource").Range("-1", "20")
                            .AddProperty<bool>("HasThumbnail").Mod,

                    new Model("MediaFolder")
                            .BuildCode().BuildTable("MediaFolders").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Media")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Name").Required.Range("1", "50")
                            .AddProperty<string>("FullName").Required.Range("1", "200")
                            .AddProperty<string>("DirectoryName").Required.Range("1", "25")
                            .AddProperty<string>("Extension").Required.Range("1", "5")   
                            .AddProperty<string>("Description").Required.Range("1", "200")
                            .AddProperty<int>("Length")
                            .AddProperty<DateTime>("LastWriteTime")
                            .AddProperty<int>("SortIndex")
                            .AddProperty<int>("ParentId")
                            .AddProperty<int>("FileType")
                            .AddProperty<bool>("IsPublic")
                            .AddProperty<bool>("HasMediaFiles")
                            .AddProperty<int>("TotalMediaFiles")
                            .AddProperty<bool>("IsExternalFile")
                            .AddProperty<string>("ExternalFileSource").Range("-1", "20")
                            .AddProperty<bool>("HasThumbnail").Mod,

                    new Model("Category")
                            .BuildCode().BuildTable("Categorys").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.Web.Modules.Categorys")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("AppId")
                            .AddProperty<string>("Name").Required.Range("1", "25")
                            .AddProperty<string>("Description").Required.Range("1", "20")
                            .AddProperty<string>("Group").MaxLength("25")
                            .AddProperty<int>("SortIndex")
                            .AddProperty<int>("Count")
                            .AddProperty<int>("ParentId").Mod,


                    new Model("Country")
                            .BuildCode().BuildTable("Countrys").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.LocationSupport")
                            .InheritsFrom("ModelBase")
                            .AddProperty<string>("CountryCode").MaxLength("10")
                            .AddProperty<string>("Name").MaxLength("100")
                            .AddProperty<string>("Abbreviation").MaxLength("30")
                            .AddProperty<string>("AliasRefName").MaxLength("100")
                            .AddProperty<bool>("IsActive")
                            .AddProperty<bool>("IsAlias")
                            .AddProperty<int>("AliasRefId").Mod,

                    new Model("State")
                            .BuildCode().BuildTable("States").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.LocationSupport")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("CountryId")
                            .AddProperty<string>("CountryName").MaxLength("100")
                            .AddProperty<string>("Name").MaxLength("100")
                            .AddProperty<string>("Abbreviation").MaxLength("30")
                            .AddProperty<string>("AliasRefName").MaxLength("100")
                            .AddProperty<bool>("IsActive")
                            .AddProperty<bool>("IsAlias")
                            .AddProperty<int>("AliasRefId").Mod,

                    new Model("City")
                            .BuildCode().BuildTable("Citys").BuildInstallSqlFile().NoValidation()
                            .BuildActiveRecordEntity().NameSpaceIs("ComLib.LocationSupport")
                            .InheritsFrom("ModelBase")
                            .AddProperty<int>("StateId")
                            .AddProperty<string>("StateName").MaxLength("100")
                            .AddProperty<int>("ParentId")
                            .AddProperty<bool>("IsPopular")
                            .AddProperty<int>("CountryId")
                            .AddProperty<string>("CountryName").MaxLength("100")
                            .AddProperty<string>("Name").MaxLength("100")
                            .AddProperty<string>("Abbreviation").MaxLength("30")
                            .AddProperty<string>("AliasRefName").MaxLength("100")
                            .AddProperty<bool>("IsActive")
                            .AddProperty<bool>("IsAlias")
                            .AddProperty<int>("AliasRefId").Mod
                }
            };
            return models;
        }
    }
}
