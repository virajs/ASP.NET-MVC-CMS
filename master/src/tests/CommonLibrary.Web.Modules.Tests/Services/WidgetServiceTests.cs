using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;
using ComLib.Web.Templating;
using ComLib.Web.Lib.Core;
using ComLib.Web.Modules.Widgets;



namespace ComLib.Web.Lib.Services.Macros.Tests
{
    
    [TestFixture]
    public class WidgetServiceTests
    {
        [Test]
        public void CanLoad()
        {
            var service = new WidgetService();
            service.Load("CommonLibrary.Web.Modules.Tests");
            var links = service.Create("Links");
        }
    }
}
