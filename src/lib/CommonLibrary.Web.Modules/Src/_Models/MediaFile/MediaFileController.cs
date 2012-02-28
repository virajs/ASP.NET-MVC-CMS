using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComLib.Authentication;
using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Models;


namespace ComLib.Web.Modules.Media
{
    [HandleError]
    public class MediaFileController : EntityController<MediaFile>
    {
        private static Dictionary<string, string> _excludeMediaUploadFrameProps = new Dictionary<string, string>()
        {
            { "ActionUrl", "ActionUrl"},
            { "FullFrameSourceUrl", "FullFrameSourceUrl" }
        };


        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            DashboardLayout(true);
        }


        /// <summary>
        /// Do not allow creation directly.
        /// </summary>
        /// <returns></returns>
        public override ActionResult Create()
        {
            return View(PageLocationForNotAuthorized);
        }


        /// <summary>
        /// Can only create in either a folder or for an entity.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public override ActionResult Create(MediaFile item)
        {
            return View(PageLocationForNotAuthorized);
        }


        /// <summary>
        /// Do not allow copying.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ActionResult Copy(int id)
        {
            return View(PageLocationForNotAuthorized);
        }


        /// <summary>
        /// Create the file in the folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderid"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateInFolder(int id)
        {
            var result = _helper.Create();
            return BuildActionResult(result, _viewSettings.PageLocationForCreate, useDefaultFormAction: true);
        }


        /// <summary>
        /// Create the file in the folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderid"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult CreateInFolder(int id, FormCollection form)
        {
            bool success = MediaHelper.CreateMediaFiles(Request, ModelState, id, 0, true).Success;
            
            // Errors ?
            if (!success) return CreateInFolder(id);

            string successurl = "/mediafile/managebyfolder/" + id;            
            return Redirect(successurl);
        }


        /// <summary>
        /// Create the file in the folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderid"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult CreateForEntity(int id, int refgroup)
        {
            // Can create?
            var entityResult = _helper.Create();
            var actionResult = BuildActionResult(entityResult, _viewSettings.PageLocationForCreate, onAfterViewModelCreated: (viewModel) =>
            {
                EntityFormViewModel formModel = viewModel as EntityFormViewModel;
                formModel.FormActionName = "CreateForEntity";
                formModel.RouteValues = new { id = id, refgroup = refgroup };
                formModel.UrlCancel = string.Format("/mediafile/managebyrefid/{0}/?refgroup={1}", id, refgroup);            
            });
            return actionResult;
        }


        /// <summary>
        /// Create the file in the folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderid"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult CreateForEntity(int id, int refgroup, FormCollection form)
        {
            bool success = MediaHelper.CreateMediaFiles(Request, ModelState, id, refgroup, false).Success;

            // Errors ?
            if (!success) return CreateInFolder(id);

            string successurl = string.Format("/mediafile/managebyrefid/{0}/?refgroup={1}", id, refgroup);
            return Redirect(successurl);
        }


        /// <summary>
        /// Get all the media folders.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ManageByFolder(int id)
        {
            var pagedResult = _helper.Find(1, 200, Query<MediaFile>.New().Where(m => m.ParentId).Is(id).OrderByDescending(m => m.CreateDate)).ItemAs<PagedList<MediaFile>>();
            var model = new MediaFilesViewModel()
            {
                Mode = MediaGalleryViewMode.FolderId,
                FolderId = id,
                Items = pagedResult,
                UrlCreate = "/mediafile/createinfolder/" + id,
                UrlBack = Url.Link("manage", typeof(MediaFolder).Name, null),
                ControlPath = "ModelList"
            };
            return View(_viewSettings.PageLocationForManage, model);
        }


