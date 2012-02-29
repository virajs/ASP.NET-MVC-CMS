using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ComLib;
using ComLib.Extensions;
using ComLib.Web.Templating;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Services;



namespace ComLib.Web.Lib.Services.Tests
{
    
    [TestFixture]
    public class MarkupServiceTests
    {
        private DomDoc _domdoc;
                

        [Test]
        public void Can_Parse_Html()
        {
            Test1(null, "<", ">", @"<div id=""2"" class=""post"">Content in here</div>", 
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_WithSpaces()
        {
            Test1(null, "<", ">", @" <div  id=""2""  class=""post"" >  Content in here </div> ",
                "div", "  Content in here ", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 1, "  ");
        }



        [Test]
        public void Can_Parse_CustomBrackets()
        {
            Test1(null, "[", "]", @"[div id=""2"" class=""post""]Content in here[/div]", 
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_All_Characters_In_Inner_Content()
        {
            Test1(null, "[", "]", @"[div id=""2"" class=""post""]abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?[/div]",
                "div", @"abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_All_Characters_In_External_Content()
        {
            Test1("$", "[", "]", @"abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? $[div id=""2"" class=""post""]abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?[/div]abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? ",
                "div", @"abcdefghijklmnopqrstuvwxyz 0123456789 `~!@#$%^&*()_+-=[]\{}|;':"",./<>?", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 41, @"abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? abc 012 `~!@#$%^&*()_+-=[]\{}|;':"",./<>? ");
        }


        [Test]
        public void Can_Parse_Open_Close_Bracket_In_External_Content()
        {
            Test1(null, "[", "]", @"before [] [div id=""2"" class=""post""]Content in here[/div] after[]",
                "div", @"Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 10, "before []  after[]");
        }

        [Test]
        public void Can_Parse_EmptyTag()
        {
            Test1(null, "[", "]", @"-[div id=""2"" class=""post""/]-",
                "div", "", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 1, "--");
        }


        [Test]
        public void Can_Parse_CustomBrackets_And_Prefix()
        {
            Test1("$", "[", "]", @"$[div id=""2"" class=""post""]Content in here[/div]", 
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 0);
        }


        [Test]
        public void Can_Parse_CustomBrackets_And_Prefix_With_ExternalContent()
        {
            Test1("$", "[", "]", @"text before $[div id=""2"" class=""post""]Content in here[/div] text after",
                "div", "Content in here", new Dictionary<string, string>() 
            {
                { "id", "2"}, { "class", "post" }
            }, 12, "text before  text after");
        }


        private void Test1(string prefix, string open, string close, string content, 
            string expectedTagName, string expectedTagContent, IDictionary<string, string> args,
            int expectedTagPosition = 0, string expectedContent = "")
        {
            var doc = new DomDocParser(prefix, open, close);
            var result = doc.Parse(content);            
            var first = result.Tags[0];
           
            Assert.AreEqual(result.Content, expectedContent);
            Assert.AreEqual(result.Tags.Count, 1);
            Assert.AreEqual(first.Position, expectedTagPosition);
            Assert.AreEqual(first.Name,  expectedTagName);
            Assert.AreEqual(first.InnerContent, expectedTagContent);
            foreach (var pair in args)
            {
                Assert.IsTrue(first.Attributes.Contains(pair.Key));
                Assert.AreEqual(first.Attributes[pair.Key], pair.Value);
            }
        }    
    }
}
