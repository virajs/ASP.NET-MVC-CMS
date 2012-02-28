using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Feeds;
using ComLib.Entities;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// BlogRoll Widget
    /// </summary>
    [Widget(Name = "GeoMap", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class GeoMap : WidgetInstance
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public GeoMap()
        {
            Name = "map1";
            Provider = "bing";
            Height = 120;
            Width = 120;
        }


        /// <summary>
        /// Name for map instance.
        /// </summary>
        [PropertyDisplay(Order = 1, DefaultValue = "", Description = "The name of the map, used when having multiple maps.")]
        public string Name { get; set; }


        /// <summary>
        /// "bing", "google"
        /// </summary>
        [Required]
        [PropertyDisplay(Order = 2, DefaultValue = "", Description = "Either 'bing' or 'google'")]
        public string Provider { get; set; }


        /// <summary>
        /// The full address in one line separated by comma to represent the address the show.
        /// </summary>
        [Required]
        [PropertyDisplay(Label = "Full Address", Order = 3, DefaultValue = "", Description = "The address to show ( separate parts of address by comma )")]
        public string FullAddress { get; set; }


        /// <summary>
        /// Height of the map
        /// </summary>
        [Range(30, 800)]
        [PropertyDisplay(Order = 4, DefaultValue = "120", Description = "The height of the map")]
        public int Height { get; set; }


        /// <summary>
        /// Width of the map
        /// </summary>
        [Range(30, 800)]
        [PropertyDisplay(Order = 5, DefaultValue = "120", Description = "The width of the map")]
        public int Width { get; set; }


        /// <summary>
        /// Some textual content for the map supplied by user.
        /// </summary>
        public string Content { get; set; }


        public override bool IsSelfRenderable
        {
            get
            {
                return true;
            }
        }


        public override string Render()
        {
            return "geomap";
        }
    }
}
