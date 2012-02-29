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
    /// Generic repository for persisting Widget.
    /// </summary>
    public partial class WidgetRepository : RepositorySql<Widget>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public WidgetRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  WidgetRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public WidgetRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public WidgetRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new WidgetRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Widget Create(Widget entity)
        {
            string sql = "insert into Widgets ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [Name]"
			 + ", [Description], [FullTypeName], [Path], [Version], [Author], [AuthorUrl]"
			 + ", [Email], [Url], [IncludeProperties], [ExcludeProperties], [StringClobProperties], [PathToEditor]"
			 + ", [DeclaringType], [DeclaringAssembly], [SortIndex], [IsCacheable], [IsEditable] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @Name"
			 + ", @Description, @FullTypeName, @Path, @Version, @Author, @AuthorUrl"
			 + ", @Email, @Url, @IncludeProperties, @ExcludeProperties, @StringClobProperties, @PathToEditor"
			 + ", @DeclaringType, @DeclaringAssembly, @SortIndex, @IsCacheable, @IsEditable );" + IdentityStatement;;
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
        public override Widget Update(Widget entity)
        {
            string sql = "update Widgets set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [Name] = @Name"
			 + ", [Description] = @Description, [FullTypeName] = @FullTypeName, [Path] = @Path, [Version] = @Version, [Author] = @Author, [AuthorUrl] = @AuthorUrl"
			 + ", [Email] = @Email, [Url] = @Url, [IncludeProperties] = @IncludeProperties, [ExcludeProperties] = @ExcludeProperties, [StringClobProperties] = @StringClobProperties, [PathToEditor] = @PathToEditor"
			 + ", [DeclaringType] = @DeclaringType, [DeclaringAssembly] = @DeclaringAssembly, [SortIndex] = @SortIndex, [IsCacheable] = @IsCacheable, [IsEditable] = @IsEditable where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Widget Get(int id)
        {
            Widget entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Widget entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@Description", SqlDbType.NText, string.IsNullOrEmpty(entity.Description) ? "" : entity.Description));
			dbparams.Add(BuildParam("@FullTypeName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.FullTypeName) ? "" : entity.FullTypeName));
			dbparams.Add(BuildParam("@Path", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Path) ? "" : entity.Path));
			dbparams.Add(BuildParam("@Version", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Version) ? "" : entity.Version));
			dbparams.Add(BuildParam("@Author", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Author) ? "" : entity.Author));
			dbparams.Add(BuildParam("@AuthorUrl", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.AuthorUrl) ? "" : entity.AuthorUrl));
			dbparams.Add(BuildParam("@Email", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Email) ? "" : entity.Email));
			dbparams.Add(BuildParam("@Url", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Url) ? "" : entity.Url));
			dbparams.Add(BuildParam("@IncludeProperties", SqlDbType.NText, string.IsNullOrEmpty(entity.IncludeProperties) ? "" : entity.IncludeProperties));
			dbparams.Add(BuildParam("@ExcludeProperties", SqlDbType.NText, string.IsNullOrEmpty(entity.ExcludeProperties) ? "" : entity.ExcludeProperties));
			dbparams.Add(BuildParam("@StringClobProperties", SqlDbType.NText, string.IsNullOrEmpty(entity.StringClobProperties) ? "" : entity.StringClobProperties));
			dbparams.Add(BuildParam("@PathToEditor", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.PathToEditor) ? "" : entity.PathToEditor));
			dbparams.Add(BuildParam("@DeclaringType", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.DeclaringType) ? "" : entity.DeclaringType));
			dbparams.Add(BuildParam("@DeclaringAssembly", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.DeclaringAssembly) ? "" : entity.DeclaringAssembly));
			dbparams.Add(BuildParam("@SortIndex", SqlDbType.Int, entity.SortIndex));
			dbparams.Add(BuildParam("@IsCacheable", SqlDbType.Bit, entity.IsCacheable));
			dbparams.Add(BuildParam("@IsEditable", SqlDbType.Bit, entity.IsEditable));

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
    /// RowMapper for Widget.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class WidgetRowMapper : EntityRowMapper<Widget>, IEntityRowMapper<Widget>
    {
        public override Widget MapRow(IDataReader reader, int rowNumber)
        {
            Widget entity =  _entityFactoryMethod == null ? Widget.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			entity.FullTypeName = reader["FullTypeName"] == DBNull.Value ? string.Empty : reader["FullTypeName"].ToString();
			entity.Path = reader["Path"] == DBNull.Value ? string.Empty : reader["Path"].ToString();
			entity.Version = reader["Version"] == DBNull.Value ? string.Empty : reader["Version"].ToString();
			entity.Author = reader["Author"] == DBNull.Value ? string.Empty : reader["Author"].ToString();
			entity.AuthorUrl = reader["AuthorUrl"] == DBNull.Value ? string.Empty : reader["AuthorUrl"].ToString();
			entity.Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
			entity.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();
			entity.IncludeProperties = reader["IncludeProperties"] == DBNull.Value ? string.Empty : reader["IncludeProperties"].ToString();
			entity.ExcludeProperties = reader["ExcludeProperties"] == DBNull.Value ? string.Empty : reader["ExcludeProperties"].ToString();
			entity.StringClobProperties = reader["StringClobProperties"] == DBNull.Value ? string.Empty : reader["StringClobProperties"].ToString();
			entity.PathToEditor = reader["PathToEditor"] == DBNull.Value ? string.Empty : reader["PathToEditor"].ToString();
			entity.DeclaringType = reader["DeclaringType"] == DBNull.Value ? string.Empty : reader["DeclaringType"].ToString();
			entity.DeclaringAssembly = reader["DeclaringAssembly"] == DBNull.Value ? string.Empty : reader["DeclaringAssembly"].ToString();
			entity.SortIndex = reader["SortIndex"] == DBNull.Value ? 0 : (int)reader["SortIndex"];
			entity.IsCacheable = reader["IsCacheable"] == DBNull.Value ? false : (bool)reader["IsCacheable"];
			entity.IsEditable = reader["IsEditable"] == DBNull.Value ? false : (bool)reader["IsEditable"];

            return entity;
        }
    }
}