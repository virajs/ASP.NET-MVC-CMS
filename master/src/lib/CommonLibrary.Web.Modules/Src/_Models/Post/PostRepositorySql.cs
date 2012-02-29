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



namespace ComLib.Web.Modules.Posts
{
    /// <summary>
    /// Generic repository for persisting Post.
    /// </summary>
    public partial class PostRepository : RepositorySql<Post>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public PostRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  PostRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public PostRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public PostRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new PostRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override Post Create(Post entity)
        {
            string sql = "insert into Posts ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Title], [Description], [Content], [PublishDate], [IsPublished], [IsPublic]"
			 + ", [CategoryId], [Tags], [Slug], [RefKey], [IsFavorite], [IsCommentEnabled]"
			 + ", [IsCommentModerated], [IsRatable], [Year], [Month], [CommentCount], [ViewCount]"
			 + ", [AverageRating], [TotalLiked], [TotalDisLiked], [HasMediaFiles], [TotalMediaFiles] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Title, @Description, @Content, @PublishDate, @IsPublished, @IsPublic"
			 + ", @CategoryId, @Tags, @Slug, @RefKey, @IsFavorite, @IsCommentEnabled"
			 + ", @IsCommentModerated, @IsRatable, @Year, @Month, @CommentCount, @ViewCount"
			 + ", @AverageRating, @TotalLiked, @TotalDisLiked, @HasMediaFiles, @TotalMediaFiles );" + IdentityStatement;;
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
        public override Post Update(Post entity)
        {
            string sql = "update Posts set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Title] = @Title, [Description] = @Description, [Content] = @Content, [PublishDate] = @PublishDate, [IsPublished] = @IsPublished, [IsPublic] = @IsPublic"
			 + ", [CategoryId] = @CategoryId, [Tags] = @Tags, [Slug] = @Slug, [RefKey] = @RefKey, [IsFavorite] = @IsFavorite, [IsCommentEnabled] = @IsCommentEnabled"
			 + ", [IsCommentModerated] = @IsCommentModerated, [IsRatable] = @IsRatable, [Year] = @Year, [Month] = @Month, [CommentCount] = @CommentCount, [ViewCount] = @ViewCount"
			 + ", [AverageRating] = @AverageRating, [TotalLiked] = @TotalLiked, [TotalDisLiked] = @TotalDisLiked, [HasMediaFiles] = @HasMediaFiles, [TotalMediaFiles] = @TotalMediaFiles where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override Post Get(int id)
        {
            Post entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(Post entity)
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
			dbparams.Add(BuildParam("@PublishDate", SqlDbType.DateTime, entity.PublishDate));
			dbparams.Add(BuildParam("@IsPublished", SqlDbType.Bit, entity.IsPublished));
			dbparams.Add(BuildParam("@IsPublic", SqlDbType.Bit, entity.IsPublic));
			dbparams.Add(BuildParam("@CategoryId", SqlDbType.Int, entity.CategoryId));
			dbparams.Add(BuildParam("@Tags", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Tags) ? "" : entity.Tags));
			dbparams.Add(BuildParam("@Slug", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Slug) ? "" : entity.Slug));
			dbparams.Add(BuildParam("@RefKey", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.RefKey) ? "" : entity.RefKey));
			dbparams.Add(BuildParam("@IsFavorite", SqlDbType.Bit, entity.IsFavorite));
			dbparams.Add(BuildParam("@IsCommentEnabled", SqlDbType.Bit, entity.IsCommentEnabled));
			dbparams.Add(BuildParam("@IsCommentModerated", SqlDbType.Bit, entity.IsCommentModerated));
			dbparams.Add(BuildParam("@IsRatable", SqlDbType.Bit, entity.IsRatable));
			dbparams.Add(BuildParam("@Year", SqlDbType.Int, entity.Year));
			dbparams.Add(BuildParam("@Month", SqlDbType.Int, entity.Month));
			dbparams.Add(BuildParam("@CommentCount", SqlDbType.Int, entity.CommentCount));
			dbparams.Add(BuildParam("@ViewCount", SqlDbType.Int, entity.ViewCount));
			dbparams.Add(BuildParam("@AverageRating", SqlDbType.Int, entity.AverageRating));
			dbparams.Add(BuildParam("@TotalLiked", SqlDbType.Int, entity.TotalLiked));
			dbparams.Add(BuildParam("@TotalDisLiked", SqlDbType.Int, entity.TotalDisLiked));
			dbparams.Add(BuildParam("@HasMediaFiles", SqlDbType.Bit, entity.HasMediaFiles));
			dbparams.Add(BuildParam("@TotalMediaFiles", SqlDbType.Int, entity.TotalMediaFiles));

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
    /// RowMapper for Post.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class PostRowMapper : EntityRowMapper<Post>, IEntityRowMapper<Post>
    {
        public override Post MapRow(IDataReader reader, int rowNumber)
        {
            Post entity =  _entityFactoryMethod == null ? Post.New() : _entityFactoryMethod(reader);
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
			entity.PublishDate = reader["PublishDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["PublishDate"];
			entity.IsPublished = reader["IsPublished"] == DBNull.Value ? false : (bool)reader["IsPublished"];
			entity.IsPublic = reader["IsPublic"] == DBNull.Value ? false : (bool)reader["IsPublic"];
			entity.CategoryId = reader["CategoryId"] == DBNull.Value ? 0 : (int)reader["CategoryId"];
			entity.Tags = reader["Tags"] == DBNull.Value ? string.Empty : reader["Tags"].ToString();
			entity.Slug = reader["Slug"] == DBNull.Value ? string.Empty : reader["Slug"].ToString();
			entity.RefKey = reader["RefKey"] == DBNull.Value ? string.Empty : reader["RefKey"].ToString();
			entity.IsFavorite = reader["IsFavorite"] == DBNull.Value ? false : (bool)reader["IsFavorite"];
			entity.IsCommentEnabled = reader["IsCommentEnabled"] == DBNull.Value ? false : (bool)reader["IsCommentEnabled"];
			entity.IsCommentModerated = reader["IsCommentModerated"] == DBNull.Value ? false : (bool)reader["IsCommentModerated"];
			entity.IsRatable = reader["IsRatable"] == DBNull.Value ? false : (bool)reader["IsRatable"];
			entity.CommentCount = reader["CommentCount"] == DBNull.Value ? 0 : (int)reader["CommentCount"];
			entity.ViewCount = reader["ViewCount"] == DBNull.Value ? 0 : (int)reader["ViewCount"];
			entity.AverageRating = reader["AverageRating"] == DBNull.Value ? 0 : (int)reader["AverageRating"];
			entity.TotalLiked = reader["TotalLiked"] == DBNull.Value ? 0 : (int)reader["TotalLiked"];
			entity.TotalDisLiked = reader["TotalDisLiked"] == DBNull.Value ? 0 : (int)reader["TotalDisLiked"];
			entity.HasMediaFiles = reader["HasMediaFiles"] == DBNull.Value ? false : (bool)reader["HasMediaFiles"];
			entity.TotalMediaFiles = reader["TotalMediaFiles"] == DBNull.Value ? 0 : (int)reader["TotalMediaFiles"];

            return entity;
        }
    }
}