using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Data;
using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.Web.Modules.OptionDefs
{
    public partial class OptionDef : ActiveRecordBaseEntity<OptionDef>, IEntity
    {


        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                OptionDef entity = (OptionDef)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Section, false, true, true, -1, 50, results, "Section");
                Validation.IsStringLengthMatch(entity.Key, false, true, true, 1, 50, results, "Key");
                Validation.IsStringLengthMatch(entity.ValType, false, true, true, 1, 20, results, "ValType");
                Validation.IsStringLengthMatch(entity.Values, true, false, true, -1, 200, results, "Values");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Get the options by the section name.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static IList<OptionDef> BySection(params string[] sectionNames)
        {
            var options = Find(Query<OptionDef>.New().Where(o => o.Section).In<string>(sectionNames).OrderBy(o => o.SortIndex));
            return options;
        }
    }
}
