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



namespace ComLib.Web.Modules.Media
{
    /// <summary>
    /// Generic repository for persisting MediaFolder.
    /// </summary>
    public partial class MediaFolderRepository : RepositorySql<MediaFolder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedQueryRepository"/> class.
        /// </summary>
        public MediaFolderRepository() { }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection string.</param>
        public  MediaFolderRepository(string connectionString) : base(connectionString)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public MediaFolderRepository(ConnectionInfo connectionInfo) : base(connectionInfo)
        {            
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;TId, T&gt;"/> class.
        /// </summary>
        /// <param name="connectionInfo">The connection info.</param>
        /// <param name="helper">The helper.</param>
        public MediaFolderRepository(ConnectionInfo connectionInfo, IDatabase db)
            : base(connectionInfo, db)
        {
        }


        /// <summary>
        /// Initialize the rowmapper
        /// </summary>
        public override void Init(ConnectionInfo connectionInfo, IDatabase db)
        {
            base.Init(connectionInfo, db);
            this.RowMapper = new MediaFolderRowMapper();
        }


        /// <summary>
        /// Create the entity using sql.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public override MediaFolder Create(MediaFolder entity)
        {
            string sql = "insert into MediaFolders ( [CreateDate], [UpdateDate], [CreateUser], [UpdateUser], [UpdateComment], [AppId]"
			 + ", [Name], [FullName], [DirectoryName], [Extension], [Description], [Length]"
			 + ", [LastWriteTime], [SortIndex], [ParentId], [FileType], [IsPublic], [HasMediaFiles]"
			 + ", [TotalMediaFiles], [IsExternalFile], [ExternalFileSource], [HasThumbnail] ) values ( @CreateDate, @UpdateDate, @CreateUser, @UpdateUser, @UpdateComment, @AppId"
			 + ", @Name, @FullName, @DirectoryName, @Extension, @Description, @Length"
			 + ", @LastWriteTime, @SortIndex, @ParentId, @FileType, @IsPublic, @HasMediaFiles"
			 + ", @TotalMediaFiles, @IsExternalFile, @ExternalFileSource, @HasThumbnail );" + IdentityStatement;;
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
        public override MediaFolder Update(MediaFolder entity)
        {
            string sql = "update MediaFolders set [CreateDate] = @CreateDate, [UpdateDate] = @UpdateDate, [CreateUser] = @CreateUser, [UpdateUser] = @UpdateUser, [UpdateComment] = @UpdateComment, [AppId] = @AppId"
			 + ", [Name] = @Name, [FullName] = @FullName, [DirectoryName] = @DirectoryName, [Extension] = @Extension, [Description] = @Description, [Length] = @Length"
			 + ", [LastWriteTime] = @LastWriteTime, [SortIndex] = @SortIndex, [ParentId] = @ParentId, [FileType] = @FileType, [IsPublic] = @IsPublic, [HasMediaFiles] = @HasMediaFiles"
			 + ", [TotalMediaFiles] = @TotalMediaFiles, [IsExternalFile] = @IsExternalFile, [ExternalFileSource] = @ExternalFileSource, [HasThumbnail] = @HasThumbnail where Id = " + entity.Id;
            var dbparams = BuildParams(entity); 
            _db.ExecuteNonQueryText(sql, dbparams);
            return entity;
        }


        public override MediaFolder Get(int id)
        {
            MediaFolder entity = base.Get(id);
            
            return entity;
        }


        protected virtual DbParameter[] BuildParams(MediaFolder entity)
        {
            var dbparams = new List<DbParameter>();
            			dbparams.Add(BuildParam("@CreateDate", SqlDbType.DateTime, entity.CreateDate));
			dbparams.Add(BuildParam("@UpdateDate", SqlDbType.DateTime, entity.UpdateDate));
			dbparams.Add(BuildParam("@CreateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.CreateUser) ? "" : entity.CreateUser));
			dbparams.Add(BuildParam("@UpdateUser", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateUser) ? "" : entity.UpdateUser));
			dbparams.Add(BuildParam("@UpdateComment", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.UpdateComment) ? "" : entity.UpdateComment));
			dbparams.Add(BuildParam("@AppId", SqlDbType.Int, entity.AppId));
			dbparams.Add(BuildParam("@Name", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Name) ? "" : entity.Name));
			dbparams.Add(BuildParam("@FullName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.FullName) ? "" : entity.FullName));
			dbparams.Add(BuildParam("@DirectoryName", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.DirectoryName) ? "" : entity.DirectoryName));
			dbparams.Add(BuildParam("@Extension", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Extension) ? "" : entity.Extension));
			dbparams.Add(BuildParam("@Description", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.Description) ? "" : entity.Description));
			dbparams.Add(BuildParam("@Length", SqlDbType.Int, entity.Length));
			dbparams.Add(BuildParam("@LastWriteTime", SqlDbType.DateTime, entity.LastWriteTime));
			dbparams.Add(BuildParam("@SortIndex", SqlDbType.Int, entity.SortIndex));
			dbparams.Add(BuildParam("@ParentId", SqlDbType.Int, entity.ParentId));
			dbparams.Add(BuildParam("@FileType", SqlDbType.Int, entity.FileType));
			dbparams.Add(BuildParam("@IsPublic", SqlDbType.Bit, entity.IsPublic));
			dbparams.Add(BuildParam("@HasMediaFiles", SqlDbType.Bit, entity.HasMediaFiles));
			dbparams.Add(BuildParam("@TotalMediaFiles", SqlDbType.Int, entity.TotalMediaFiles));
			dbparams.Add(BuildParam("@IsExternalFile", SqlDbType.Bit, entity.IsExternalFile));
			dbparams.Add(BuildParam("@ExternalFileSource", SqlDbType.NVarChar, string.IsNullOrEmpty(entity.ExternalFileSource) ? "" : entity.ExternalFileSource));
			dbparams.Add(BuildParam("@HasThumbnail", SqlDbType.Bit, entity.HasThumbnail));

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
    /// RowMapper for MediaFolder.
    /// </summary>
    /// <typeparam name="?"></typeparam>
    public partial class MediaFolderRowMapper : EntityRowMapper<MediaFolder>, IEntityRowMapper<MediaFolder>
    {
        public override MediaFolder MapRow(IDataReader reader, int rowNumber)
        {
            MediaFolder entity =  _entityFactoryMethod == null ? MediaFolder.New() : _entityFactoryMethod(reader);
            			entity.Id = reader["Id"] == DBNull.Value ? 0 : (int)reader["Id"];
			entity.CreateDate = reader["CreateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["CreateDate"];
			entity.UpdateDate = reader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["UpdateDate"];
			entity.CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString();
			entity.UpdateUser = reader["UpdateUser"] == DBNull.Value ? string.Empty : reader["UpdateUser"].ToString();
			entity.UpdateComment = reader["UpdateComment"] == DBNull.Value ? string.Empty : reader["UpdateComment"].ToString();
			entity.AppId = reader["AppId"] == DBNull.Value ? 0 : (int)reader["AppId"];
			entity.Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString();
			entity.FullName = reader["FullName"] == DBNull.Value ? string.Empty : reader["FullName"].ToString();
			entity.DirectoryName = reader["DirectoryName"] == DBNull.Value ? string.Empty : reader["DirectoryName"].ToString();
			entity.Extension = reader["Extension"] == DBNull.Value ? string.Empty : reader["Extension"].ToString();
			entity.Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString();
			entity.Length = reader["Length"] == DBNull.Value ? 0 : (int)reader["Length"];
			entity.LastWriteTime = reader["LastWriteTime"] == DBNull.Value ? DateTime.MinValue : (DateTime)reader["LastWriteTime"];
			entity.SortIndex = reader["SortIndex"] == DBNull.Value ? 0 : (int)reader["SortIndex"];
			entity.ParentId = reader["ParentId"] == DBNull.Value ? 0 : (int)reader["ParentId"];
			entity.FileType = reader["FileType"] == DBNull.Value ? 0 : (int)reader["FileType"];
			entity.IsPublic = reader["IsPublic"] == DBNull.Value ? false : (bool)reader["IsPublic"];
			entity.HasMediaFiles = reader["HasMediaFiles"] == DBNull.Value ? false : (bool)reader["HasMediaFiles"];
			entity.TotalMediaFiles = reader["TotalMediaFiles"] == DBNull.Value ? 0 : (int)reader["TotalMediaFiles"];
			entity.IsExternalFile = reader["IsExternalFile"] == DBNull.Value ? false : (bool)reader["IsExternalFile"];
			entity.ExternalFileSource = reader["ExternalFileSource"] == DBNull.Value ? string.Empty : reader["ExternalFileSource"].ToString();
			entity.HasThumbnail = reader["HasThumbnail"] == DBNull.Value ? false : (bool)reader["HasThumbnail"];

            return entity;
        }
    }
}