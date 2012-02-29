using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

using NUnit.Framework;

using ComLib;
using ComLib.Account;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Authentication;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Services;
using ComLib.Web.Modules.Services;
using ComLib.Web.Modules.Profiles;


namespace ComLib.Web.Lib.Services.Tests
{
    
    [TestFixture]
    public class MembershipServiceTests
    {
        private IMembershipService _service;


        public MembershipServiceTests()
        {
            _service = new MembershipService { EnableNotifications = false };

            UserSettings settings = new UserSettings();
            settings.UserNameRegEx = @"[a-zA-Z1-9\._]{3,15}";
            settings.PasswordRegEx = "[a-zA-Z1-9]{5,15}"; 
            User.Init(new UserService(new RepositoryInMemory<User>(), new UserValidator(), settings));
            Profile.Init(new RepositoryInMemory<Profile>(), false);
        }



        [Test]
        public void CanCreate()
        {
            CreateUser("admin", "password_a", "admin@abc.com", "Admin");

            // Now retrieve.
            var profile = Profile.FindUser("admin");
            var user = User.Get(profile.UserId);
            Assert.IsNotNull(profile);
            Assert.IsNotNull(user);
            Assert.AreEqual(user.Roles, "Admin");
            Assert.AreEqual(profile.UserName, user.UserName);            
        }


        [Test]
        public void CanLogin()
        {
            string roles = "";
            CreateUser("admin", "password_a", "admin@abc.com", "Admin");
            var result = _service.ValidateUser("admin", "password_a", ref roles);
            Assert.IsFalse(_service.IsValidateUser("admin", "abcdefghi").Success);
            Assert.IsTrue(result.Success);
            Assert.AreEqual(roles, "Admin");
        }


        [Test]
        public void CanChangePassword()
        {
            CreateUser("admin", "password_a", "admin@abc.com", "Admin");
            bool changed = _service.ChangePassword("admin", "password_a", "password_b");
            Assert.IsTrue(changed);
            Assert.IsFalse(_service.IsValidateUser("admin", "password_a").Success);
            Assert.IsTrue(_service.IsValidateUser("admin", "password_b").Success);            
        }


        [Test]
        public void CanNotCreateDuplicateUser()
        {
            CreateUser("admin", "password_a", "admin@abc.com", "Admin");
            var result = CreateUser("admin", "password_a", "admin2@abc.com", "Admin", false);
            Assert.AreEqual(result, MembershipCreateStatus.DuplicateUserName);
        }


        [Test]
        public void CanNotCreateDuplicateEmail()
        {
            CreateUser("admin", "password_a", "admin@abc.com", "Admin");
            var result = CreateUser("admin2", "password_a", "admin@abc.com", "Admin", false);
            Assert.AreEqual(result, MembershipCreateStatus.DuplicateEmail);
        }


        private MembershipCreateStatus CreateUser(string user, string password, string email, string roles, bool assert = true)
        {
            MembershipCreateStatus status = MembershipCreateStatus.ProviderError;
            var result = _service.CreateUser(user, password, email, roles, ref status);
            if (assert)
            {
                Assert.AreEqual(status, MembershipCreateStatus.Success);
                Assert.IsTrue(result.Success);
            }
            return status;
        }
    }
}
