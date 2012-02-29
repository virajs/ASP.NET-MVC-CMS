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



namespace ComLib.Web.Modules.Themes
{
    /// <summary>
    /// Generic repository for persisting Theme.
    /// </summary>
    public partial class ThemeRepository : RepositorySql<Theme>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public ThemeRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  ThemeRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ThemeRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ThemeRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new ThemeRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Theme Create(Theme entity)
        {
            string sql = "insert into Themes ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Name], [Description], [Path], [Layouts], [Zones], [SelectedLayout]"
			 + ", [IsActive], [Author], [Version], [Email], [Url], [SortIndex] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Name, @Description, @Path, @Layouts, @Zones, @SelectedLayout"
			 + ", @IsActive, @Author, @Version, @Email, @Url, @SortIndex );" + IdentityStatement;;
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
        public override Theme Update(Theme entity)
        {
            string sql = "update Themes set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Name] = @Name, [Description] = @Description, [Path] = @Path, [Layouts] = @Layouts, [Zones] = @Zones, [SelectedLayout] = @SelectedLayout"
			 + ", [IsActive] = @IsActive, [Author] = @Author, [Version] = @Version, [Email] = @Email, [Url] = @Url, [SortIndex] = @SortIndex where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Theme Get(int id)
        {
            Theme entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Theme entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@Description", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Description) ? "" : entity.Description));
			dbparams.Add(BuildParam("@Path", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Path) ? "" : entity.Path));
			dbparams.Add(BuildParam("@Layouts", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Layouts) ? "" : entity.Layouts));
			dbparams.Add(BuildParam("@Zones", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Zones) ? "" : entity.Zones));
			dbparams.Add(BuildParam("@SelectedLayout", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.SelectedLayout) ? "" : entity.SelectedLayout));
			dbparams.Add(BuildParam("@IsActive", SqlDbType.Bit, entity.IsActive));
			dbparams.Add(BuildParam("@Author", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Author) ? "" : entity.Author));
			dbparams.Add(BuildParam("@Version", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Version) ? "" : entity.Version));
			dbparams.Add(BuildParam("@Email", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Email) ? "" : entity.Email));
			dbparams.Add(BuildParam("@Url", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Url) ? "" : entity.Url));
			dbparams.Add(BuildParam("@SortIndex", SqlDbType.Int, entity.SortIndex));

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
    /// RowMapper for Theme.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class ThemeRowMapper : EntityRowMapper<Theme>, IEntityRowMapper<Theme>
    {
        public override Theme MapRow(IDataReader reader, int rowNumber)
        {
            Theme entity =  _entityFactoryMethod == null ? Theme.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			entity.Path = reader["Path"] == DBNull.Value ? string.Empty : reader["Path"].ToString();
			entity.Layouts = reader["Layouts"] == DBNull.Value ? string.Empty : reader["Layouts"].ToString();
			entity.Zones = reader["Zones"] == DBNull.Value ? string.Empty : reader["Zones"].ToString();
			entity.SelectedLayout = reader["SelectedLayout"] == DBNull.Value ? string.Empty : reader["SelectedLayout"].ToString();
			entity.IsActive = reader["IsActive"] == DBNull.Value ? false : (bool)reader["IsActive"];
			entity.Author = reader["Author"] == DBNull.Value ? string.Empty : reader["Author"].ToString();
			entity.Version = reader["Version"] == DBNull.Value ? string.Empty : reader["Version"].ToString();
			entity.Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
			entity.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();
			entity.SortIndex = reader["SortIndex"] == DBNull.Value ? 0 : (int)reader["SortIndex"];

            return entity;
        }
    }
}