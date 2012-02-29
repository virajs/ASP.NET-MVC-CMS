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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

using ComLib.Entities;
using ComLib.Data;
using ComLib.LocationSupport;



namespace ComLib.Web.Modules.Events
{
    /// <summary>
    /// Generic repository for persisting Event.
    /// </summary>
    public partial class EventRepository : RepositorySql<Event>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public EventRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  EventRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public EventRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public EventRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new EventRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Event Create(Event entity)
        {
            string sql = "insert into Events ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Title], [Description], [Content], [IsPublished], [IsPublic], [CategoryId]"
			 + ", [StartDate], [EndDate], [StartTime], [EndTime], [Year], [Month]"
			 + ", [Day], [IsFeatured], [IsTravelRequired], [IsConference], [IsAllTimes], [Cost]"
			 + ", [Skill], [Seats], [IsAgeApplicable], [AgeFrom], [AgeTo], [Email]"
			 + ", [Phone], [Url], [Tags], [RefKey], [AverageRating], [TotalLiked]"
			 + ", [TotalDisLiked], [TotalBookMarked], [TotalAbuseReports], [HasMediaFiles], [TotalMediaFiles], [Street]"
			 + ", [City], [State], [Country], [Zip], [CityId], [StateId]"
			 + ", [CountryId], [IsOnline] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Title, @Description, @Content, @IsPublished, @IsPublic, @CategoryId"
			 + ", @StartDate, @EndDate, @StartTime, @EndTime, @Year, @Month"
			 + ", @Day, @IsFeatured, @IsTravelRequired, @IsConference, @IsAllTimes, @Cost"
			 + ", @Skill, @Seats, @IsAgeApplicable, @AgeFrom, @AgeTo, @Email"
			 + ", @Phone, @Url, @Tags, @RefKey, @AverageRating, @TotalLiked"
			 + ", @TotalDisLiked, @TotalBookMarked, @TotalAbuseReports, @HasMediaFiles, @TotalMediaFiles, @Street"
			 + ", @City, @State, @Country, @Zip, @CityId, @StateId"
			 + ", @CountryId, @IsOnline );" + IdentityStatement;;
            var dbparams = BuildParams(entity);            
            object result = _db.ExecuteScalarText(sql, dbparams);
            entity.Id = Convert.ToInt32(result);
            return entity;
        }


