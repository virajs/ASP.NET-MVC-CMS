using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.Web.Modules.Feedbacks
{
    public partial class Feedback : ActiveRecordBaseEntity<Feedback>, IEntity
    {
        /// <summary>
        /// Returns a validator to validate this entity.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Feedback entity = (Feedback)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Title, true, false, true, -1, 150, results, "Title");
                Validation.IsStringLengthMatch(entity.Content, false, true, false, 1, 1000, results, "Content");
                Validation.IsStringLengthMatch(entity.Name, true, false, true, -1, 20, results, "Name");
                Validation.IsStringRegExMatch(entity.Email, false, RegexPatterns.Email, results, "Email");
                Validation.IsStringRegExMatch(entity.Url, true, RegexPatterns.Url, results, "Url");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }
    }
}
