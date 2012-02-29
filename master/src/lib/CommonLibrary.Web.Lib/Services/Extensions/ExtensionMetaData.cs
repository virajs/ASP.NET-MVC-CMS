using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Web.Lib.Attributes;

namespace ComLib.Web.Lib.Services
{
    public class ExtensionMetaData
    {
        public string Id;
        public object Instance;       
        public ExtensionAttribute Attribute;
        public List<object> AdditionalAttributes = new List<object>();
        public Type DataType;
    }
}
