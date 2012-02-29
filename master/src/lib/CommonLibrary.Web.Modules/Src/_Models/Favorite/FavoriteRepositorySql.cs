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



namespace ComLib.Web.Modules.Favorites
{
    /// <summary>
    /// Generic repository for persisting Favorite.
    /// </summary>
    public partial class FavoriteRepository : RepositorySql<Favorite>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public FavoriteRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  FavoriteRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public FavoriteRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public FavoriteRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new FavoriteRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Favorite Create(Favorite entity)
        {
            string sql = "insert into Favorites ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [UserId], [Title], [RefId], [Model], [Url] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @UserId, @Title, @RefId, @Model, @Url );" + IdentityStatement;;
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
        public override Favorite Update(Favorite entity)
        {
            string sql = "update Favorites set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [UserId] = @UserId, [Title] = @Title, [RefId] = @RefId, [Model] = @Model, [Url] = @Url where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Favorite Get(int id)
        {
            Favorite entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Favorite entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@UserId", SqlDbType.Int, entity.UserId));
			dbparams.Add(BuildParam("@Title", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Title) ? "" : entity.Title));
			dbparams.Add(BuildParam("@RefId", SqlDbType.Int, entity.RefId));
			dbparams.Add(BuildParam("@Model", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Model) ? "" : entity.Model));
			dbparams.Add(BuildParam("@Url", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Url) ? "" : entity.Url));

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
    /// RowMapper for Favorite.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class FavoriteRowMapper : EntityRowMapper<Favorite>, IEntityRowMapper<Favorite>
    {
        public override Favorite MapRow(IDataReader reader, int rowNumber)
        {
            Favorite entity =  _entityFactoryMethod == null ? Favorite.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.UserId = reader["UserId"] == DBNull.Value ? 0 : (int)reader["UserId"];
			entity.Title = reader["Title"] == DBNull.Value ? string.Empty : reader["Title"].ToString();
			entity.RefId = reader["RefId"] == DBNull.Value ? 0 : (int)reader["RefId"];
			entity.Model = reader["Model"] == DBNull.Value ? string.Empty : reader["Model"].ToString();
			entity.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();

            return entity;
        }
    }
}