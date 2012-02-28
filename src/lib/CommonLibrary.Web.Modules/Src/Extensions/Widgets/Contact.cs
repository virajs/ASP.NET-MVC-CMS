using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Entities;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Modules.Links;
using ComLib.Web.Modules.Widgets;


namespace ComLib.CMS.Models.Widgets
{

    /// <summary>
    /// Links widget
    /// </summary>
    [Widget(Name = "Contact", IsCachable = true, IsEditable = true, SortIndex = 5, Path = "Widgets/Contact")]
    public class Contact : WidgetInstance
    {
        /// <summary>
        /// Initialize
        /// </summary>
        public Contact()
        {
            Country = "USA";
            IsLocationApplicable = true;
        }
        

        /// <summary>
        /// Gets or sets the ContactName.
        /// </summary>
        /// <value>The name of the ContactName.</value>
        [PropertyDisplay(Label = "Contact Header", Order = 1, DefaultValue = "", Description = "A header for the contact info")]
        public string ContactHeader { get; set; }


        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        /// <value>The name of the Url.</value>
        [PropertyDisplay(Order = 2, DefaultValue = "", Description = "A url for the contact info")]
        public string Url { get; set; }


        /// <summary>
        /// Gets or sets the ContactName.
        /// </summary>
        /// <value>The name of the ContactName.</value>
        [PropertyDisplay(Label = "Contact Name", Order = 3, DefaultValue = "", Description = "Name of contact")]
        public string ContactName { get; set; }


        /// <summary>
        /// Gets or sets the contact Email.
        /// </summary>
        /// <value>The name of the Email.</value>
        [PropertyDisplay(Order = 4, DefaultValue = "", Description = "Email of contact")]
        public string Email { get; set; }


        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        /// <value>The name of the Phone.</value>
        [PropertyDisplay(Order = 5, DefaultValue = "", Description = "Phone number of contact")]
        public string Phone { get; set; }


        /// <summary>
        /// Gets or sets the Fax.
        /// </summary>
        /// <value>The name of the Fax.</value>
        [PropertyDisplay(Order = 6, DefaultValue = "", Description = "A fax for the contact")]
        public string Fax { get; set; }


        /// <summary>
        /// Gets or sets the street.
        /// </summary>
        /// <value>The name of the street.</value>
        [PropertyDisplay(Order = 8, DefaultValue = "", Description = "The street part of the address")]
        public string Street { get; set; }


        /// <summary>
        /// Gets or sets the City.
        /// </summary>
        /// <value>The name of the City.</value>
        [PropertyDisplay(Order = 9, DefaultValue = "", Description = "The city part of the address.")]
        public string City { get; set; }


        /// <summary>
        /// Gets or sets the Zip.
        /// </summary>
        /// <value>The name of the City.</value>
        [PropertyDisplay(Order = 10, DefaultValue = "", Description = "The zipcode of the address")]
        public string Zip { get; set; }


        /// <summary>
        /// Gets or sets the State.
        /// </summary>
        /// <value>The name of the State.</value>
        [PropertyDisplay(Order = 11, DefaultValue = "", Description = "State part of the address")]
        public string State { get; set; }


        /// <summary>
        /// Gets or sets the Country.
        /// </summary>
        /// <value>The name of the Country.</value>
        [PropertyDisplay(Order = 12, DefaultValue = "", Description = "Country part of the address")]
        public string Country { get; set; }


        /// <summary>
        /// Whether or not the location(street,city, state, country, zip ) is applicable.
        /// </summary>
        [PropertyDisplay(Label = "Is Location Applicable", Order = 13, DefaultValue = "", Description = "Whether or not the location is appcliable")]
        public bool IsLocationApplicable { get; set; }
    }
}
