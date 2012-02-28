using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib;
using ComLib.Entities;
using ComLib.Web.Lib.Core;
using ComLib.ValidationSupport;
using ComLib.Web.Lib.Attributes;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Modules.Links
{
    
    /// <summary>
    /// Link class extensions.
    /// </summary>    
    [Model(Id = 3, DisplayName = "Link", Description= "Link to external site or resource", IsPagable = true,
        IsExportable = true, IsImportable = true, FormatsForExport = "xml,csv,ini", FormatsForImport = "xml,csv,ini",
        RolesForCreate = "${Admin}", RolesForView = "?", RolesForIndex = "?", RolesForManage = "${Admin}", 
        RolesForDelete = "${Admin}",  RolesForImport = "${Admin}", RolesForExport = "${Admin}")]
    public partial class Link : ActiveRecordBaseEntity<Link>, IEntity, IEntitySortable, IEntityClonable
    {
        /// <summary>
        /// Get validator for validating this entity.
        /// </summary>
        /// <returns></returns>
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Link entity = (Link)validationEvent.Target;
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 100, results, "Name");
                Validation.IsStringLengthMatch(entity.Group, true, false, true, -1, 100, results, "Group");
                Validation.IsStringRegExMatch(entity.Url, false, RegexPatterns.Url, results, "Url");
                Validation.IsStringLengthMatch(entity.Description, true, false, true, -1, 50, results, "Description");
                Validation.IsNumericWithinRange(entity.SortIndex, false, false, -1, -1, results, "SortIndex");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }


        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="url">The URL.</param>
        /// <param name="group">The group.</param>
        /// <param name="description">The description.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns></returns>
        public static Link Create(string name, string url, string group, string description, int sortIndex)
        {
            var link = new Link() { Name = name, Url = url, Group = group, Description = description, SortIndex = sortIndex };
            Create(link);
            return link;
        }
    }
    
}
