using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Core
{
    public class MenuNode
    {
        /// <summary>
        /// Default initialization.
        /// </summary>
        public MenuNode()
        {
        }


        /// <summary>
        /// Initialize the menunode.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="isRolesEnabled"></param>
        /// <param name="roles"></param>
        /// <param name="navigateUrl"></param>
        public MenuNode(string name, string text = "", bool isRolesEnabled = false, string roles = "", string navigateUrl = "")
        {
            Name = name;
            Text = string.IsNullOrEmpty(text) ? name : text;
            IsRolesEnabled = isRolesEnabled;
            Roles = roles;
            NavigateUrl = navigateUrl;
        }


        /// <summary>
        /// The name of the menu used as an identifier.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The text to display for the menu.
        /// </summary>
        public string Text { get; set; }


        /// <summary>
        /// Tooltip for the menu.
        /// </summary>
        public string ToolTip { get; set; }


        /// <summary>
        /// Optional value to represent this menu node.
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// Whether or not this is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }


        /// <summary>
        /// Whether or not the menu node is selectable /clickable.
        /// </summary>
        public bool IsSelectable { get; set; }


        /// <summary>
        /// Whether or not this is enabled based on roles.
        /// </summary>
        public bool IsRolesEnabled { get; set; }


        /// <summary>
        /// The roles associated w/ this menu item.
        /// </summary>
        public string Roles { get; set; }


        /// <summary>
        /// Url for an image.
        /// </summary>
        public string ImageUrl { get; set; }


        /// <summary>
        /// A navigation url.
        /// </summary>
        public string NavigateUrl { get; set; }


        /// <summary>
        /// The parent menu node.
        /// </summary>
        public MenuNode Parent { get; set; }

        
        /// <summary>
        /// The target of the onclick
        /// </summary>
        public string Target { get; set; }


        /// <summary>
        /// Whether or not this is actually an instance of a menu.
        /// </summary>
        public virtual bool IsMenu
        {
            get { return this is Menu; }
        }


        /// <summary>
        /// Convert this instance to a Menu ( upcast )
        /// </summary>
        public virtual Menu AsMenu
        {
            get { if (!IsMenu ) return null; return this as Menu; }
        }
    }



    /// <summary>
    /// Menu with collection of menu nodes.
    /// </summary>
    public class Menu : MenuNode
    {
        /// <summary>
        /// Default initialization.
        /// </summary>
        public Menu()
        {
        }


        /// <summary>
        /// Initialize the menunode.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="isRolesEnabled"></param>
        /// <param name="roles"></param>
        /// <param name="navigateUrl"></param>
        public Menu(string name, string text = "", bool isRolesEnabled = false, string roles = "", string navigateUrl = "")
        {
            Name = name;
            Text = string.IsNullOrEmpty(text) ? name : text;
            IsRolesEnabled = isRolesEnabled;
            Roles = roles;
            NavigateUrl = navigateUrl;
        }

        public List<MenuNode> Items = new List<MenuNode>();
    }
}
