/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: ? 2009 Kishore Reddy
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

using ComLib.Data;
using ComLib.Entities;
using ComLib.Extensions;
using ComLib.LocationSupport;
using ComLib.ValidationSupport;


namespace ComLib.Web.Modules.Media
{
    /// <summary>
    /// ImageFile entity.
    /// </summary>
    public partial class MediaFile : ActiveRecordBaseEntity<MediaFile>, IEntity
    {
        /// <summary>
        /// Create a new widget instance and associate it w/ its widget.
        /// </summary>
        /// <param name="widgetTitle"></param>
        /// <param name="instance"></param>
        public static void Create(IList<MediaFile> files, bool checkIfPresent)
        {
            MediaHelper.CreateThumbNailsFor(files, true);
            // Now create the files in the datastore.
            if (checkIfPresent)
                Create(files, i => i.FullName, i => i.ParentId, i => i.RefGroupId, i => i.RefId);
            else
                Create(files);
        }


        /// <summary>
        /// Find images using the imagegallery view model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IList<MediaFile> Find(ImageGalleryViewModel model)
        {
            IQuery<MediaFile> query = model.Mode == MediaGalleryViewMode.FolderId
                            ? query = Query<MediaFile>.New().Where(i => i.ParentId).Is(model.FolderId).And(i => i.HasThumbnail).Is(true)
                            : query = Query<MediaFile>.New().Where(i => i.RefGroupId).Is(model.RefGroupId).And(i => i.RefId).Is(model.RefId).And(i => i.HasThumbnail).Is(true);

            // Get all images matching query.
            var images = MediaFile.Find(query);
            return images;
        }


        #region Public Properties
        public MediaFolder Folder { get; set; }


        public static readonly byte[] EmptyContents = new byte[]{};
        private byte[] _contents = EmptyContents;
        /// <summary>
        /// Get/Set Contents
        /// </summary>
        public byte[] Contents
        {
            get { return _contents; }
            set { if (value != null) _contents = value; }
        }


        /// <summary>
        /// Length in kilobytes
        /// </summary>
        public int LengthInK { get { return Length == 0 ? 0 : Length / 1000; } }


        private string _fullname;
        /// <summary>
        /// Get/Set FullName
        /// </summary>
        public string FullName
        {
            get { return _fullname; }
            set
            {
                _fullname = value;
            }
        }


        /// <summary>
        /// The full name in raw form which will be used to calculate the Name, FullName, Extension, IsExternalFile fields.
        /// </summary>
        public string FullNameRaw
        {
            set
            {
                _fullname = value;
                if (string.IsNullOrEmpty(_fullname))
                    return;

                string fullnamelower = _fullname.ToLower();
                if (fullnamelower.StartsWith("http:"))
                {
                    IsExternalFile = true;
                }
                // Calculate the file size if local file ( NOT UPLOADED ).
                Try.CatchLog(() =>
                {
                    if (HttpContext.Current != null && IsFileSystemFile)
                    {
                        var path = HttpContext.Current.Server.MapPath(_fullname);
                        FileInfo file = new FileInfo(path);
                        LastWriteTime = file.LastWriteTime;
                        Length = (int)file.Length;
                    }
                });
                Name = Path.GetFileName(_fullname);
                Extension = Path.GetExtension(Name);
            }
        }


        public string UrlName
        {
            get
            {
                string urlname = Name.Replace(" ", "-");
                urlname = urlname.Replace("_", "-");
                return urlname;
            }
        }


        /// <summary>
        /// Get the absolute url for referening in html.
        /// </summary>
        public string AbsoluteUrl
        {
            get
            {
                if (IsExternalFile)
                    return _fullname;

                // 1. File is located on the local file system.
                if ( IsFileSystemFile )
                    return _fullname;

                // 2. File is stored in mediafile in the underlying data store.
                // Set the id in the file name so that the media handler can get it from the datastore more quickly.
                var url = "/media/" + Id + "/" + Name;
                return url;
            }
        }


        /// <summary>
        /// Url for thumbnail.
        /// </summary>
        public string AbsoluteUrlThumbnail
        {
            get
            {
                if (IsExternalFile || !HasThumbnail)
                    return string.Empty;

                return "/media/" + Id + "/thumbnail" + Extension;
            }
        }


        /// <summary>
        /// Whether or not the file represented by this is on the local file system.
        /// </summary>
        public bool IsFileSystemFile
        {
            get { return _fullname[0] == '/' || _fullname[0] == '\\'; }
        }


        /// <summary>
        /// Whether or not this media file is associated w/ an entity.
        /// </summary>
        public bool IsDirectoryBased
        {
            get { return ParentId > 0 || !string.IsNullOrEmpty(DirectoryName); }
        }


        /// <summary>
        /// Is audio file?
        /// </summary>
        public bool IsAudio
        {
            get { return ComLib.MediaSupport.MediaHelper.IsAudioFormat(Extension); }
        }


