using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using ComLib.Web.Lib.Core;
using ComLib.Entities;


namespace ComLib.Web.Modules.Articles
{

    public class Article : ActiveRecordBaseEntity<Article>
    {
        public DateTime PublishDate { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public bool IsExternal { get; set; }
        public int NumberOfComments { get; set; }
    }





    public class ArticleController : JsonController<Article>
    {
        /// <summary>
        /// Initialize the articles management metadata.
        /// </summary>
        public ArticleController()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "PublishDate", "Title", "Is External", "Author", "Comments" },
               columnProps: new List<Expression<Func<Article, object>>>() { a => a.Id, a => a.PublishDate, a => a.Title, a => a.IsExternal, a => a.Author, a => a.NumberOfComments },
               columnWidths: null);
        }
    }
}
