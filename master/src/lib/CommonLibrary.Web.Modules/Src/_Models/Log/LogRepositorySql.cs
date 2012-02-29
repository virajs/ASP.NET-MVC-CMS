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



namespace ComLib.Web.Modules.Logs
{
    /// <summary>
    /// Generic repository for persisting Log.
    /// </summary>
    public partial class LogRepository : RepositorySql<Log>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public LogRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  LogRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public LogRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public LogRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new LogRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Log Create(Log entity)
        {
            string sql = "insert into Logs ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [Application]"
			 + ", [Computer], [LogLevel], [Exception], [Message], [UserName] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @Application"
			 + ", @Computer, @LogLevel, @Exception, @Message, @UserName );" + IdentityStatement;;
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
        public override Log Update(Log entity)
        {
            string sql = "update Logs set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [Application] = @Application"
			 + ", [Computer] = @Computer, [LogLevel] = @LogLevel, [Exception] = @Exception, [Message] = @Message, [UserName] = @UserName where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Log Get(int id)
        {
            Log entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Log entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@Application", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Application) ? "" : entity.Application));
			dbparams.Add(BuildParam("@Computer", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Computer) ? "" : entity.Computer));
			dbparams.Add(BuildParam("@LogLevel", SqlDbType.Int, entity.LogLevel));
			dbparams.Add(BuildParam("@Exception", SqlDbType.NText, string.IsNullOrEmpty(entity.Exception) ? "" : entity.Exception));
			dbparams.Add(BuildParam("@Message", SqlDbType.NText, string.IsNullOrEmpty(entity.Message) ? "" : entity.Message));
			dbparams.Add(BuildParam("@UserName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UserName) ? "" : entity.UserName));

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
    /// RowMapper for Log.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class LogRowMapper : EntityRowMapper<Log>, IEntityRowMapper<Log>
    {
        public override Log MapRow(IDataReader reader, int rowNumber)
        {
            Log entity =  _entityFactoryMethod == null ? Log.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.Application = reader["Application"] == DBNull.Value ? string.Empty : reader["Application"].ToString();
			entity.Computer = reader["Computer"] == DBNull.Value ? string.Empty : reader["Computer"].ToString();
			entity.LogLevel = reader["LogLevel"] == DBNull.Value ? 0 : (int)reader["LogLevel"];
			entity.Exception = reader["Exception"] == DBNull.Value ? string.Empty : reader["Exception"].ToString();
			entity.Message = reader["Message"] == DBNull.Value ? string.Empty : reader["Message"].ToString();
			entity.UserName = reader["UserName"] == DBNull.Value ? string.Empty : reader["UserName"].ToString();

            return entity;
        }
    }
}