        /// <summary>
        /// Is video file?
        /// </summary>
        public bool IsVideo
        {
            get { return ComLib.MediaSupport.MediaHelper.IsVideoFormat(Extension); }
        }


        /// <summary>
        /// Is image file?
        /// </summary>
        public bool IsImage
        {
            get { return ComLib.MediaSupport.MediaHelper.IsImageFormat(Extension); }
        }
        #endregion


        /// <summary>
        /// Convert this to thumbnail only if this is an image file.
        /// </summary>
        /// <returns></returns>
        public void ToThumbNail(int width = 40, int height = 40, bool processLocalFileSystemFile = false)
        {
            if (!IsImage || IsExternalFile ) return;

            HasThumbnail = !IsExternalFile;

            bool isLocalFileSystemFile = IsFileSystemFile;

            // Now convert to thumbnail by converting the contents. but check for length first.
            if (!isLocalFileSystemFile && ( Contents == null || Contents.Length == 0 ))
                return;

            byte[] contents = Contents;

            // Convert the local file system file to a thumbnail.
            if (isLocalFileSystemFile && processLocalFileSystemFile && contents == null || contents.Length == 0)
            {
                contents = File.ReadAllBytes(HttpContext.Current.Server.MapPath(_fullname));
            }

            var result = ComLib.MediaSupport.ImageHelper.ConvertToThumbNail(contents, width, height);
            if (result.Success)
            {
                ContentsForThumbNail = result.Item;
            }
        }

        
        /// <summary>
        /// Copy the properties from this instance to the copy supplied.
        /// Does not copy the length, and Contents properties.
        /// </summary>
        /// <param name="copy"></param>
        public void CopyTo(MediaFile copy)
        {
            copy.AppId = this.AppId;
            copy.CreateDate = this.CreateDate;
            copy.CreateUser = this.CreateUser;
            copy.Description = this.Description;
            copy.DirectoryName = this.DirectoryName;
            copy.Extension = this.Extension;
            copy.ExternalFileSource = this.ExternalFileSource;
            copy.FullName = this.FullName;
            copy.IsExternalFile = this.IsExternalFile;
            copy.IsPublic = this.IsPublic;
            copy.LastWriteTime = this.LastWriteTime;
            copy.Name = this.Name;
            copy.RefGroupId = this.RefGroupId;
            copy.ParentId = this.ParentId;
            copy.RefId = this.RefId;
            copy.SortIndex = this.SortIndex;
            copy.UpdateComment = this.UpdateComment;
            copy.UpdateDate = this.UpdateDate;
            copy.UpdateUser = this.UpdateUser;
        }


        /// <summary>
        /// Get validator for validating this entity.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                MediaFile entity = (MediaFile)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 100, results, "Name");
                Validation.IsStringLengthMatch(entity.FullName, false, true, true, 1, 150, results, "FullName");
                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Perform actions before saving to database.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            if (string.IsNullOrEmpty(this.Title))
                Title = this.Name;

            if (this.LastWriteTime == DateTime.MinValue)
                LastWriteTime = DateTime.Now;
            return true;
        }


        /// <summary>
        /// Set up various properties before saving.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool OnBeforeCreate(object ctx)
        {
            // Set the length;
            if(Length == 0)
                Length = Contents == null ? 0 : Contents.Length;
                        
            // Set extension from file name. e.g. mypic.jpg.
            if (string.IsNullOrEmpty(Extension)) Extension = Path.GetExtension(Name);
            
            // Get the folders
            if (IsDirectoryBased)
            {
                var parentid = ParentId;
                var lookup = MediaFolder.Repository.ToLookUpMulti<string>("Name");

                if (parentid <= 0 && !string.IsNullOrEmpty(DirectoryName))
                {
                    var parentfolder = lookup[DirectoryName];
                    if (parentfolder != null)
                    {
                        parentid = parentfolder.Id;
                        this.ParentId = parentid;
                    }
                }

                ToDo.Optimize(ToDo.Priority.Normal, "kishore", "Optimistic concurrency on media folder size calculation.", () =>
                {
                    var folder = MediaFolder.Get(parentid);
                    folder.Length += this.Length;
                    folder.Update();
                });
                // Setup directory name and folder id.
                DirectoryName = lookup[parentid].Name;
            }
            if (Contents != null && Contents.Length > 0 && IsImage)
            {
                Tuple2<int, int> dimensions = ComLib.MediaSupport.ImageHelper.GetDimensions(Contents);
                this.Height = dimensions.First;
                this.Width = dimensions.Second;
            }
            
            return true;
        }


        /// <summary>
        /// Set up various properties before saving.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public override bool  OnBeforeDelete(object ctx)
        {
            // Get the folders
            var parentid = ParentId;
            if (parentid > 0)
            {   
                ToDo.Optimize(ToDo.Priority.Normal, "kishore", "Optimistic concurrency on media folder size calculation.", () =>
                {
                    var folder = MediaFolder.Get(parentid);
                    folder.Length -= this.Length;
                    folder.Update();
                });
            }
            return true;
        }
    }
}
