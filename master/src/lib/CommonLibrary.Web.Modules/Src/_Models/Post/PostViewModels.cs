using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using ComLib;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Account;


namespace ComLib.Web.Modules.Posts
{

    public class PostActionsViewModel
    {
        public int EntityId;
        public string EntityName;
        public string EntityDetailsUrl;

        public bool CommentCountEnabled;
        public int  CommentCount;
        public string CommentLink; 
        
        public string PermaLink;
        public string EmailSubject;
        public string EmailBody;

        public bool EnableComments;
        public bool EnableFavorites;
        public bool EnableFlaging;
       

        public PostActionsViewModel()
        {
        }


        public PostActionsViewModel(bool showCommentCount, int commentCount, string commentsUrl, string permaLink, string subject, string body)
        {
            CommentCountEnabled = showCommentCount;
            CommentCount = commentCount;
            PermaLink = permaLink;
            EmailBody = body;
            EmailSubject = subject;
        }
    }
}
