using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Reflection;
using NUnit.Framework;

using ComLib.Entities;
using ComLib;


/*
namespace CommonLibrary.Web.Modules.Tests
{
    public class ModelMetaData
    {
        public Type ModelType;
        public List<string> ColumnNames;
        public List<PropertyInfo> ColumnProps;
        public int[] ColumnWidths;
    }


    public class Article
    {
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public bool IsExternal { get; set; }
        public int NumberOfComments { get; set; }
    }


    [TestFixture]
    class JsonControllerTests
    {
        [Test]
        public void CanConfigure()
        {
            var controller = new JsonController<Article>();

            controller.InitColumns(
                columnNames: new List<string>() { "Id", "PublishDate", "Title", "Is External", "Author", "Comments" },
                columnProps: new List<Expression<Func<Article, object>>>() { a => a.Id, a => a.PublishDate, a => a.Title, a => a.IsExternal, a => a.Author, a => a.NumberOfComments },
                columnWidths: null);

            // Get the paged results.
            JsonFormatResult json = controller.FindRecent(1, 5);
            Assert.IsNotNull(json);
            Assert.AreEqual(results.Action, "index");
            Assert.AreEqual(results.PageIndex, 1);
            Assert.AreEqual(results.PageSize, 5);
            Assert.AreEqual(results.Count, 5);
            Assert.AreEqual(results.Success, true);
            Assert.AreEqual(results.Error, string.Empty);
            Assert.AreEqual(results.Message, string.Empty);
            Assert.AreEqual(results.Columns.Count, 6);
            Assert.AreEqual(results.Columns[0], "Id");
            Assert.AreEqual(results.Columns[1], "PublishDate");
            Assert.AreEqual(results.Columns[2], "Title");
            Assert.AreEqual(results.Columns[3], "Is External");
            Assert.AreEqual(results.Columns[4], "Author");
            Assert.AreEqual(results.Columns[5], "Comments");
            Assert.AreEqual(results.ColumnWidths, null);
            Assert.AreEqual(results.Data.Count, 5);
            Assert.AreEqual(results.Data[0].Title, "Article 1");
            Assert.AreEqual(results.Data[2].Title, "Article 2");
        }
    }
}
*/