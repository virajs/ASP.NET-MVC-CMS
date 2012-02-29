using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using ComLib.Data;
using ComLib.Authentication;
using ComLib.Web.Lib.Core;
using ComLib.Extensions;


namespace ComLib.Web.Modules.Comments
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class CommentController : JsonController<Comment>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Name", "Email", "Url", "Gravatar", "Approved", "Image", "Content", "GroupId", "Title" },
               columnProps: new List<Expression<Func<Comment, object>>>() 
                { a => a.Id, a => a.Name, a => a.Email, a => a.Url, a => a.IsGravatarEnabled, a => a.IsApproved, a => a.ImageUrl, a => a.Content, a => a.GroupId, a => a.Title },
               columnWidths: null, customRowBuilder: "CommentGridBuilder" );
        }


        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model. "post" | "event"</param>
        /// <param name="refid">The Id of the entity this comment applies to.</param>
        /// <param name="name">The name of user adding comment.</param>
        /// <param name="email">The email of user adding comment.</param>
        /// <param name="url">The URL of user adding comment.</param>
        /// <param name="content">The content of the comment.</param>
        /// <param name="rating">The rating ( if applicable ) that user has supplied for the post this comment applies to.</param>
        /// <returns></returns>
        public ActionResult Add(string captchatext, string reftext, string model, int refid, string name, string email, string url, string content, int rating)
        {
            // Validate captcha.
            if (!ComLib.CaptchaSupport.Captcha.IsCorrect(captchatext, reftext))
                return Json(new { Success = false, Message = "Invalid verification code supplied." }, JsonRequestBehavior.AllowGet);

            int groupId = 1;
            Comment comment = new Comment() { GroupId = groupId, Content = content, Rating = rating, RefId = refid, Email = email, Name = name, Url = url };
            comment.Create();
            bool success = !comment.Errors.HasAny;
            string message = success ? "Comment has been added." : "Unable to add comment. " + comment.Errors.Message("<br/>");
            return Json(new { Success = success, Message = message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Gets comments based on the group and refid.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="refid">The refid.</param>
        /// <param name="pagenumber">The pagenumber.</param>
        /// <returns></returns>
        public ActionResult ByGroupAndRefId(string group, int refid, int pagenumber, int pagesize)
        {
            if (pagesize == 0) pagesize = 15;
            if (pagenumber <= 0) pagenumber = 1;

            PagedList<Comment> comments = Comment.Find(Query<Comment>.New().Where(c => c.GroupId).Is(1).And(c => c.RefId).Is(refid), pagenumber, pagesize);
            var commentsList = new List<CommentListViewModel>();
            foreach (var comment in comments) commentsList.Add(new CommentListViewModel(comment));
            return Json(new { Success = true, Message = string.Empty, TotalPages = comments.TotalPages, Items = commentsList }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Used for the view. As the original model ( comment ) contains user data e.g. Email address.
        /// </summary>
        class CommentListViewModel
        {
            public string Content;
            public string ImageUrl;
            public string Url;
            public string User;
            public int Rating;
            public double CreateDate;
            public bool IsRegisteredUser;


            public CommentListViewModel(Comment comment)
            {
                Content = comment.Content;
                User = comment.Name;
                Url = comment.Url;
                IsRegisteredUser = comment.UserId > 0;
                CreateDate = comment.CreateDate.ToJavascriptDate();
                ImageUrl = comment.IsGravatarEnabled ? comment.ImageUrl : "/content/images/generic/user_blank.jpg";
                Rating = comment.Rating;
            }
        }
    }
}
