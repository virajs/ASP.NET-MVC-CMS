using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.IO;

using ComLib;
using ComLib.Collections;
using ComLib.Arguments;
using ComLib.Data;
using ComLib.Application;
using ComLib.Entities;
using ComLib.Models;
using ComLib.LocationSupport;
using ComLib.Extensions;
using ComLib.Samples;
using ComLib.Account;
using ComLib.Web.Modules;
using ComLib.Scheduling;
using ComLib.Exceptions;
using System.Threading;

//using CommonLibrary.Tests.LocationTests;
using CommonLibrary.Tests;
using ComLib.CMS.Tests;
using ComLib.Web.Lib.Services;
using ComLib.Web.Lib.Services.Tests;
using ComLib.Web.Lib.Services.Macros.Tests;
using NUnit.Framework;

/*
using ComLib.Web.Modules.Pages;
using ComLib.Web.Modules.Events;
using ComLib.Web.Modules.Links;
using ComLib.Web.Modules.Posts;
/* */

namespace ComLib.Web.Modules
{
    public class ModulesCodeRunner : App
    {
        static void Main(string[] rawArgs)
        {
            App.Run(new ModulesCodeRunner(), rawArgs);
        }


        public override BoolMessageItem Execute(object context)
        {            
            string con = @".\SQLEXPRESS;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|aspnetdb.mdf;User Instance=true";
            //string con = "Server=kishore_pc1;Database=testdb1;User=testuser1;Password=password;"
            ConnectionInfo conn = new ConnectionInfo(con, "System.Data.SqlClient");
            WebModels models = new WebModels(conn);
            var ctx = models.GetModelContext();
            CodeGeneration.CodeBuilder.CreateAll(ctx, "Profile");
            return BoolMessageItem.True;
        }
    }


    /// <summary>
    /// Light weight unit test runner using NUnit attributes.
    /// This is just used for Visual Web Developer Express.
    /// </summary>
    public class TestRunner : App
    {
        class TestGroup
        {
            public string Name;
            public bool Success;
            public bool Enabled = false;
            public Func<object> Instance;
            public DateTime Started;
            public DateTime Ended;
            public TimeSpan Duration;
            public List<Test> Tests = new List<Test>();

            public void ToFile(string filepath)
            {
                using (var writer = new StreamWriter(filepath))
                {
                    var results = ToResult();
                    writer.Write(results);
                    writer.Flush();
                }
            }


            public string ToResult()
            {
                var buffer = new StringBuilder();
                var txt = string.Format(@"<Group name=""{0}"", Enabled=""{1}"" Passed=""{2}""  Started=""{3}"" Ended=""{4}"" Duration=""{5}"" Tests=""{6}"" >",
                    this.Name, this.Enabled.ToString(), this.Success.ToString(), this.Started, this.Ended, this.Duration.TotalMilliseconds, this.Tests.Count);
                buffer.Append(txt + Environment.NewLine);
                foreach (var test in Tests)
                {
                    buffer.Append("\t");
                    buffer.Append(test.ToXml());
                }
                buffer.Append("</Group>" + Environment.NewLine);
                return buffer.ToString();
            }
        }


        class Test
        {
            public string Name;
            public bool Success;
            public DateTime Started;
            public DateTime Ended;
            public TimeSpan Duration;
            public Exception Error;


            public override string ToString()
            {
                var txt = string.Format("Test Passed: {0}, Name:{1}, Started: {2}, Ended: {3}, Duration: {4}, Error: {5}",
                    this.Success.ToString(), this.Name, this.Started, this.Ended, this.Duration.TotalMilliseconds, this.Error == null ? "" : this.Error.Message);
                return txt + Environment.NewLine;
            }


            public string ToXml()
            {
                var txt = string.Format(@"<Test Passed=""{0}"", Name=""{1}"",Started=""{2}"", Ended=""{3}"", Duration=""{4}"", Error=""{5}"" />",
                    this.Success.ToString(), this.Name, this.Started, this.Ended, this.Duration.TotalMilliseconds, this.Error == null ? "" : this.Error.Message);
                return txt + Environment.NewLine;
            }
        }


        public override BoolMessageItem Execute(object context)
        {
            var tests = new List<TestGroup>();

            // Run all the tests.
            tests.Add(new TestGroup { Enabled = true,  Instance = () => new InformationServiceTests() });
            tests.Add(new TestGroup { Enabled = true,  Instance = () => new MacroServiceTests() });
            tests.Add(new TestGroup { Enabled = false, Instance = () => new DashboardServiceTests() });
            tests.Add(new TestGroup { Enabled = false, Instance = () => new MembershipServiceTests() });
            tests.Add(new TestGroup { Enabled = false, Instance = () => new EntitySecurityHelperTests() });
            tests.Add(new TestGroup { Enabled = false, Instance = () => new MarkupServiceTests() });
            tests.Add(new TestGroup { Enabled = false, Instance = () => new WidgetTests() });
            tests.Add(new TestGroup { Enabled = false, Instance = () => new EntityControllerHelperTests() });
            return RunTests(tests);
        }


        private BoolMessageItem RunTests(List<TestGroup> tests)
        {
            foreach (var group in tests)
            {
                if (group.Enabled)
                {
                    var fixture = group.Instance();
                    group.Name = fixture.GetType().Name;
                    var methods = fixture.GetType().GetMethods( BindingFlags.Public | BindingFlags.Instance );
                    foreach (var method in methods)
                    {
                        var attributes = method.GetCustomAttributes(typeof(TestAttribute), true);
                        if (attributes.Length > 0)
                        {
                            var test = new Test { Name = method.Name, Started = DateTime.Now };
                            try
                            {
                                var instance = group.Instance();
                                method.Invoke(instance, null);
                                test.Ended = DateTime.Now;
                                test.Success = true;
                            }
                            catch (Exception ex)
                            {
                                test.Error = ex;
                                test.Success = false;
                                test.Ended = DateTime.Now;
                            }
                            test.Duration = test.Ended - test.Started;
                            group.Tests.Add(test);
                        }
                    }
                    group.ToFile(@"c:\dev\tests\unittests\" + group.Name + ".xml");
                }
                
            }
            return new BoolMessageItem(null, true, string.Empty);
        }
    }
}
