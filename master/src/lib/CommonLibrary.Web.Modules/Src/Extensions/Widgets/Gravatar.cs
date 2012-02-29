using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Gravitar Widget
    /// </summary>
    [Widget(Name = "Gravatar", IsCachable = true, IsEditable = true, SortIndex = 5)]
    public class Gravatar : WidgetInstance
    {
        /// <summary>
        /// Gets or sets the name of the blogger.
        /// </summary>
        /// <value>The name of the blogger.</value>
        [StringLength(50)]
        [PropertyDisplay(Label = "Full Name", Order = 1, DefaultValue = "", Description = "The full name to display")]
        public string FullName { get; set; }


        /// <summary>
        /// Rating based image to get. e.g. "g". for all audiences.
        /// </summary>
        [StringLength(2)]
        [PropertyDisplay(Order = 2, DefaultValue = "g", Description = "The rating to enforce( g, etc )")]
        public string Rating { get; set; }


        /// <summary>
        /// Size of the image to get.
        /// </summary>
        [Range(10, 400)] 
        [PropertyDisplay(Order = 3, DefaultValue = "80", Description = "The size of the gravatar in pixels")]
        public int Size { get; set; }


        /// <summary>
        /// Extension for the image.
        /// </summary>
        [PropertyDisplay(Label = "Image Extension", Order = 4, DefaultValue = "", Description = "The image extension to use")]
        public string ImageExtension { get; set; }


        /// <summary>
        /// Email to use for gravatar.
        /// </summary>
        [Required]
        [RegularExpression(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$", ErrorMessage = "Email is invalid")]
        [PropertyDisplay(Name = "Email", Order = 5, DefaultValue = "", Description = "The email associated with the gravatar")]
        public string Email { get; set; }


        /// <summary>
        /// Descriptive text about the person represented by this gravatar.
        /// </summary>       
        [PropertyDisplay(Order = 6, DefaultValue = "", Description = "Optional content to display.")]
        public string About { get; set; }


        /// <summary>
        /// Whether or not this can render html itself or uses a control to do it.
        /// </summary>
        /// <value></value>
        public override bool IsSelfRenderable
        {
            get { return true; }
        }


        /// <summary>
        /// Renders this instance as Html.
        /// </summary>
        /// <returns></returns>
        public override string Render()
        {
            var gravatar = new ComLib.Web.Services.GravatarSupport.Gravatar(Email, Size, Rating, string.Empty, ImageExtension); 
            var buffer = new StringBuilder();

            // Heading FullName
            buffer.Append("<div class=\"wtitle\">" + HttpUtility.HtmlEncode(FullName) + "</div>" + Environment.NewLine);
            buffer.Append("<p>" + Environment.NewLine);

            // Image and content.
            string content = "<img src=\"" +gravatar.Url + "\" alt=\"gravatar\" align=\"left\" style=\"padding: 2px 5px 5px 2px\"/>";
            content += About;
            buffer.Append(content);

            buffer.Append("</p>" + Environment.NewLine);
            string html = buffer.ToString();
            return html;
        }
    }
}
