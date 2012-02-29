using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Reflection;
using ComLib.Web.Templating;


namespace ComLib.Web.Lib.Services.Macros
{
    /// <summary>
    /// Interface for a macro
    /// </summary>
    public interface IMacro
    {
        string Process(Tag tag);
    }
}
