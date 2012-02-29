using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Tags;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Tags Widget
    /// </summary>
    [Widget(Name = "Categories", IsCachable = true, IsEditable = true, SortIndex = 5, Path = "Widgets/Categories")]
    public class Categories : WidgetInstance
    {
        /// <summary>
        /// The name of the group that these tags apply to. E.g. BlogPosts will be 1. Events will be 2.
        /// </summary>
        [Required]
        [PropertyDisplay(Order = 1, DefaultValue = "", Description = "The group of the categories to load")]
        public string Group { get; set; }


        /// <summary>
        /// Gets or sets the name of the entity that this set of tags apply to.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [Required]
        [PropertyDisplay(Label = "Entity Name", Order = 2, DefaultValue = "", Description = "The name of the entity")]
        public string EntityName { get; set; }


        /// <summary>
        /// Gets or sets the number of categories to display. This is not-applicable if "ShowAllCategories" is enabled.
        /// </summary>
        /// <value>The number of entries.</value>
        [Range(1, 400)]
        [PropertyDisplay(Label = "Number To Display", Order = 3, DefaultValue = "", Description = "Total number of categories to display")]
        public int NumberToDisplay { get; set; }


        /// <summary>
        /// Whether or not to show all categories.
        /// </summary>
        [PropertyDisplay(Label = "Show All Categories", Order = 4, DefaultValue = "", Description = "Whether or not to show all categories")]
        public bool ShowAllCategories { get; set; }


        /// <summary>
        /// link to page that can be used by user to request a new category.
        /// </summary>
        [PropertyDisplay(Label = "Request Link", Order = 5, DefaultValue = "", Description = "Url of link to page for user to request a new category")]
        public string RequestLink { get; set; }


        /// <summary>
        /// Whether or not this can render html itself or uses a control to do it.
        /// </summary>
        /// <value></value>
        public override bool IsSelfRenderable
        {
            get { return false; }
        }    
    }
}
