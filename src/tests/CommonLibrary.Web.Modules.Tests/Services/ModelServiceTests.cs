using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;
using ComLib.Web.Templating;
using ComLib.Web.Lib.Core;
using ComLib.Web.Modules.Services;
using ComLib.Web.Lib.Attributes;

using ComLib.Web.Modules.Resources;
using ComLib.Web.Modules.OptionDefs;

namespace ComLib.Web.Lib.Services.Macros.Tests
{
    
    [TestFixture]
    public class ModelServiceTests
    {
        public class Person
        {
            [PropertyDisplay( Label = "First Name", Description = "First name of person", Example = "John" )]
            public string Name { get; set; }

            [PropertyDisplay(Label = "Current Age", Description = "Age of person", Example = "25")]
            public int Age { get; set; }

            [PropertyDisplay(Label = "Male?", Description = "Male / Female", Example = "true/false")]
            public bool IsMale { get; set; }

            [PropertyDisplay(Label = "Birthday", Description = "Birthdate of person", Example = "MM/DD/YYYY")]
            public DateTime BirthDate { get; set; }

            [PropertyDisplay(Label = "Salary", Description = "Salary of person", Example = "35.5")]
            public float Salary { get; set; }
        }


        [Test]
        public void CanRegisterUsingAttributes()
        {
            var service = new ModelService2();
            service.Register<Person>();
            service.Get<Person>().Init(new List<OptionDef>
            {
                new OptionDef { Key = "FirstName", ValType = "string",  IsRequired = true, SortIndex = 1, IsBasicType = true },
                new OptionDef { Key = "Age",       ValType = "int",     IsRequired = true, SortIndex = 2, IsBasicType = true },
                new OptionDef { Key = "IsMale",    ValType = "bool",    IsRequired = true, SortIndex = 3, IsBasicType = true },
                new OptionDef { Key = "BirthDate", ValType = "date",    IsRequired = true, SortIndex = 4, IsBasicType = true },
                new OptionDef { Key = "Salary",    ValType = "number",  IsRequired = true, SortIndex = 5, IsBasicType = true },
            },
            new List<Resource>()
            {
                new Resource { Key = "FirstName", Language = "en", Section = "Resource", Name = "First Name", Example = "john", Description = "The users first name" },
                new Resource { Key = "Age",       Language = "en", Section = "Resource", Name = "Age", Example = "27", Description = "The users age" },
                new Resource { Key = "IsMale",    Language = "en", Section = "Resource", Name = "Is Male", Example = "true|false|yes|no", Description = "The users sex" },
                new Resource { Key = "BirthDate", Language = "en", Section = "Resource", Name = "Birthday", Example = "MM/DD/YYYY", Description = "The users birthday" },
                new Resource { Key = "Salary",    Language = "en", Section = "Resource", Name = "Moolah", Example = "80.5k", Description = "The users salary" }
            });
            var label = service.Get<Person>().Label("FirstName");
            var desc = service.Get<Person>().Description("FirstName");
            var example = service.Get<Person>().Example("FirstName");

            Assert.AreEqual("First Name", label);
            Assert.AreEqual("First name of person", desc);
            Assert.AreEqual("John", example);
        }
    }
}
