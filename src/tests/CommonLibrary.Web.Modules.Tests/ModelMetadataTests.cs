using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Resources;
using NUnit.Framework;
using System.Linq;
using System.Data;

using ComLib;
using ComLib.Entities;
using ComLib.IO;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Modules.Links;
using ComLib.Web.Lib.Models;
using ComLib.Authentication;

namespace CommonLibrary.Tests
{   
    [TestFixture]
    public class ModelMetaDataTests
    {
        [Test]
        public void CanGetModels()
        {
            var models = ModelHelper.LoadFromAssembly("CommonLibrary.Web.Modules");
            var defs = (from m in models where m.Key.Name == "Link" select m).ToList();

            Assert.IsNotNull(defs);
            Assert.AreEqual(defs.Count, 1);
        }
    }
}
