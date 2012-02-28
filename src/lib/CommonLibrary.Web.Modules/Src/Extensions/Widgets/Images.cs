using System;
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
    public class Images : WidgetInstance
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public Images()
        {
            MaxHeight = 100;
            MaxWidth = 100;
            DisplayImageTitle = true;
            IsFolderMode = true;
            FolderName = "samples";
            MaxImagesToDisplay = 5;
        }


        /// <summary>
        /// MaxHeight of each image.
        /// </summary>
        public int MaxHeight { get; set; }


        /// <summary>
        /// Max width of each image.
        /// </summary>
        public int MaxWidth { get; set; }


        /// <summary>
        /// Whether or not to display the title of each image if there is one.
        /// </summary>
        public bool DisplayImageTitle { get; set; }


        /// <summary>
        /// The name of the folder associated w/ the images that should be displayed.
        /// </summary>
        public string FolderName { get; set; }


        /// <summary>
        /// Whether or not displaying of images is based on the folder the images are located in or by association w/ an entity.
        /// </summary>
        public bool IsFolderMode { get; set; }


        /// <summary>
        /// The name of the entity asscoiated w/ the images if displaying image by entity.
        /// </summary>
        public string EntityGroup { get; set; }


        /// <summary>
        /// The id of the entity associated w/ the images if displaying images by entity.
        /// </summary>
        public int EntityRefId { get; set; }


        /// <summary>
        /// The maximum number of images to display.
        /// </summary>
        public int MaxImagesToDisplay { get; set; }

    }
}
