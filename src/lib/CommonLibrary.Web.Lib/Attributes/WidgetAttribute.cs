using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Attributes
{  
    /// <summary>
    /// Attribute used to define a widget.
    /// </summary>
    public class WidgetAttribute : ExtensionAttribute
    {
        /// <summary>
        /// Default values.
        /// </summary>
        public WidgetAttribute()
        {
            IsEditable = true;
        }

        
        /// <summary>
        /// Path to .ascx control if this widget does not render itself.
        /// </summary>
        public string Path { get; set; }

        
        /// <summary>
        /// Path to .ascx control if this widget does not render itself.
        /// </summary>
        public string PathToEditor { get; set; }


        /// <summary>
        /// Whether or not this widget is editable.
        /// </summary>
        public bool IsEditable { get; set; }


        /// <summary>
        /// Whether or not this widget is cacheable.
        /// </summary>
        public bool IsCachable { get; set; }


        /// <summary>
        /// The sort index used to sort the widget in zones.
        /// </summary>
        public int SortIndex { get; set; }


        /// <summary>
        /// The comma delimited list of properties that should be included for persistance of the widget instance 
        /// into the underlying datastore.
        /// </summary>
        public string IncludeProperties { get; set; }
    }
}
