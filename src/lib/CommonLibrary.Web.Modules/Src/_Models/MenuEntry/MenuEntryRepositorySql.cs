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



namespace ComLib.Web.Modules.MenuEntrys
{
    /// <summary>
    /// Generic repository for persisting MenuEntry.
    /// </summary>
    public partial class MenuEntryRepository : RepositorySql<MenuEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public MenuEntryRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  MenuEntryRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public MenuEntryRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public MenuEntryRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new MenuEntryRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override MenuEntry Create(MenuEntry entity)
        {
            string sql = "insert into MenuEntrys ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Name], [Url], [Description], [Roles], [ParentItem], [RefId]"
			 + ", [IsRerouted], [IsPublic], [SortIndex] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Name, @Url, @Description, @Roles, @ParentItem, @RefId"
			 + ", @IsRerouted, @IsPublic, @SortIndex );" + IdentityStatement;;
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
        public override MenuEntry Update(MenuEntry entity)
        {
            string sql = "update MenuEntrys set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Name] = @Name, [Url] = @Url, [Description] = @Description, [Roles] = @Roles, [ParentItem] = @ParentItem, [RefId] = @RefId"
			 + ", [IsRerouted] = @IsRerouted, [IsPublic] = @IsPublic, [SortIndex] = @SortIndex where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override MenuEntry Get(int id)
        {
            MenuEntry entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(MenuEntry entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@Url", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Url) ? "" : entity.Url));
			dbparams.Add(BuildParam("@Description", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Description) ? "" : entity.Description));
			dbparams.Add(BuildParam("@Roles", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Roles) ? "" : entity.Roles));
			dbparams.Add(BuildParam("@ParentItem", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ParentItem) ? "" : entity.ParentItem));
			dbparams.Add(BuildParam("@RefId", SqlDbType.Int, entity.RefId));
			dbparams.Add(BuildParam("@IsRerouted", SqlDbType.Bit, entity.IsRerouted));
			dbparams.Add(BuildParam("@IsPublic", SqlDbType.Bit, entity.IsPublic));
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
    /// RowMapper for MenuEntry.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class MenuEntryRowMapper : EntityRowMapper<MenuEntry>, IEntityRowMapper<MenuEntry>
    {
        public override MenuEntry MapRow(IDataReader reader, int rowNumber)
        {
            MenuEntry entity =  _entityFactoryMethod == null ? MenuEntry.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();
			entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			entity.Roles = reader["Roles"] == DBNull.Value ? string.Empty : reader["Roles"].ToString();
			entity.ParentItem = reader["ParentItem"] == DBNull.Value ? string.Empty : reader["ParentItem"].ToString();
			entity.RefId = reader["RefId"] == DBNull.Value ? 0 : (int)reader["RefId"];
			entity.IsRerouted = reader["IsRerouted"] == DBNull.Value ? false : (bool)reader["IsRerouted"];
			entity.SortIndex = reader["SortIndex"] == DBNull.Value ? 0 : (int)reader["SortIndex"];

            return entity;
        }
    }
}