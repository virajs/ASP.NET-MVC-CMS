using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq;
using System.Data;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Modules.Media;


namespace CommonLibrary.Tests
{
    [TestFixture]
    public class StorageTests
    {
        public StorageTests()
        {
            MediaFolder.Init(new RepositoryInMemory<MediaFolder>("Id,CreateUser,CreateDate,UpdateUser,UpdateDate,Name,FullName,Extension,SortIndex,ParentId"), false);
            MediaFile.Init(new RepositoryInMemory<MediaFile>("Id,CreateUser,CreateDate,UpdateUser,UpdateDate,Name,FullName,Extensions,SortIndex,ParentId"), false);

            // 1. Create the folder.
            MediaFolder.Create(new MediaFolder() { Name = "photos", Description = "for images", DirectoryName = string.Empty, SortIndex = 1, IsPublic = true, Length = 0 });
            MediaFolder.Create(new MediaFolder() { Name = "videos", Description = "for videos", DirectoryName = string.Empty, SortIndex = 2, IsPublic = true, Length = 0 });
            MediaFolder.Create(new MediaFolder() { Name = "audios", Description = "for audios", DirectoryName = string.Empty, SortIndex = 3, IsPublic = true, Length = 0 });
            MediaFolder.Create(new MediaFolder() { Name = "docs", Description = "for docs", DirectoryName = string.Empty, SortIndex = 4, IsPublic = true, Length = 0 });
        }


        [Test]
        public void CanSetupMediaFileAsExternal()
        {
            var mediafile = new MediaFile();
            mediafile.FullNameRaw = "http://www.flickr.com/kishore/my_profile.jpg";
            Assert.IsTrue(mediafile.IsExternalFile);
            Assert.AreEqual(mediafile.FullName, "http://www.flickr.com/kishore/my_profile.jpg");            
            Assert.AreEqual(mediafile.Name, "my_profile.jpg");
            Assert.AreEqual(mediafile.Extension, ".jpg");
            Assert.IsTrue(mediafile.IsImage);
            // Simulate persistance
            mediafile.Id = 1;
            Assert.AreEqual(mediafile.AbsoluteUrl, "http://www.flickr.com/kishore/my_profile.jpg");

            mediafile = new MediaFile();
            mediafile.FullNameRaw = "http://www.flickr.com/kishore/my_profile";
            Assert.IsTrue(mediafile.IsExternalFile);
            Assert.AreEqual(mediafile.FullName, "http://www.flickr.com/kishore/my_profile");
            Assert.AreEqual(mediafile.Name, "my_profile");
            Assert.AreEqual(mediafile.Extension, string.Empty);
            mediafile.Id = 1;
            Assert.AreEqual(mediafile.AbsoluteUrl, "http://www.flickr.com/kishore/my_profile");
        }


        [Test]
        public void CanSetupInternalFileSystemFile()
        {
            var mediafile = new MediaFile();
            mediafile.Contents = new byte[] { }; 
            mediafile.FullNameRaw = "/content/media/photos/my_user_profile.jpg";
            Assert.IsFalse(mediafile.IsExternalFile);
            Assert.AreEqual(mediafile.FullName, "/content/media/photos/my_user_profile.jpg");
            Assert.AreEqual(mediafile.Name, "my_user_profile.jpg");
            Assert.AreEqual(mediafile.Extension, ".jpg");
            mediafile.Id = 1234;
            Assert.AreEqual(mediafile.AbsoluteUrl, "/content/media/photos/my_user_profile.jpg");
        }


        [Test]
        public void CanSetupUploadedFile()
        {
            var mediafile = new MediaFile();
            mediafile.Contents = new byte[] { };
            mediafile.FullNameRaw = "my_user_profile.jpg";
            Assert.IsFalse(mediafile.IsExternalFile);
            Assert.AreEqual(mediafile.FullName, "my_user_profile.jpg");
            Assert.AreEqual(mediafile.Name, "my_user_profile.jpg");
            Assert.AreEqual(mediafile.Extension, ".jpg");
            Assert.IsTrue(mediafile.IsImage);
            mediafile.Id = 1234;
            Assert.AreEqual(mediafile.AbsoluteUrl, "/media/1234/my_user_profile.jpg");
        }


        [Test]
        public void CanCreateThumbNailFromMainImage()
        {
            var mediafile = new MediaFile();
            mediafile.Contents = new byte[] { };
            mediafile.FullNameRaw = "my_user_profile.jpg";
            mediafile.Id = 1234;
            mediafile.ToThumbNail();

            // Confirm that the original file is still valid.
            Assert.IsFalse(mediafile.IsExternalFile);
            Assert.AreEqual(mediafile.FullName, "my_user_profile.jpg");
            Assert.AreEqual(mediafile.Name, "my_user_profile.jpg");
            Assert.AreEqual(mediafile.Extension, ".jpg");
            Assert.IsTrue(mediafile.IsImage);
            Assert.AreEqual(mediafile.AbsoluteUrl, "/media/1234/my_user_profile.jpg");
            Assert.IsTrue(mediafile.HasThumbnail);
            Assert.AreEqual(mediafile.AbsoluteUrlThumbnail, "/media/1234/thumbnail.jpg");
        }


        [Test]
        public void CanStore()
        {
            // Get the folders
            var lookup = MediaFolder.Repository.ToLookUpMulti<string>("Name");
            var file1 = new MediaFile()
            {
                FullNameRaw = "profile_image.jpg",
                Description = "Main photo for profile.",
                DirectoryName = "photos",
                SortIndex = 1,
                IsPublic = true,
                Length = 0,
                ParentId = lookup["photos"].Id,
                Contents = "testing some sample content".ToBytesAscii()
            };

            var controller = new MediaFileController();

            MediaFile.Create(file1);

            // Confirm 2 things.
            // 1. The length of the MediaFolder is updated.
            // 2. The length of the MediaFile is updated
            // 3. The extension on the MediaFile is set to jpg
            var folder = MediaFolder.Get(lookup["photos"].Id);
            var file = MediaFile.GetAll()[0];

            Assert.AreEqual(file.Extension, ".jpg");
            Assert.AreEqual(folder.Length, file.Length);
        }

    }
}
