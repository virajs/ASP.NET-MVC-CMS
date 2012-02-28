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
using ComLib.IO;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Modules.Links;
using ComLib.Web.Lib.Models;
using ComLib.Authentication;

namespace CommonLibrary.Tests
{   
    [TestFixture]
    public class EntitySecurityHelperTests
    {
        

        [Test]
        public void GuestOk()
        {            
            var settings = new Dictionary<string, object>();
            var helper = new EntitySettingsHelper(settings);
            settings["Article"] = new Dictionary<string, object>();
            settings.Section("Article")["Create"] = "?";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));
            Assert.IsTrue(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", true)));
            Assert.IsTrue(helper.HasAccessTo("Article", "Create"));
        }


        [Test]
        public void MustBeLoggedIn()
        {            
            var settings = new Dictionary<string, object>();
            var helper = new EntitySettingsHelper(settings);
            settings["Article"] = new Dictionary<string, object>();
            settings.Section("Article")["Create"] = "*";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));
            Assert.IsFalse(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", true)));
            Assert.IsTrue(helper.HasAccessTo("Article", "Create"));
        }


        [Test]
        public void MustBeInRoles()
        {
            var settings = new Dictionary<string, object>();
            var helper = new EntitySettingsHelper(settings);
            settings["Article"] = new Dictionary<string, object>();
            settings.Section("Article")["Create"] = "moderator";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));
            Assert.IsFalse(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", true)));
            Assert.IsFalse(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "moderator", "custom", true)));
            Assert.IsTrue(helper.HasAccessTo("Article", "Create"));
        }


        [Test]
        public void GuestOkInherited()
        {
            var settings = new Dictionary<string, object>();
            var helper = new EntitySettingsHelper(settings);
            settings["CommonPermissions"] = new Dictionary<string, object>();
            settings.Section("CommonPermissions")["Create"] = "?";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));
            Assert.IsTrue(helper.HasAccessTo("Feedback", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", true)));
            Assert.IsTrue(helper.HasAccessTo("Feedback", "Create"));
        }


        [Test]
        public void MustBeLoggedInInherited()
        {
            var settings = new Dictionary<string, object>();
            var helper = new EntitySettingsHelper(settings);
            settings["CommonPermissions"] = new Dictionary<string, object>();
            settings.Section("CommonPermissions")["Create"] = "*";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));
            Assert.IsFalse(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", true)));
            Assert.IsTrue(helper.HasAccessTo("Article", "Create"));
        }


        [Test]
        public void MustBeInRolesInherited()
        {
            var settings = new Dictionary<string, object>();
            var helper = new EntitySettingsHelper(settings);
            settings["CommonPermissions"] = new Dictionary<string, object>();
            settings.Section("CommonPermissions")["Create"] = "moderator";

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", false)));
            Assert.IsFalse(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "normaluser", "custom", true)));
            Assert.IsFalse(helper.HasAccessTo("Article", "Create"));

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "moderator", "custom", true)));
            Assert.IsTrue(helper.HasAccessTo("Article", "Create"));
        }


        [Test]
        public void CanUseModelPermissions()
        {   
            var permissionsMap = new Dictionary<Type, ModelSettings>();
            var permissions = new ModelPermissionSettings()
            {
                RolesForCreate = "moderator", RolesForDelete = "moderator", RolesForImport = "moderator", 
                RolesForView =  "user", RolesForIndex = "user", RolesForManage = "user"
            };
            permissionsMap[typeof(Link)] = new ModelSettings()  { Permissions = permissions };

            var securityHelper = new EntitySettingsHelper();
            securityHelper.Init(permissionsMap);

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "moderator", "custom", true)));
            Assert.IsTrue(securityHelper.HasAccessToCreate<Link>());
            Assert.IsTrue(securityHelper.HasAccessToDelete<Link>());
            Assert.IsTrue(securityHelper.HasAccessToImport<Link>());
            Assert.IsFalse(securityHelper.HasAccessToIndex<Link>());
            Assert.IsFalse(securityHelper.HasAccessToManage<Link>());
            Assert.IsFalse(securityHelper.HasAccessToView<Link>());

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "user", "custom", true)));
            Assert.IsFalse(securityHelper.HasAccessToCreate<Link>());
            Assert.IsFalse(securityHelper.HasAccessToDelete<Link>());
            Assert.IsFalse(securityHelper.HasAccessToImport<Link>());
            Assert.IsTrue(securityHelper.HasAccessToIndex<Link>());
            Assert.IsTrue(securityHelper.HasAccessToManage<Link>());
            Assert.IsTrue(securityHelper.HasAccessToView<Link>());
        }


        [Test]
        public void CanNotUseModelPermissions()
        {
            var permissionsMap = new Dictionary<Type, ModelSettings>();
            var permissions = new ModelPermissionSettings()
            {
                RolesForCreate = "Admin",
                RolesForView = "Admin",
                RolesForIndex = "Admin",
                RolesForDelete = "Admin",
                RolesForManage = "Admin",
                RolesForImport = "Admin"
            };
            permissionsMap[typeof(Link)] = new ModelSettings()
            {
                Permissions = permissions  
            };

            var securityHelper = new EntitySettingsHelper();
            securityHelper.Init(permissionsMap);

            Auth.Init(new AuthWin("Admin", new UserPrincipal(1, "kishore", "moderator", "custom", true)));

            // Check everything is false.
            Assert.IsFalse(securityHelper.HasAccessToCreate<Link>());
            Assert.IsFalse(securityHelper.HasAccessToDelete<Link>());
            Assert.IsFalse(securityHelper.HasAccessToImport<Link>());
            Assert.IsFalse(securityHelper.HasAccessToIndex<Link>());
            Assert.IsFalse(securityHelper.HasAccessToManage<Link>());
            Assert.IsFalse(securityHelper.HasAccessToView<Link>());
        }
    }
}
