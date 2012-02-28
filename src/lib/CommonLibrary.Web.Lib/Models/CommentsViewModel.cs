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


namespace ComLib.Web.Lib.Models
{
    
    public class CommentsViewModel
    {
        public string PostAuthor; 
        public string Group;
        public int RefId;
        public string Url;
        public int PageSize;
        public bool EnablePaging;
        public bool EnableCommentAdding;
        public string SubmitControllerName;
        public string SubmitActionName;
    }
}
