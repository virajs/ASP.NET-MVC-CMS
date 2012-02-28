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



namespace ComLib.Web.Modules.OptionDefs
{
    /// <summary>
    /// Generic repository for persisting OptionDef.
    /// </summary>
    public partial class OptionDefRepository : RepositorySql<OptionDef>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public OptionDefRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  OptionDefRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public OptionDefRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public OptionDefRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new OptionDefRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override OptionDef Create(OptionDef entity)
        {
            string sql = "insert into OptionDefs ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Section], [Key], [ValType], [Values], [DefaultValue], [DisplayStyle]"
			 + ", [IsRequired], [IsBasicType], [SortIndex] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Section, @Key, @ValType, @Values, @DefaultValue, @DisplayStyle"
			 + ", @IsRequired, @IsBasicType, @SortIndex );" + IdentityStatement;;
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
        public override OptionDef Update(OptionDef entity)
        {
            string sql = "update OptionDefs set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Section] = @Section, [Key] = @Key, [ValType] = @ValType, [Values] = @Values, [DefaultValue] = @DefaultValue, [DisplayStyle] = @DisplayStyle"
			 + ", [IsRequired] = @IsRequired, [IsBasicType] = @IsBasicType, [SortIndex] = @SortIndex where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override OptionDef Get(int id)
        {
            OptionDef entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(OptionDef entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@Section", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Section) ? "" : entity.Section));
			dbparams.Add(BuildParam("@Key", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Key) ? "" : entity.Key));
			dbparams.Add(BuildParam("@ValType", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ValType) ? "" : entity.ValType));
			dbparams.Add(BuildParam("@Values", SqlDbType.NText, string.IsNullOrEmpty(entity.Values) ? "" : entity.Values));
			dbparams.Add(BuildParam("@DefaultValue", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.DefaultValue) ? "" : entity.DefaultValue));
			dbparams.Add(BuildParam("@DisplayStyle", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.DisplayStyle) ? "" : entity.DisplayStyle));
			dbparams.Add(BuildParam("@IsRequired", SqlDbType.Bit, entity.IsRequired));
			dbparams.Add(BuildParam("@IsBasicType", SqlDbType.Bit, entity.IsBasicType));
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
    /// RowMapper for OptionDef.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class OptionDefRowMapper : EntityRowMapper<OptionDef>, IEntityRowMapper<OptionDef>
    {
        public override OptionDef MapRow(IDataReader reader, int rowNumber)
        {
            OptionDef entity =  _entityFactoryMethod == null ? OptionDef.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.Section = reader["Section"] == DBNull.Value ? string.Empty : reader["Section"].ToString();
			entity.Key = reader["Key"] == DBNull.Value ? string.Empty : reader["Key"].ToString();
			entity.ValType = reader["ValType"] == DBNull.Value ? string.Empty : reader["ValType"].ToString();
			entity.Values = reader["Values"] == DBNull.Value ? string.Empty : reader["Values"].ToString();
			entity.DefaultValue = reader["DefaultValue"] == DBNull.Value ? string.Empty : reader["DefaultValue"].ToString();
			entity.DisplayStyle = reader["DisplayStyle"] == DBNull.Value ? string.Empty : reader["DisplayStyle"].ToString();
			entity.IsRequired = reader["IsRequired"] == DBNull.Value ? false : (bool)reader["IsRequired"];
			entity.IsBasicType = reader["IsBasicType"] == DBNull.Value ? false : (bool)reader["IsBasicType"];
			entity.SortIndex = reader["SortIndex"] == DBNull.Value ? 0 : (int)reader["SortIndex"];

            return entity;
        }
    }
}