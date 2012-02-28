using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Configuration;
using ComLib.EmailSupport;


namespace ComLib.Web.Lib.Settings
{
    /// <summary>
    /// Site / General settings.
    /// </summary>
    public class EmailSettings : ConfigSourceDynamic, IEmailSettings
    {
        /// <summary>
        /// Set default settings.
        /// </summary>
        public EmailSettings()
        {
        }


        /// <summary>
        /// Whether or not enable emails.
        /// </summary>
        public bool EnableEmails { get; set; }


        /// <summary>
        /// Gets or sets the SMPT server.
        /// </summary>
        /// <value>The SMPT server.</value>
        public string SmptServer { get; set; }


        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>From.</value>
        public string From { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether [use port].
        /// </summary>
        /// <value><c>true</c> if [use port]; otherwise, <c>false</c>.</value>
        public bool UsePort { get; set; }


        /// <summary>
        /// Gets or sets the name of the authentication user.
        /// </summary>
        /// <value>The name of the authentication user.</value>
        public string AuthenticationUserName { get; set; }


        /// <summary>
        /// Gets or sets the authentication password.
        /// </summary>
        /// <value>The authentication password.</value>
        public string AuthenticationPassword { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this instance is authentication required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is authentication required; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticationRequired { get; set; }


        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>The port.</value>
        public int Port { get; set; }
    }
}
