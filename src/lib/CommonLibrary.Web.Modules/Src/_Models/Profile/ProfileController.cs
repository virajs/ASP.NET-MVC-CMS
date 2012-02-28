using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

using ComLib.Data;
using ComLib.Account;
using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.ViewModels;
using ComLib.Authentication;
using ComLib.Web.Helpers;
using ComLib.Web.Modules.Media;

namespace ComLib.Web.Modules.Profiles
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class ProfileController : JsonController<Profile>
    {
        /// <summary>
        /// Show the post with an seo-optimized title.
        /// e.g. my-first-how-to-472 where 472 is the post id.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public override ActionResult Show(string username)
        {
            var result = _helper.Details(() => Profile.FindUser(username));
            HandlePageTitle(result);
            return BuildActionResult(result, _viewSettings.PageLocationForDetails);
        }


        /// <summary>
        /// Edits the entity based on the username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public ActionResult EditProfileByName(string username)
        {   
            DashboardLayout(true);
            var result = _helper.Edit(() => Profile.FindUser(username));
            var viewmodel = BuildActionResult(result, _viewSettings.PageLocationForEdit, onAfterViewModelCreated: (entityViewModel) =>
            {
                var formModel = entityViewModel as EntityFormViewModel;
                formModel.FormActionName = "edit";
                formModel.RouteValues = new { @id = result.ItemAs<Profile>().Id };
            });
            return viewmodel;
        }        


        /// <summary>
        /// Prevent deletion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ActionResult Delete(int id)
        {
            return Json(new { Success = false, Message = "Deleting of profile not currently supported" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Display the profile details.
        /// </summary>
        /// <param name="id">The User Id</param>
        /// <returns></returns>
        public ActionResult DetailsByUser(int id)
        {
            var result = _helper.Details(() => Query<Profile>.New().Where(p => p.UserId).Is(id));
            return BuildActionResult(result, _viewSettings.PageLocationForDetails);
        }


        /// <summary>
        /// Gets the photo thumbnail as a json response, result object contains "Success:bool, Message:string, Item:string" fields.
        /// </summary>
        /// <param name="id">The id of the profile.</param>
        /// <returns></returns>
        public ActionResult GetPhotoThumbnail(int id)
        {
            var result = _helper.Details(id);
            if (!result.Success)
                return Json(new { Success = false, Message = result.Message }, JsonRequestBehavior.AllowGet);

            var imageTag = result.ItemAs<Profile>().GetImageUrl(wrapInImageTag: true, size: 45);
            return Json(new { Success = true, Message = result.Message, Item = imageTag }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Gets the photo thumbnail as a json response, result object contains "Success:bool, Message:string, Item:string" fields.
        /// </summary>
        /// <param name="id">The id of the profile.</param>
        /// <returns></returns>
        public ActionResult ApplyProfilePhoto(int id)
        {
            var result = _helper.Edit(id);
            if (!result.Success)
                return Json(new { Success = false, Message = result.Message }, JsonRequestBehavior.AllowGet);

            var profile = result.ItemAs<Profile>();
            ProfileHelper.ApplyProfileImage(profile);
            profile.Save();
            var success = !profile.Errors.HasAny;
            var message = success ? string.Empty : profile.Errors.Message("<br/>");            
            return Json(new { Success = success, Message = message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Delete the photoid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        public ActionResult DeletePhoto(int id)
        {
            DashboardLayout(true);
            var result = _helper.Edit(id);
            if(!result.Success)
                return Json( new { Success = false, Message = result.Message}, JsonRequestBehavior.AllowGet);

            if ( !result.ItemAs<Profile>().IsLoggedInUser()) 
                return Json( new { Success = false, Message = "Not authorized to delete"}, JsonRequestBehavior.AllowGet);

            result.ItemAs<Profile>().DeletePhoto();
            return Json(new { Success = true, Message = "Photo has been deleted" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Edit the entity by passing in the form collection so it maps the 
        /// html fields to the entity properites.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="fetcher"></param>
        /// <returns></returns>
        public override ActionResult Edit(Profile updated)
        {
            DashboardLayout(true);
            EntityActionResult result = null;
            try
            {
                result = _helper.Edit(updated, (original) =>
                {
                    // Copy back the original values.
                    updated.UserId = original.UserId;
                    updated.UserName = original.UserName;
                    ProfileHelper.ChangeEmail(updated, original);

                    if (!updated.Errors.HasAny)
                        ProfileHelper.ApplyProfileImage(updated);
                });                
            }
            catch
            {
                result = new EntityActionResult(false, "Error while updating profile", item: updated, isAuthorized: true, isAvailable: true);
            }

            return BuildRedirectResult(result, successAction: _viewSettings.ActionForCreationSuccess, routeValues: new { id = updated.Id },
                                       viewLocationForErrors: _viewSettings.PageLocationForEdit, viewActionForErrors: "Edit");
        }
    }
}