        /// <summary>
        /// Update the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Event Update(Event entity)
        {
            string sql = "update Events set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Title] = @Title, [Description] = @Description, [Content] = @Content, [IsPublished] = @IsPublished, [IsPublic] = @IsPublic, [CategoryId] = @CategoryId"
			 + ", [StartDate] = @StartDate, [EndDate] = @EndDate, [StartTime] = @StartTime, [EndTime] = @EndTime, [Year] = @Year, [Month] = @Month"
			 + ", [Day] = @Day, [IsFeatured] = @IsFeatured, [IsTravelRequired] = @IsTravelRequired, [IsConference] = @IsConference, [IsAllTimes] = @IsAllTimes, [Cost] = @Cost"
			 + ", [Skill] = @Skill, [Seats] = @Seats, [IsAgeApplicable] = @IsAgeApplicable, [AgeFrom] = @AgeFrom, [AgeTo] = @AgeTo, [Email] = @Email"
			 + ", [Phone] = @Phone, [Url] = @Url, [Tags] = @Tags, [RefKey] = @RefKey, [AverageRating] = @AverageRating, [TotalLiked] = @TotalLiked"
			 + ", [TotalDisLiked] = @TotalDisLiked, [TotalBookMarked] = @TotalBookMarked, [TotalAbuseReports] = @TotalAbuseReports, [HasMediaFiles] = @HasMediaFiles, [TotalMediaFiles] = @TotalMediaFiles, [Street] = @Street"
			 + ", [City] = @City, [State] = @State, [Country] = @Country, [Zip] = @Zip, [CityId] = @CityId, [StateId] = @StateId"
			 + ", [CountryId] = @CountryId, [IsOnline] = @IsOnline where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Event Get(int id)
        {
            Event entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Event entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@Title", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Title) ? "" : entity.Title));
			dbparams.Add(BuildParam("@Description", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Description) ? "" : entity.Description));
			dbparams.Add(BuildParam("@Content", SqlDbType.NText, string.IsNullOrEmpty(entity.Content) ? "" : entity.Content));
			dbparams.Add(BuildParam("@IsPublished", SqlDbType.Bit, entity.IsPublished));
			dbparams.Add(BuildParam("@IsPublic", SqlDbType.Bit, entity.IsPublic));
			dbparams.Add(BuildParam("@CategoryId", SqlDbType.Int, entity.CategoryId));
			dbparams.Add(BuildParam("@StartDate", SqlDbType.DateTime, entity.StartDate));
			dbparams.Add(BuildParam("@EndDate", SqlDbType.DateTime, entity.EndDate));
			dbparams.Add(BuildParam("@StartTime", SqlDbType.Int, entity.StartTime));
			dbparams.Add(BuildParam("@EndTime", SqlDbType.Int, entity.EndTime));
			dbparams.Add(BuildParam("@Year", SqlDbType.Int, entity.Year));
			dbparams.Add(BuildParam("@Month", SqlDbType.Int, entity.Month));
			dbparams.Add(BuildParam("@Day", SqlDbType.Int, entity.Day));
			dbparams.Add(BuildParam("@IsFeatured", SqlDbType.Bit, entity.IsFeatured));
			dbparams.Add(BuildParam("@IsTravelRequired", SqlDbType.Bit, entity.IsTravelRequired));
			dbparams.Add(BuildParam("@IsConference", SqlDbType.Bit, entity.IsConference));
			dbparams.Add(BuildParam("@IsAllTimes", SqlDbType.Bit, entity.IsAllTimes));
			dbparams.Add(BuildParam("@Cost", SqlDbType.Decimal, entity.Cost));
			dbparams.Add(BuildParam("@Skill", SqlDbType.Int, entity.Skill));
			dbparams.Add(BuildParam("@Seats", SqlDbType.Int, entity.Seats));
			dbparams.Add(BuildParam("@IsAgeApplicable", SqlDbType.Bit, entity.IsAgeApplicable));
			dbparams.Add(BuildParam("@AgeFrom", SqlDbType.Int, entity.AgeFrom));
			dbparams.Add(BuildParam("@AgeTo", SqlDbType.Int, entity.AgeTo));
			dbparams.Add(BuildParam("@Email", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Email) ? "" : entity.Email));
			dbparams.Add(BuildParam("@Phone", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Phone) ? "" : entity.Phone));
			dbparams.Add(BuildParam("@Url", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Url) ? "" : entity.Url));
			dbparams.Add(BuildParam("@Tags", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Tags) ? "" : entity.Tags));
			dbparams.Add(BuildParam("@RefKey", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.RefKey) ? "" : entity.RefKey));
			dbparams.Add(BuildParam("@AverageRating", SqlDbType.Int, entity.AverageRating));
			dbparams.Add(BuildParam("@TotalLiked", SqlDbType.Int, entity.TotalLiked));
			dbparams.Add(BuildParam("@TotalDisLiked", SqlDbType.Int, entity.TotalDisLiked));
			dbparams.Add(BuildParam("@TotalBookMarked", SqlDbType.Int, entity.TotalBookMarked));
			dbparams.Add(BuildParam("@TotalAbuseReports", SqlDbType.Int, entity.TotalAbuseReports));
			dbparams.Add(BuildParam("@HasMediaFiles", SqlDbType.Bit, entity.HasMediaFiles));
			dbparams.Add(BuildParam("@TotalMediaFiles", SqlDbType.Int, entity.TotalMediaFiles));
			dbparams.Add(BuildParam("@Street", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Address.Street) ? "" : entity.Address.Street));
			dbparams.Add(BuildParam("@City", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Address.City) ? "" : entity.Address.City));
			dbparams.Add(BuildParam("@State", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Address.State) ? "" : entity.Address.State));
			dbparams.Add(BuildParam("@Country", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Address.Country) ? "" : entity.Address.Country));
			dbparams.Add(BuildParam("@Zip", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Address.Zip) ? "" : entity.Address.Zip));
			dbparams.Add(BuildParam("@CityId", SqlDbType.Int, entity.Address.CityId));
			dbparams.Add(BuildParam("@StateId", SqlDbType.Int, entity.Address.StateId));
			dbparams.Add(BuildParam("@CountryId", SqlDbType.Int, entity.Address.CountryId));
			dbparams.Add(BuildParam("@IsOnline", SqlDbType.Bit, entity.Address.IsOnline));

            return dbparams.ToArray();
        }


        protected virtual DbParameter BuildParam(string name, SqlDbType dbType, object val)
        {
            var param = new SqlParameter(name, dbType);
            param.Value = val;
            return param;
        }

    }


    
    /// <summary>
    /// RowMapper for Event.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class EventRowMapper : EntityRowMapper<Event>, IEntityRowMapper<Event>
    {
        public override Event MapRow(IDataReader reader, int rowNumber)
        {
            Event entity =  _entityFactoryMethod == null ? Event.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.Title = reader["Title"] == DBNull.Value ? string.Empty : reader["Title"].ToString();
			entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			entity.Content = reader["Content"] == DBNull.Value ? string.Empty : reader["Content"].ToString();
			entity.IsPublished = reader["IsPublished"] == DBNull.Value ? false : (bool)reader["IsPublished"];
			entity.IsPublic = reader["IsPublic"] == DBNull.Value ? false : (bool)reader["IsPublic"];
			entity.CategoryId = reader["CategoryId"] == DBNull.Value ? 0 : (int)reader["CategoryId"];
			entity.StartDate = reader["StartDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["StartDate"];
			entity.EndDate = reader["EndDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["EndDate"];
			entity.StartTime = reader["StartTime"] == DBNull.Value ? 0 : (int)reader["StartTime"];
			entity.EndTime = reader["EndTime"] == DBNull.Value ? 0 : (int)reader["EndTime"];
			entity.IsFeatured = reader["IsFeatured"] == DBNull.Value ? false : (bool)reader["IsFeatured"];
			entity.IsTravelRequired = reader["IsTravelRequired"] == DBNull.Value ? false : (bool)reader["IsTravelRequired"];
			entity.IsConference = reader["IsConference"] == DBNull.Value ? false : (bool)reader["IsConference"];
			entity.IsAllTimes = reader["IsAllTimes"] == DBNull.Value ? false : (bool)reader["IsAllTimes"];
			entity.Cost = reader["Cost"] == DBNull.Value ? 0 : Convert.ToDouble(reader["Cost"]);
			entity.Skill = reader["Skill"] == DBNull.Value ? 0 : (int)reader["Skill"];
			entity.Seats = reader["Seats"] == DBNull.Value ? 0 : (int)reader["Seats"];
			entity.IsAgeApplicable = reader["IsAgeApplicable"] == DBNull.Value ? false : (bool)reader["IsAgeApplicable"];
			entity.AgeFrom = reader["AgeFrom"] == DBNull.Value ? 0 : (int)reader["AgeFrom"];
			entity.AgeTo = reader["AgeTo"] == DBNull.Value ? 0 : (int)reader["AgeTo"];
			entity.Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
			entity.Phone = reader["Phone"] == DBNull.Value ? string.Empty : reader["Phone"].ToString();
			entity.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();
			entity.Tags = reader["Tags"] == DBNull.Value ? string.Empty : reader["Tags"].ToString();
			entity.RefKey = reader["RefKey"] == DBNull.Value ? string.Empty : reader["RefKey"].ToString();
			entity.AverageRating = reader["AverageRating"] == DBNull.Value ? 0 : (int)reader["AverageRating"];
			entity.TotalLiked = reader["TotalLiked"] == DBNull.Value ? 0 : (int)reader["TotalLiked"];
			entity.TotalDisLiked = reader["TotalDisLiked"] == DBNull.Value ? 0 : (int)reader["TotalDisLiked"];
			entity.TotalBookMarked = reader["TotalBookMarked"] == DBNull.Value ? 0 : (int)reader["TotalBookMarked"];
			entity.TotalAbuseReports = reader["TotalAbuseReports"] == DBNull.Value ? 0 : (int)reader["TotalAbuseReports"];
			entity.HasMediaFiles = reader["HasMediaFiles"] == DBNull.Value ? false : (bool)reader["HasMediaFiles"];
			entity.TotalMediaFiles = reader["TotalMediaFiles"] == DBNull.Value ? 0 : (int)reader["TotalMediaFiles"];
entity.Address = new Address();
			entity.Address.Street = reader["Street"] == DBNull.Value ? string.Empty : reader["Street"].ToString();
			entity.Address.City = reader["City"] == DBNull.Value ? string.Empty : reader["City"].ToString();
			entity.Address.State = reader["State"] == DBNull.Value ? string.Empty : reader["State"].ToString();
			entity.Address.Country = reader["Country"] == DBNull.Value ? string.Empty : reader["Country"].ToString();
			entity.Address.Zip = reader["Zip"] == DBNull.Value ? string.Empty : reader["Zip"].ToString();
			entity.Address.CityId = reader["CityId"] == DBNull.Value ? 0 : (int)reader["CityId"];
			entity.Address.StateId = reader["StateId"] == DBNull.Value ? 0 : (int)reader["StateId"];
			entity.Address.CountryId = reader["CountryId"] == DBNull.Value ? 0 : (int)reader["CountryId"];
			entity.Address.IsOnline = reader["IsOnline"] == DBNull.Value ? false : (bool)reader["IsOnline"];

            return entity;
        }
    }
}