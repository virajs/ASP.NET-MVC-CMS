using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib;
using ComLib.Entities;
using ComLib.ValidationSupport;
using ComLib.Web.Services.GravatarSupport;


namespace ComLib.Web.Modules.Comments
{
    /// <summary>
    /// Customized behaviour for the comments.
    /// </summary>
    public partial class Comment : ActiveRecordBaseEntity<Comment>
    {
        /// <summary>
        /// Gets the validator internal.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Comment entity = (Comment)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Title, true, true, true, 1, 50, results, "Title");
                Validation.IsStringLengthMatch(entity.Content, false, false, false, -1, -1, results, "Content");
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 30, results, "Name");
                Validation.IsStringRegExMatch(entity.Email, false, RegexPatterns.Email, results, "Email");
                Validation.IsStringRegExMatch(entity.Url, true, RegexPatterns.Url, results, "Url");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }



        /// <summary>
        /// Called when [before save].
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <returns></returns>
        public override bool OnBeforeSave(object ctx)
        {
            if (this.IsGravatarEnabled && string.IsNullOrEmpty(ImageUrl))
            {
                Gravatar gravatar = new Gravatar(Email, 40, ComLib.Web.Services.GravatarSupport.Rating.g, IconType.none, ".png");
                ImageUrl = gravatar.Url;
            }
            return true;
        }


        
    }
}
