using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Web.Lib.Core
{
    public interface IEntityActivatable
    {
        bool IsActive { get; set; }
    }
}
