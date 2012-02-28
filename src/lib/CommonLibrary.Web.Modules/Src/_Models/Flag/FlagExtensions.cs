using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Entities;
using ComLib.ValidationSupport;
using ComLib.Web.Lib.Attributes;


namespace ComLib.Web.Modules.Flags
{
    [Model(Id = 106, DisplayName = "Flag", Description = "Flag", IsPagable = true, IsSystemModel =  true,
        RolesForCreate = "?", RolesForView = "${Admin}", RolesForIndex = "?", RolesForManage = "${Admin}",
        RolesForDelete = "${Admin}", RolesForImport = "${Admin}", RolesForExport = "${Admin}")]
    public partial class Flag : ActiveRecordBaseEntity<Flag>
    {
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Flag entity = (Flag)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Title, false, true, true, 1, 150, results, "Title");
                Validation.IsNumericWithinRange(entity.RefId, true, false, 1, -1, results, "RefId");
                Validation.IsStringLengthMatch(entity.Model, false, true, true, 1, 30, results, "Model");
                Validation.IsStringLengthMatch(entity.Url, false, true, true, 10, 200, results, "Url");

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
            this.FlaggedDate = DateTime.Now;
            this.FlaggedByUser = Authentication.Auth.UserShortName;                
            return true;
        }
    }
}
