using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Web.Lib.Core;
using NUnit.Framework;


namespace ComLib.CMS.Tests
{
    [TestFixture]
    public class MenuTests
    {
        
        private Menu GetTestMenu()
        {

            var menu = new Menu() { Name = "models", Text = "Models", IsRolesEnabled = true, Roles = "*", Value = "models", Items = new List<MenuNode> {                
                    new Menu { Name = "System", Text = "System", IsRolesEnabled = true, Roles = "Admin", Value = "system",  Items = new List<MenuNode> {
                            new MenuNode{ Name = "System", Text = "System",   IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/admin/console/getinfo?name=System" },
                            new MenuNode{ Name = "StartUp", Text = "StartUp", IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/admin/console/getinfo?name=Startup" },
                            new MenuNode{ Name = "Widgets", Text = "Widgets", IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/admin/console/getinfo?name=Widgets" } ,
                            new MenuNode{ Name = "Cache", Text = "Cache",     IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/admin/cache/index" },
                        }
                    },
                    new Menu { Name = "Appearence", Text = "Appearance", IsRolesEnabled = true, Roles = "Admin", Value = "appearence", Items = new List<MenuNode> {
                            new MenuNode{ Name = "Themes",  Text = "Themes",  IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/theme/manage" },
                            new MenuNode{ Name = "Layouts", Text = "Layouts", IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/theme/layouts" },
                            new MenuNode{ Name = "Css",     Text = "Css",     IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/theme/EditCss" } ,
                            new MenuNode{ Name = "Widges",  Text = "Widgets", IsRolesEnabled = true, Roles = "Admin", NavigateUrl = "/widget/manage"},
                        }
                    },
                    new Menu { Name = "Models", Text = "Models", IsRolesEnabled = true, Roles = "*", Value = "models", Items = new List<MenuNode> {
                            new Menu { Name = "Post", Text = "Post", IsRolesEnabled = true, Roles = "Admin", Items = new List<MenuNode> {
                                    new MenuNode{ Name = "Add",     Text = "Add",    IsRolesEnabled = true, Roles = "Admin", Value = "post_add" },
                                    new MenuNode{ Name = "Manage",  Text = "Manage", IsRolesEnabled = true, Roles = "Admin", Value = "post_manage" },
                                }
                            },
                            new Menu{ Name = "Event", Text = "Event", IsRolesEnabled = true, Roles = "*", Items = new List<MenuNode> {
                                    new MenuNode{ Name = "Add",     Text = "Add",    IsRolesEnabled = true, Roles = "*", Value = "event_add" },
                                    new MenuNode{ Name = "Manage",  Text = "Manage", IsRolesEnabled = true, Roles = "*", Value = "event_manage" },
                                }
                            }
                        }
                    }
                }
            };
            return menu;
        }



        [Test]
        public void CanLoadMenu()
        {
            var menu = GetTestMenu();

            Assert.IsTrue(menu.Items[2].IsMenu);
            Assert.AreEqual(menu.Items[2].AsMenu.Items[0].Name, "Post");

        }
    }
}
