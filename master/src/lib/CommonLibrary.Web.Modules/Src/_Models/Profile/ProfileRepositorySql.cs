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



namespace ComLib.Web.Modules.Profiles
{
    /// <summary>
    /// Generic repository for persisting Profile.
    /// </summary>
    public partial class ProfileRepository : RepositorySql<Profile>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public ProfileRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  ProfileRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ProfileRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ProfileRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new ProfileRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Profile Create(Profile entity)
        {
            string sql = "insert into Profiles ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [UserId]"
			 + ", [UserName], [About], [FirstName], [LastName], [Alias], [IsFeatured]"
			 + ", [Email], [WebSite], [ImageUrl], [AddressDisplayLevel], [ImageRefId], [EnableDisplayOfName]"
			 + ", [IsGravatarEnabled], [IsAddressEnabled], [EnableMessages], [HasMediaFiles], [TotalMediaFiles], [Street]"
			 + ", [City], [State], [Country], [Zip], [CityId], [StateId]"
			 + ", [CountryId], [IsOnline] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @UserId"
			 + ", @UserName, @About, @FirstName, @LastName, @Alias, @IsFeatured"
			 + ", @Email, @WebSite, @ImageUrl, @AddressDisplayLevel, @ImageRefId, @EnableDisplayOfName"
			 + ", @IsGravatarEnabled, @IsAddressEnabled, @EnableMessages, @HasMediaFiles, @TotalMediaFiles, @Street"
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
        public override Profile Update(Profile entity)
        {
            string sql = "update Profiles set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [UserId] = @UserId"
			 + ", [UserName] = @UserName, [About] = @About, [FirstName] = @FirstName, [LastName] = @LastName, [Alias] = @Alias, [IsFeatured] = @IsFeatured"
			 + ", [Email] = @Email, [WebSite] = @WebSite, [ImageUrl] = @ImageUrl, [AddressDisplayLevel] = @AddressDisplayLevel, [ImageRefId] = @ImageRefId, [EnableDisplayOfName] = @EnableDisplayOfName"
			 + ", [IsGravatarEnabled] = @IsGravatarEnabled, [IsAddressEnabled] = @IsAddressEnabled, [EnableMessages] = @EnableMessages, [HasMediaFiles] = @HasMediaFiles, [TotalMediaFiles] = @TotalMediaFiles, [Street] = @Street"
			 + ", [City] = @City, [State] = @State, [Country] = @Country, [Zip] = @Zip, [CityId] = @CityId, [StateId] = @StateId"
			 + ", [CountryId] = @CountryId, [IsOnline] = @IsOnline where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Profile Get(int id)
        {
            Profile entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Profile entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@UserId", SqlDbType.Int, entity.UserId));
			dbparams.Add(BuildParam("@UserName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UserName) ? "" : entity.UserName));
			dbparams.Add(BuildParam("@About", SqlDbType.NText, string.IsNullOrEmpty(entity.About) ? "" : entity.About));
			dbparams.Add(BuildParam("@FirstName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.FirstName) ? "" : entity.FirstName));
			dbparams.Add(BuildParam("@LastName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.LastName) ? "" : entity.LastName));
			dbparams.Add(BuildParam("@Alias", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Alias) ? "" : entity.Alias));
			dbparams.Add(BuildParam("@IsFeatured", SqlDbType.Bit, entity.IsFeatured));
			dbparams.Add(BuildParam("@Email", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Email) ? "" : entity.Email));
			dbparams.Add(BuildParam("@WebSite", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.WebSite) ? "" : entity.WebSite));
			dbparams.Add(BuildParam("@ImageUrl", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ImageUrl) ? "" : entity.ImageUrl));
			dbparams.Add(BuildParam("@AddressDisplayLevel", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.AddressDisplayLevel) ? "" : entity.AddressDisplayLevel));
			dbparams.Add(BuildParam("@ImageRefId", SqlDbType.Int, entity.ImageRefId));
			dbparams.Add(BuildParam("@EnableDisplayOfName", SqlDbType.Bit, entity.EnableDisplayOfName));
			dbparams.Add(BuildParam("@IsGravatarEnabled", SqlDbType.Bit, entity.IsGravatarEnabled));
			dbparams.Add(BuildParam("@IsAddressEnabled", SqlDbType.Bit, entity.IsAddressEnabled));
			dbparams.Add(BuildParam("@EnableMessages", SqlDbType.Bit, entity.EnableMessages));
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
    /// RowMapper for Profile.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class ProfileRowMapper : EntityRowMapper<Profile>, IEntityRowMapper<Profile>
    {
        public override Profile MapRow(IDataReader reader, int rowNumber)
        {
            Profile entity =  _entityFactoryMethod == null ? Profile.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.UserId = reader["UserId"] == DBNull.Value ? 0 : (int)reader["UserId"];
			entity.UserName = reader["UserName"] == DBNull.Value ? string.Empty : reader["UserName"].ToString();
			entity.About = reader["About"] == DBNull.Value ? string.Empty : reader["About"].ToString();
			entity.FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : reader["FirstName"].ToString();
			entity.LastName = reader["LastName"] == DBNull.Value ? string.Empty : reader["LastName"].ToString();
			entity.Alias = reader["Alias"] == DBNull.Value ? string.Empty : reader["Alias"].ToString();
			entity.IsFeatured = reader["IsFeatured"] == DBNull.Value ? false : (bool)reader["IsFeatured"];
			entity.Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
			entity.WebSite = reader["WebSite"] == DBNull.Value ? string.Empty : reader["WebSite"].ToString();
			entity.ImageUrl = reader["ImageUrl"] == DBNull.Value ? string.Empty : reader["ImageUrl"].ToString();
			entity.AddressDisplayLevel = reader["AddressDisplayLevel"] == DBNull.Value ? string.Empty : reader["AddressDisplayLevel"].ToString();
			entity.ImageRefId = reader["ImageRefId"] == DBNull.Value ? 0 : (int)reader["ImageRefId"];
			entity.EnableDisplayOfName = reader["EnableDisplayOfName"] == DBNull.Value ? false : (bool)reader["EnableDisplayOfName"];
			entity.IsGravatarEnabled = reader["IsGravatarEnabled"] == DBNull.Value ? false : (bool)reader["IsGravatarEnabled"];
			entity.IsAddressEnabled = reader["IsAddressEnabled"] == DBNull.Value ? false : (bool)reader["IsAddressEnabled"];
			entity.EnableMessages = reader["EnableMessages"] == DBNull.Value ? false : (bool)reader["EnableMessages"];
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