using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using ComLib.Extensions;
using ComLib.CaptchaSupport;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Core;


namespace ComLib.Web.Modules.Feedbacks
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class FeedbackController : JsonController<Feedback>
    {

        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Title", "Content", "Email", "Name", "Url" },
               columnProps: new List<Expression<Func<Feedback, object>>>() { a => a.Id, a => a.Title, a => a.Content, a => a.Email, a => a.Name, a => a.Url },
               columnWidths: null, customRowBuilder: "FeedbackGridBuilder");
        }


        /// <summary>
        /// Create a feedback entry.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Create()
        {
            ForceTheme(true);
            SetPageTitle("Feedback");
            return base.Create();
        }


        [AcceptVerbs(HttpVerbs.Post)]        
        public override ActionResult  Create(Feedback entity)
        {
            ForceTheme(true);
            // Wrong verification code.
            if (!Captcha.IsCorrect())
            {
                return View(_viewSettings.PageLocationForCreate,
                    EntityViewHelper.BuildViewModelForForm(
                    new EntityActionResult(false, message: "Verification code is incorrect", item: entity),
                    this._modelname, "create", _settings));
            }

            var result = _helper.Create(entity);
            if(result.Success) return View("Pages/Thankyou", "Thank you for submitting your feedback");

            return View(_viewSettings.PageLocationForCreate, 
                EntityViewHelper.BuildViewModelForForm(result, this._modelname, "create", _settings));
        }
    }
}
