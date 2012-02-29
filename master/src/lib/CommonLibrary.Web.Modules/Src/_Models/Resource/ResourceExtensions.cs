using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Data;
using ComLib.Entities;
using ComLib.ValidationSupport;


namespace ComLib.Web.Modules.Resources
{
    public partial class Resource : ActiveRecordBaseEntity<Resource>, IEntity
    {


        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Resource entity = (Resource)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.AppName, false, true, true, 1, 30, results, "AppName");
                Validation.IsStringLengthMatch(entity.ResourceType, false, true, true, 1, 30, results, "ResourceType");
                Validation.IsStringLengthMatch(entity.Section, false, true, true, 1, 50, results, "Section");
                Validation.IsStringLengthMatch(entity.Key, false, true, true, 1, 50, results, "Key");
                Validation.IsStringLengthMatch(entity.Language, false, true, true, 1, 20, results, "Language");
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 80, results, "Name");
                Validation.IsStringLengthMatch(entity.ValType, false, true, true, 1, 20, results, "ValType");
                Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 200, results, "Description");
                Validation.IsStringLengthMatch(entity.Example, true, false, true, -1, 50, results, "Example");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Get the resources by the section name.
        /// </summary>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static IDictionary<string, Resource> BySection(params string[] sectionNames)
        {
            var resources = Find(Query<Resource>.New().Where(r => r.Section).In<string>(sectionNames).OrderBy(r => r.Key));
            IDictionary<string, Resource> lookup = new Dictionary<string, Resource>();
            foreach (Resource res in resources) lookup[res.Key] = res;
            return lookup;
        }
    }
}
