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



namespace ComLib.Web.Modules.Widgets
{
    /// <summary>
    /// Generic repository for persisting WidgetInstance.
    /// </summary>
    public partial class WidgetInstanceRepository : RepositorySql<WidgetInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public WidgetInstanceRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  WidgetInstanceRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public WidgetInstanceRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public WidgetInstanceRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new WidgetInstanceRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override WidgetInstance Create(WidgetInstance entity)
        {
            string sql = "insert into WidgetInstances ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [WidgetId], [Header], [Zone], [DefName], [Roles], [StateData]"
			 + ", [SortIndex], [IsActive] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @WidgetId, @Header, @Zone, @DefName, @Roles, @StateData"
			 + ", @SortIndex, @IsActive );" + IdentityStatement;;
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
        public override WidgetInstance Update(WidgetInstance entity)
        {
            string sql = "update WidgetInstances set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [WidgetId] = @WidgetId, [Header] = @Header, [Zone] = @Zone, [DefName] = @DefName, [Roles] = @Roles, [StateData] = @StateData"
			 + ", [SortIndex] = @SortIndex, [IsActive] = @IsActive where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override WidgetInstance Get(int id)
        {
            WidgetInstance entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(WidgetInstance entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@WidgetId", SqlDbType.Int, entity.WidgetId));
			dbparams.Add(BuildParam("@Header", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Header) ? "" : entity.Header));
			dbparams.Add(BuildParam("@Zone", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Zone) ? "" : entity.Zone));
			dbparams.Add(BuildParam("@DefName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.DefName) ? "" : entity.DefName));
			dbparams.Add(BuildParam("@Roles", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Roles) ? "" : entity.Roles));
			dbparams.Add(BuildParam("@StateData", SqlDbType.NText, string.IsNullOrEmpty(entity.StateData) ? "" : entity.StateData));
			dbparams.Add(BuildParam("@SortIndex", SqlDbType.Int, entity.SortIndex));
			dbparams.Add(BuildParam("@IsActive", SqlDbType.Bit, entity.IsActive));

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
    /// RowMapper for WidgetInstance.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class WidgetInstanceRowMapper : EntityRowMapper<WidgetInstance>, IEntityRowMapper<WidgetInstance>
    {
        public override WidgetInstance MapRow(IDataReader reader, int rowNumber)
        {
            WidgetInstance entity =  _entityFactoryMethod == null ? WidgetInstance.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.WidgetId = reader["WidgetId"] == DBNull.Value ? 0 : (int)reader["WidgetId"];
			entity.Header = reader["Header"] == DBNull.Value ? string.Empty : reader["Header"].ToString();
			entity.Zone = reader["Zone"] == DBNull.Value ? string.Empty : reader["Zone"].ToString();
			entity.DefName = reader["DefName"] == DBNull.Value ? string.Empty : reader["DefName"].ToString();
			entity.Roles = reader["Roles"] == DBNull.Value ? string.Empty : reader["Roles"].ToString();
			entity.StateData = reader["StateData"] == DBNull.Value ? string.Empty : reader["StateData"].ToString();
			entity.SortIndex = reader["SortIndex"] == DBNull.Value ? 0 : (int)reader["SortIndex"];
			entity.IsActive = reader["IsActive"] == DBNull.Value ? false : (bool)reader["IsActive"];

            return entity;
        }
    }
}