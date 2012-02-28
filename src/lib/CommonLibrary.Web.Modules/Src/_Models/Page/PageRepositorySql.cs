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



namespace ComLib.Web.Modules.Pages
{
    /// <summary>
    /// Generic repository for persisting Page.
    /// </summary>
    public partial class PageRepository : RepositorySql<Page>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public PageRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  PageRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public PageRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public PageRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new PageRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Page Create(Page entity)
        {
            string sql = "insert into Pages ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Title], [Description], [Content], [IsPublished], [Keywords], [Slug]"
			 + ", [AccessRoles], [Parent], [IsPublic], [IsFrontPage] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Title, @Description, @Content, @IsPublished, @Keywords, @Slug"
			 + ", @AccessRoles, @Parent, @IsPublic, @IsFrontPage );" + IdentityStatement;;
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
        public override Page Update(Page entity)
        {
            string sql = "update Pages set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Title] = @Title, [Description] = @Description, [Content] = @Content, [IsPublished] = @IsPublished, [Keywords] = @Keywords, [Slug] = @Slug"
			 + ", [AccessRoles] = @AccessRoles, [Parent] = @Parent, [IsPublic] = @IsPublic, [IsFrontPage] = @IsFrontPage where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Page Get(int id)
        {
            Page entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Page entity)
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
			dbparams.Add(BuildParam("@IsPublished", SqlDbType.Int, entity.IsPublished));
			dbparams.Add(BuildParam("@Keywords", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Keywords) ? "" : entity.Keywords));
			dbparams.Add(BuildParam("@Slug", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Slug) ? "" : entity.Slug));
			dbparams.Add(BuildParam("@AccessRoles", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.AccessRoles) ? "" : entity.AccessRoles));
			dbparams.Add(BuildParam("@Parent", SqlDbType.Int, entity.Parent));
			dbparams.Add(BuildParam("@IsPublic", SqlDbType.Bit, entity.IsPublic));
			dbparams.Add(BuildParam("@IsFrontPage", SqlDbType.Bit, entity.IsFrontPage));

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
    /// RowMapper for Page.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class PageRowMapper : EntityRowMapper<Page>, IEntityRowMapper<Page>
    {
        public override Page MapRow(IDataReader reader, int rowNumber)
        {
            Page entity =  _entityFactoryMethod == null ? Page.New() : _entityFactoryMethod(reader);
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
			entity.IsPublished = reader["IsPublished"] == DBNull.Value ? 0 : (int)reader["IsPublished"];
			entity.Keywords = reader["Keywords"] == DBNull.Value ? string.Empty : reader["Keywords"].ToString();
			entity.Slug = reader["Slug"] == DBNull.Value ? string.Empty : reader["Slug"].ToString();
			entity.AccessRoles = reader["AccessRoles"] == DBNull.Value ? string.Empty : reader["AccessRoles"].ToString();
			entity.Parent = reader["Parent"] == DBNull.Value ? 0 : (int)reader["Parent"];
			entity.IsPublic = reader["IsPublic"] == DBNull.Value ? false : (bool)reader["IsPublic"];
			entity.IsFrontPage = reader["IsFrontPage"] == DBNull.Value ? false : (bool)reader["IsFrontPage"];

            return entity;
        }
    }
}