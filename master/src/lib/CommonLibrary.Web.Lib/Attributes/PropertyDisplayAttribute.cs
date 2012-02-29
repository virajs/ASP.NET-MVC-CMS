using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Attributes
{  
    /// <summary>
    /// Attribute used to define displayable properties of a widget.
    /// </summary>
    public class PropertyDisplayAttribute : Attribute
    {
        /// <summary>
        /// Application Id.
        /// </summary>
        public int AppId { get; set; }


        /// <summary>
        /// Group this property belongs to.
        /// </summary>
        public string Group { get; set; }


        /// <summary>
        /// Name of the property being referenced.
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// The label to use for this property in an editor.
        /// </summary>
        public string Label { get; set; }
        
        
        /// <summary>
        /// Shorter name to use for display purposes if name is too long.
        /// </summary>
        public string ShortName { get; set; }
        
        
        /// <summary>
        /// Description of the property.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Example of a value.
        /// </summary>
        public string Example { get; set; }


        /// <summary>
        /// DataType of the property.
        /// </summary>
        public string DataType { get; set; }


        /// <summary>
        /// Order number for the property.
        /// </summary>
        public int Order { get; set; }


        /// <summary>
        /// Whether or not the type this property attribute applies to is a basic type.
        /// </summary>
        public bool IsBasicType { get; set; }


        /// <summary>
        /// Default value to supply.
        /// </summary>
        public string DefaultValue { get; set; }


        /// <summary>
        /// Specify the style to display ( "wide" for richtext box. )
        /// </summary>
        public string DisplayStyle { get; set; }
    }
}
