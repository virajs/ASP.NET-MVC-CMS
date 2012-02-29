using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Services;



namespace ComLib.Web.Lib.Services.Tests
{
    [TestFixture]
    public class DashboardServiceTests
    {
        private DashboardService _service;

        public DashboardServiceTests()
        {
            _service = new DashboardService();
            _service.AdminRole = "Admin";
            _service.Sections.AddRange(new List<Menu>()
            {
                new Menu("System", isRolesEnabled: true, roles: "Admin") { Items = new List<MenuNode> {
                        new MenuNode("Summary",     navigateUrl: "/admin/console/getinfo?name=System"),
                        new MenuNode("Startup",     navigateUrl: "/admin/console/getinfo?name=Startup"),
                        new MenuNode("Widgets",     navigateUrl: "/admin/console/getinfo?name=Widgets"),
                        new MenuNode("Cache",       navigateUrl: "/admin/cache/index"),
                        new MenuNode("Logs",        navigateUrl: "/admin/log/index"),
                        new MenuNode("Users",       navigateUrl: "/admin/user/manage"),
                        new MenuNode("Diagnostics", navigateUrl: "/admin/diagnostics/index"),
                        new MenuNode("Email",       navigateUrl: "/admin/console/email"),
                        new MenuNode("Queues",      navigateUrl: "/admin/queue/index"),
                        new MenuNode("Tasks",       navigateUrl: "/admin/task/index"),
                        new MenuNode("Flags",       navigateUrl: "/flag/manage")
                    }
                },
                new Menu("Settings", isRolesEnabled: true, roles: "Admin"),
                new Menu("Appearence", isRolesEnabled: true, roles: "Admin" ) { Items = new List<MenuNode>{
                        new MenuNode("Themes",      navigateUrl: "/theme/manage"),
                        new MenuNode("Layouts",     navigateUrl: "/theme/layouts"),
                        new MenuNode("Css",         navigateUrl: "/theme/EditCss"),
                        new MenuNode("Widgets",     navigateUrl: "/widget/manage")
                    }
                },
                new Menu("Media", isRolesEnabled: true, roles: "Admin" ) { Items = new List<MenuNode> {
                        new MenuNode("Manage",      navigateUrl: "/mediafolder/manage"),
                    }
                },
                new Menu("Location", isRolesEnabled: true, roles: "Admin") { Items = new List<MenuNode> {
                        new MenuNode("City",        navigateUrl:  "/admin/city/manage"),
                        new MenuNode("State",       navigateUrl:  "/admin/state/manage"),
                        new MenuNode("Country",     navigateUrl:  "/admin/country/manage")
                    }
                },
                new Menu("Misc", isRolesEnabled: true, roles: "*") { Items = new List<MenuNode> {
                        new MenuNode("Favorites",   isRolesEnabled: true, roles: "*",     navigateUrl: "/favorite/manage"),
                        new MenuNode("Feedback",    isRolesEnabled: true, roles: "Admin", navigateUrl: "/feedback/manage"),
                        new MenuNode("Comments",    isRolesEnabled: true, roles: "*",     navigateUrl: "/comment/manage")
                    }
                },
                new Menu("Tools", isRolesEnabled: true, roles: "*") { Items = new List<MenuNode> {
                        new MenuNode("Import",      navigateUrl: "/tools/import"),
                        new MenuNode("Export",      navigateUrl: "/tools/export")
                    }
                }
            });
        }


        [Test]
        public void CanGetAdminSections()
        {
            var menus = _service.GetSections(AuthType.Admin, null);
            Assert.AreEqual(menus.Count, 5); 
            Assert.IsNotNull(_service.AdminSections);
            Assert.AreEqual(_service.AdminSections.Count, 5);            
        }



        [Test]
        public void CanGetAuthenticatedSections()
        {
            var menus = _service.GetSections(AuthType.Authenticated, null);
            Assert.AreEqual(menus.Count, 2);
            Assert.IsNotNull(_service.AuthenticatedSections);
            Assert.AreEqual(_service.AuthenticatedSections.Count, 2);            
        }
    }
}
