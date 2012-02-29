using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

using ComLib.Web.Lib.Core;
using ComLib.Authentication;
using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Modules.Media
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class MediaFolderController : JsonController<MediaFolder>
    {

        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Name", "Description", "Updated", "Size", "Public" },
               columnProps: new List<Expression<Func<MediaFolder, object>>>() { a => a.Id, a => a.Name, a => a.Description, a => a.UpdateDate, a => a.LengthInK, a => a.IsPublic },
               columnWidths: null, customRowBuilder: "MediaFolderGridBuilder");
        }


        public override ActionResult Details(int id)
        {
            var result = _helper.Details(id);

            // Check permissions to view.
            if (result.Success)
            {
                // Check if folder is public and if person can view it.                
                var folder = result.ItemAs<MediaFolder>();
                if (!folder.IsPublic && !folder.IsOwnerOrAdmin())
                    return View(PageLocationForNotAuthorized);
            }
            HandlePageTitle(result);
            return BuildActionResult(result, _viewSettings.PageLocationForDetails);
        }
    }
}
