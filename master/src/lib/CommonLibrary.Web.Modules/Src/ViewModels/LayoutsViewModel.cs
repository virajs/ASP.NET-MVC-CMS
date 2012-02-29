using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ComLib.Web.Modules.Themes
{
    public class LayoutsViewModel
    {
        public bool CanChangeLayout;
        public string Message;
        public ReadOnlyCollection<Layout> Layouts;
    }
}
