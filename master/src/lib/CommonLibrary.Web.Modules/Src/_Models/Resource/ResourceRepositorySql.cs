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



namespace ComLib.Web.Modules.Resources
{
    /// <summary>
    /// Generic repository for persisting Resource.
    /// </summary>
    public partial class ResourceRepository : RepositorySql<Resource>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public ResourceRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  ResourceRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ResourceRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public ResourceRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new ResourceRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Resource Create(Resource entity)
        {
            string sql = "insert into Resources ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppName]"
			 + ", [Language], [ResourceType], [Section], [Key], [ValType], [Name]"
			 + ", [Description], [Example] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppName"
			 + ", @Language, @ResourceType, @Section, @Key, @ValType, @Name"
			 + ", @Description, @Example );" + IdentityStatement;;
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
        public override Resource Update(Resource entity)
        {
            string sql = "update Resources set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppName] = @AppName"
			 + ", [Language] = @Language, [ResourceType] = @ResourceType, [Section] = @Section, [Key] = @Key, [ValType] = @ValType, [Name] = @Name"
			 + ", [Description] = @Description, [Example] = @Example where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Resource Get(int id)
        {
            Resource entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Resource entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.AppName) ? "" : entity.AppName));
			dbparams.Add(BuildParam("@Language", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Language) ? "" : entity.Language));
			dbparams.Add(BuildParam("@ResourceType", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ResourceType) ? "" : entity.ResourceType));
			dbparams.Add(BuildParam("@Section", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Section) ? "" : entity.Section));
			dbparams.Add(BuildParam("@Key", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Key) ? "" : entity.Key));
			dbparams.Add(BuildParam("@ValType", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ValType) ? "" : entity.ValType));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@Description", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Description) ? "" : entity.Description));
			dbparams.Add(BuildParam("@Example", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Example) ? "" : entity.Example));

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
    /// RowMapper for Resource.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class ResourceRowMapper : EntityRowMapper<Resource>, IEntityRowMapper<Resource>
    {
        public override Resource MapRow(IDataReader reader, int rowNumber)
        {
            Resource entity =  _entityFactoryMethod == null ? Resource.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppName = reader["AppName"] == DBNull.Value ? string.Empty : reader["AppName"].ToString();
			entity.Language = reader["Language"] == DBNull.Value ? string.Empty : reader["Language"].ToString();
			entity.ResourceType = reader["ResourceType"] == DBNull.Value ? string.Empty : reader["ResourceType"].ToString();
			entity.Section = reader["Section"] == DBNull.Value ? string.Empty : reader["Section"].ToString();
			entity.Key = reader["Key"] == DBNull.Value ? string.Empty : reader["Key"].ToString();
			entity.ValType = reader["ValType"] == DBNull.Value ? string.Empty : reader["ValType"].ToString();
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			entity.Example = reader["Example"] == DBNull.Value ? string.Empty : reader["Example"].ToString();

            return entity;
        }
    }
}