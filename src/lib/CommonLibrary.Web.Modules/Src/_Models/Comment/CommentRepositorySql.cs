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



namespace ComLib.Web.Modules.Comments
{
    /// <summary>
    /// Generic repository for persisting Comment.
    /// </summary>
    public partial class CommentRepository : RepositorySql<Comment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public CommentRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  CommentRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public CommentRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public CommentRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new CommentRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Comment Create(Comment entity)
        {
            string sql = "insert into Comments ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [GroupId], [RefId], [Title], [Content], [UserId], [Name]"
			 + ", [Email], [Url], [ImageUrl], [IsGravatarEnabled], [IsApproved], [Rating] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @GroupId, @RefId, @Title, @Content, @UserId, @Name"
			 + ", @Email, @Url, @ImageUrl, @IsGravatarEnabled, @IsApproved, @Rating );" + IdentityStatement;;
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
        public override Comment Update(Comment entity)
        {
            string sql = "update Comments set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [GroupId] = @GroupId, [RefId] = @RefId, [Title] = @Title, [Content] = @Content, [UserId] = @UserId, [Name] = @Name"
			 + ", [Email] = @Email, [Url] = @Url, [ImageUrl] = @ImageUrl, [IsGravatarEnabled] = @IsGravatarEnabled, [IsApproved] = @IsApproved, [Rating] = @Rating where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Comment Get(int id)
        {
            Comment entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Comment entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@GroupId", SqlDbType.Int, entity.GroupId));
			dbparams.Add(BuildParam("@RefId", SqlDbType.Int, entity.RefId));
			dbparams.Add(BuildParam("@Title", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Title) ? "" : entity.Title));
			dbparams.Add(BuildParam("@Content", SqlDbType.NText, string.IsNullOrEmpty(entity.Content) ? "" : entity.Content));
			dbparams.Add(BuildParam("@UserId", SqlDbType.Int, entity.UserId));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@Email", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Email) ? "" : entity.Email));
			dbparams.Add(BuildParam("@Url", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Url) ? "" : entity.Url));
			dbparams.Add(BuildParam("@ImageUrl", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ImageUrl) ? "" : entity.ImageUrl));
			dbparams.Add(BuildParam("@IsGravatarEnabled", SqlDbType.Bit, entity.IsGravatarEnabled));
			dbparams.Add(BuildParam("@IsApproved", SqlDbType.Bit, entity.IsApproved));
			dbparams.Add(BuildParam("@Rating", SqlDbType.Int, entity.Rating));

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
    /// RowMapper for Comment.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class CommentRowMapper : EntityRowMapper<Comment>, IEntityRowMapper<Comment>
    {
        public override Comment MapRow(IDataReader reader, int rowNumber)
        {
            Comment entity =  _entityFactoryMethod == null ? Comment.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.GroupId = reader["GroupId"] == DBNull.Value ? 0 : (int)reader["GroupId"];
			entity.RefId = reader["RefId"] == DBNull.Value ? 0 : (int)reader["RefId"];
			entity.Title = reader["Title"] == DBNull.Value ? string.Empty : reader["Title"].ToString();
			entity.Content = reader["Content"] == DBNull.Value ? string.Empty : reader["Content"].ToString();
			entity.UserId = reader["UserId"] == DBNull.Value ? 0 : (int)reader["UserId"];
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.Email = reader["Email"] == DBNull.Value ? string.Empty : reader["Email"].ToString();
			entity.Url = reader["Url"] == DBNull.Value ? string.Empty : reader["Url"].ToString();
			entity.ImageUrl = reader["ImageUrl"] == DBNull.Value ? string.Empty : reader["ImageUrl"].ToString();
			entity.IsGravatarEnabled = reader["IsGravatarEnabled"] == DBNull.Value ? false : (bool)reader["IsGravatarEnabled"];
			entity.IsApproved = reader["IsApproved"] == DBNull.Value ? false : (bool)reader["IsApproved"];
			entity.Rating = reader["Rating"] == DBNull.Value ? 0 : (int)reader["Rating"];

            return entity;
        }
    }
}