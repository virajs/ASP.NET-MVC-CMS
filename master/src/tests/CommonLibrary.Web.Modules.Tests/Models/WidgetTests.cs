using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;

using NUnit.Framework;

using ComLib;
using ComLib.Data;
using ComLib.Entities;
using CommonLibraryNet.Tests.Common;
using ComLib.CMS.Models.Widgets;
using ComLib.Web.Modules.Widgets;

namespace ComLib.CMS.Tests
{
    [TestFixture]
    public class WidgetTests
    {
        /// <summary>
        /// Validate 4 items.
        /// 1. full name too long,
        /// 2. raging too long
        /// 3. email required
        /// 4. size between 10 and 400            
        /// </summary>
        [Test]
        public void CanValidateGravatar()
        {
            var results = new ValidationResults();
            var widget = new Gravatar();

            // Confirm errors.
            widget.FullName = "123456789012345678901234567890123456789012345678901234567890";
            widget.Rating = "too_long";

            bool success = widget.Validate(results);
            Assert.IsFalse(success);
            Assert.AreEqual(results.Count, 6);

            // Confirm correct
            widget.Header = "about";
            widget.Zone = "right";
            widget.FullName = "kishore reddy";
            widget.Rating = "r";
            widget.Email = "kishore@mail.com";
            widget.Size = 80;

            results.Clear();
            success = widget.Validate(results);
            Assert.IsTrue(success);
            Assert.AreEqual(results.Count, 0);
        }


        /// <summary>
        /// Validate the users.
        /// </summary>
        [Test]
        public void CanValidateUsers()
        {
            var results = new ValidationResults();
            var widget = new Users(){ Header = "users", Zone = "left" };

            // Confirm errors.
            widget.NumberOfEntries = 0;
            widget.NumberOfEntriesAcross = 0;
            bool success = widget.Validate(results);

            Assert.IsFalse(success);
            Assert.AreEqual(results.Count, 2);

            results.Clear();
            widget.NumberOfEntries = 20;
            widget.NumberOfEntriesAcross = 4;
            success = widget.Validate(results);
            Assert.IsTrue(success);
            Assert.AreEqual(results.Count, 0);
        }


        /// <summary>
        /// Validate the users.
        /// </summary>
        [Test]
        public void CanValidateBlogRoll()
        {
            var results = new ValidationResults();
            var widget = new BlogRoll() { Header = "users", Zone = "left" };

            // Confirm errors.
            widget.NumberOfEntries = 0;
            widget.BloggerName = "";
            widget.Url = "alkjsdflk";
            widget.Format = "";
            bool success = widget.Validate(results);

            Assert.IsFalse(success);
            Assert.AreEqual(results.Count, 4);

            results.Clear();
            widget.NumberOfEntries = 20;
            widget.BloggerName = "kishore";
            widget.Url = "http://kishore.com/rss";
            widget.Format = "rss";
            success = widget.Validate(results);
            Assert.IsTrue(success);
            Assert.AreEqual(results.Count, 0);
        }


        [Test]
        public void CanLoadState()
        {
            Widget def = new Widget()
            {
                 Name = "Users", Author = "kishore", AuthorUrl = @"http://commonlibrarynetcms.codeplex.com", FullTypeName = "ComLib.CMS.Models.Widgets.Users", Email = "kishore@mail.com", 
                 DeclaringAssembly = "CommonLibrary.Web.Modules", Version = "1.0", IncludeProperties = "Header,Zone,NumberOfEntries,NumberOfEntriesAcross",
                 Url = @"http://commonlibrarynetcms.codeplex.com"
            };
            Widget.Init(new RepositoryInMemory<Widget>(), false);

            Widget.Create(def);
            WidgetInstance.Init(new RepositoryInMemory<WidgetInstance>() { OnRowsMappedCallBack = new Action<IList<WidgetInstance>>(WidgetHelper.ReloadState) }, false);
            var widget = new Users() { DefName = "Users", Header = "users", Zone = "right", NumberOfEntries = 20, NumberOfEntriesAcross = 4 };
            WidgetInstance.Save(widget);

            // Make sure it's saved.
            Assert.IsNotNull(widget.StateData);
            Action clearState = () =>
            {
                widget.Header = "";
                widget.Zone = "";
                widget.NumberOfEntries = 0;
                widget.NumberOfEntriesAcross = 0;
            };
            Action<Users> assertValidateState = (w) =>
            {
                Assert.AreEqual(w.Header, "users");
                Assert.AreEqual(w.Zone, "right");
                Assert.AreEqual(w.NumberOfEntries, 20);
                Assert.AreEqual(w.NumberOfEntriesAcross, 4);
            };
            Action<Func<Users>> checkState = (func) =>
            {
                clearState();
                Users users = func();
                assertValidateState(users);
            };

            // Now load and confirm
            checkState(() => WidgetInstance.Get(1) as Users);
            checkState(() => WidgetInstance.Lookup[1] as Users);
            checkState(() => WidgetInstance.GetAll()[0] as Users);
            checkState(() => WidgetInstance.Find(Query<WidgetInstance>.New().Where(w => w.Header).NotNull())[0] as Users);
        }
    }
}
