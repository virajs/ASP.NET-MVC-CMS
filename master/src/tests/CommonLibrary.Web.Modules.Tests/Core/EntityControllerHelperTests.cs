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
using ComLib.IO;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Modules.Links;
using ComLib.Web.Lib.Models;
using ComLib.Authentication;

using CommonLibraryNet.Tests.Common;


namespace CommonLibrary.Tests
{   
    [TestFixture]
    public class EntityControllerHelperTests
    {
        public EntityControllerHelperTests()
        {
            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "User", "Custom", true)));
            Link.Init(new RepositoryInMemory<Link>(), false);
            int count = 20;
            Console.WriteLine("Entity Controller Helper Tests constructor");
            count.Times( ndx =>
            {
                Link.Create(new Link() { Name = "link " + ndx, Group = "bloggers", SortIndex = ndx, Url = "http://www.link" + ndx + ".com" });
            });
        }


        private EntityHelper<T> CreateHelper<T>() where T: IEntity, new()
        {
            var content = ContentLoader.GetTextFileContent("Models.ini.config");
            var inidoc = new IniDocument(content, false);
            var settings = new EntitySettingsHelper(inidoc);
            var helper = new EntityHelper<T>(settings);
            return helper;
        }

        
        [Test]
        public void CanCopy()
        {
            var helper = CreateHelper<Link>();
            var firstItem = EntityRegistration.GetService<Link>().GetAll()[0];
            var result = helper.Copy(firstItem.Id);

            // This is a viewmodel
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Message, string.Empty);
            Assert.IsNotNull(result.Item);
            Assert.AreEqual(result.ItemAs<Link>().Id, 0);
            Assert.AreEqual(result.ItemAs<Link>().Name, firstItem.Name);
            Assert.IsTrue(result.IsAuthorized);
            Assert.IsTrue(result.IsAvailable);
        }


        [Test]
        public void CanCreate()
        {
            var helper = new EntityHelper<Link>();
            var link = new Link() { Name = "helix cms", Url = "http://helixcms.com", Group = "sites", SortIndex = 2 };
            var result = helper.Create(link);
                

            // This is a viewmodel
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Message, string.Empty);
            Assert.IsNotNull(result.Item);
            Assert.AreNotEqual(result.ItemAs<Link>().Id, 0);
            Assert.IsNotNullOrEmpty(result.ItemAs<Link>().Name);
            Assert.IsTrue(result.IsAuthorized);
            Assert.IsTrue(result.IsAvailable);
        }


        [Test]
        public void CanCreateViaPermissionsAsGuest()
        {
            var settings = new Dictionary<string, object>();
            settings["Link"] = new Dictionary<string, object>();
            settings.GetSection("Link")["Create"] = "?";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));

            var security = new EntitySettingsHelper(settings);
            var helper = new EntityHelper<Link>(security);
            var link = new Link() { Name = "helix cms", Url = "http://helixcms.com", Group = "sites", SortIndex = 2 };
            var result = helper.Create(link);


            // This is a viewmodel
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Message, string.Empty);
            Assert.IsNotNull(result.Item);
            Assert.AreNotEqual(result.ItemAs<Link>().Id, 0);
            Assert.IsNotNullOrEmpty(result.ItemAs<Link>().Name);
            Assert.IsTrue(result.IsAuthorized);
            Assert.IsTrue(result.IsAvailable);
        }


        [Test]
        public void CanNotCreateDueToPermissions()
        {
            var settings = new Dictionary<string, object>();
            settings["Link"] = new Dictionary<string, object>();
            settings.GetSection("Link")["Create"] = "*";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));

            var security = new EntitySettingsHelper(settings);
            var helper = new EntityHelper<Link>(security);
            var link = new Link() { Name = "helix cms", Url = "http://helixcms.com", Group = "sites", SortIndex = 2 };
            var result = helper.Create(link);


            // This is a viewmodel
            Assert.AreEqual(result.Success, false);
            Assert.AreNotEqual(result.Message, string.Empty);
            Assert.IsNull(result.Item);
            Assert.IsFalse(result.IsAuthorized);
            Assert.IsTrue(result.IsAvailable);
        }


        [Test]
        public void CanEdit()
        {
            var helper = new EntityHelper<Link>();
            var link = EntityRegistration.GetService<Link>().GetAll()[0];
            link.Name = link.Name + " updated";
            string updatedName = link.Name;
            var result = helper.Edit(link);
            link = helper.Details(link.Id).ItemAs<Link>();

            // This is a viewmodel
            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Message, string.Empty);
            Assert.IsNotNull(result.Item);
            Assert.AreEqual(result.ItemAs<Link>().Id, link.Id);
            Assert.AreEqual(result.ItemAs<Link>().Name, updatedName);
            Assert.IsTrue(result.IsAuthorized);
            Assert.IsTrue(result.IsAvailable);
        }


        [Test]
        public void CanDelete()
        {
            var helper = new EntityHelper<Link>();
            var id = helper.First().ItemAs<Link>().Id;
            var result = helper.Delete(id);

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.Item.GetType(), typeof(Link));
            Assert.AreEqual(result.IsAuthorized, true);
            Assert.IsTrue(result.IsAvailable);
        }


        [Test]
        public void CanGet()
        {
            var helper = new EntityHelper<Link>();
            var link = EntityRegistration.GetService<Link>().GetAll()[0];
            var id = link.Id;
            var result = helper.Details(id);

            Assert.AreEqual(result.Success, true);
            Assert.IsNotNull(result.Item );
            Assert.AreEqual(result.ItemAs<Link>().Id, id);
            Assert.AreEqual(result.ItemAs<Link>().Name, link.Name);
            Assert.AreEqual(result.IsAvailable, true);
            Assert.AreEqual(result.IsAuthorized, true);
        }


        [Test]
        public void CanNotGetUnavailableItem()
        {
            var helper = new EntityHelper<Link>();            
            var result = helper.Details(1000);

            Assert.AreEqual(result.Success, false);
            Assert.IsNull(result.Item);
            Assert.AreEqual(result.IsAvailable, false);
            Assert.AreEqual(result.IsAuthorized, true);
        }


        [Test]
        public void CanNotStartEditOtherUsersItem()
        {
            var helper = new EntityHelper<Link>();
            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "johndoe", "User", "Custom", true)));

            var entity = EntityRegistration.GetService<Link>().First(null);
            var result = helper.Edit(entity.Id);

            Assert.AreEqual(result.Success, false);
            Assert.IsNull(result.Item);
            Assert.AreEqual(result.IsAvailable, true);
            Assert.AreEqual(result.IsAuthorized, false);
        }


        [Test]
        public void CanNotEditOtherUsersItem()
        {
            var helper = new EntityHelper<Link>();
            var link = helper.First().ItemAs<Link>();
            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "johndoe", "User", "Custom", true)));

            var result = helper.Edit(link);

            Assert.AreEqual(result.Success, false);
            Assert.IsNotNull(result.Item);
            Assert.AreEqual(result.IsAvailable, true);
            Assert.AreEqual(result.IsAuthorized, false);
        }


        [Test]
        public void CanNotDeleteOtherUsersItem()
        {
            var helper = new EntityHelper<Link>();
            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "johndoe", "User", "Custom", true)));

            var entity = EntityRegistration.GetService<Link>().First(null);
            var result = helper.Delete(entity.Id);

            Assert.AreEqual(result.Success, false);
            Assert.IsNotNull(result.Item);
            Assert.AreEqual(result.IsAvailable, true);
            Assert.AreEqual(result.IsAuthorized, false);
        }


        [Test]
        public void CanNotCopyOtherUsersItem()
        {
            var helper = new EntityHelper<Link>();
            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "johndoe", "User", "Custom", true)));

            var entity = EntityRegistration.GetService<Link>().First(null);
            var result = helper.Copy(entity.Id);

            Assert.AreEqual(result.Success, false);
            Assert.IsNotNull(result.Item);
            Assert.AreEqual(result.IsAvailable, true);
            Assert.AreEqual(result.IsAuthorized, false);
        }


        [Test]
        public void CanNotCloneOtherUsersItem()
        {
            var helper = new EntityHelper<Link>();
            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "johndoe", "User", "Custom", true)));

            var entity = EntityRegistration.GetService<Link>().First(null);
            var result = helper.Clone(entity.Id);

            Assert.AreEqual(result.Success, false);
            Assert.IsNotNull(result.Item);
            Assert.AreEqual(result.IsAvailable, true);
            Assert.AreEqual(result.IsAuthorized, false);
        }


        [Test]
        public void CanConvertEntityResultToDetailsResult()
        {
            var helper = new EntityHelper<Link>();
            var entityResult = helper.First();
            var result = EntityViewHelper.BuildViewModel<Link>(entityResult, "Link", "details", null);
            Assert.AreEqual(result.ControlPath, "ModelDetails");
            Assert.AreEqual(result.ControllerName, "Link");

            var detailsresult = (EntityDetailsViewModel)result;
            Assert.AreEqual(detailsresult.Entity, entityResult.Item);
        }
    }
}