        /// <summary>
        /// Get all the media folders.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ManageByRefId(int id, int refgroup)
        {
            var pagedResult = _helper.Find(1, 200, Query<MediaFile>.New().Where(m => m.RefId).Is(id).And(m => m.RefGroupId).Is(refgroup).OrderByDescending(m => m.CreateDate)).ItemAs<PagedList<MediaFile>>();
            var model = new MediaFilesViewModel();
            model.EntityId = id;
            model.EntityGroup = refgroup;
            model.Mode = MediaGalleryViewMode.Entity;
            model.Items = pagedResult;
            model.UrlEdit = string.Format("/{0}/edit/{1}", ModuleMap.Instance.GetShortName(refgroup), id);
            model.UrlBack = string.Format("/{0}/details/{1}", ModuleMap.Instance.GetShortName(refgroup), id);
            model.UrlCreate = string.Format("/mediafile/createforentity/{0}/?refgroup={1}", id, refgroup);
            model.ControlPath = "ModelList";
            return View(_viewSettings.PageLocationForManage, model);
        }


        /// <summary>
        /// Edit the entity referenced by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public override ActionResult Edit(int id)
        {
            ViewData["isediting"] = true;
            return base.Edit(id);
        }


        /// <summary>
        /// Uploads media files using an iframe.
        /// </summary>
        /// <param name="formAction"></param>
        /// <param name="isFolderMode"></param>
        /// <param name="refId"></param>
        /// <param name="modelName"></param>
        /// <param name="numberOfFileUploadsAllowed"></param>
        /// <param name="showDetailUI"></param>
        /// <returns></returns>
        [Authorize]        
        public ActionResult Upload(bool isFolderMode, int refId, string modelName,
            int numberOfUploadsAllowed = 4, bool showDetailUI = true, int width = 300, int height = 200, string jscallback = "")
        {
            MediaUploadFrameViewModel options = new MediaUploadFrameViewModel
            {
                IsFolderMode = isFolderMode,
                RefId = refId,
                ModelName = modelName,
                NumberOfUploadsAllowed = numberOfUploadsAllowed,
                ShowDetailUI = showDetailUI,
                Width = width,
                Height = height,
                IsUploadMode = true,
                JavascriptCallback = jscallback,
                RunJavascriptCallBack = false
            };
            EnablePageLayout(false);
            return View(options);
        }


        [Authorize]
        [HttpPost]
        public ActionResult DoUpload(FormCollection form)
        {
            EnablePageLayout(false);
            var model = new MediaUploadFrameViewModel();
            ComLib.MapperSupport.MapperWebForms.UpdateModel(model, form, "MediaUploadFrameModel", _excludeMediaUploadFrameProps);
            int refGroup = model.IsFolderMode ? 0 : ComLib.Web.Lib.Core.ModuleMap.Instance.GetId(model.ModelName);
            BoolMessageItem<IList<MediaFile>> result =  model.IsFolderMode
                         ? MediaHelper.CreateMediaFiles(Request, ModelState, model.RefId, 0, true)
                         : MediaHelper.CreateMediaFiles(Request, ModelState, model.RefId, refGroup, false);

            model.IsUploadMode = !result.Success;
            model.RunJavascriptCallBack = result.Success;
            if (result.Success) 
                FlashMessages("File(s) have been successfully uploaded.");
            else if (result.Item != null && result.Item.Count > 0)
                foreach (var file in result.Item) FlashErrors(file.Errors);

            return View("Upload", model);
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
        protected ActionResult Edit(MediaFile mediafile, string successUrlId, bool isRedirectionToUrl = false)
        {
            if( mediafile.IsDirectoryBased )
                 successUrlId = "/mediafile/managebyfolder/" + mediafile.ParentId;
            else
                successUrlId = string.Format("/mediafile/managebyrefid/{0}/?refgroup={1}", mediafile.RefId, mediafile.RefGroupId);

            var result = _helper.Edit(mediafile);
            return BuildRedirectResult(result, successAction: "Details", routeValues: new { id = mediafile.Id },
                        viewLocationForErrors: _viewSettings.PageLocationForEdit, viewActionForErrors: "Edit");
        }


        /// <summary>
        /// Custom update model implementation.
        /// </summary>
        /// <param name="entity"></param>
        protected void DoUpdateModel(MediaFile entity)
        {
            MediaFileMapper.MapFile(Request, string.Empty, entity);
        }
    }
}
