using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Linq.Expressions;

using ComLib.Data;
using ComLib.Entities;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Extensions;


namespace ComLib.Web.Lib.Core
{

    /// <summary>
    /// Base Controller for entities/models with support for CRUD actions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityController<T> : CommonController where T : class, IEntity, new()
    {
        /// <summary>
        /// Helper class for controller crud actions ( this factors in permissions ).
        /// </summary>
        protected EntityHelper<T> _helper;


        /// <summary>
        /// Settings for models.
        /// </summary>
        protected EntitySettingsHelper _settings;


        /// <summary>
        /// Settings for the view for this model.
        /// </summary>
        protected ModelViewSettings _viewSettings;


        /// <summary>
        /// Lamda for getting the title of the page based on the entity.
        /// </summary>
        protected Func<T, string> _titleFetcher;


        /// <summary>
        /// Page location after create/editing an entity.
        /// </summary>
        protected string PageLocationForNotAuthorized = "Pages/NotAuthorized";
        protected string PageLocationForNotFound = "Pages/NotFound";


        /// <summary>
        /// The name of the model.
        /// </summary>
        protected string _modelname = typeof(T).Name;


        #region Constructors
        /// <summary>
        /// Intialize
        /// </summary>
        public EntityController()
        {
            _settings = _entitySettingsHandler();
            var modelSettings = _settings.ModelFor<T>();
            _viewSettings = modelSettings == null ? new ModelSettings().View : modelSettings.View;
            _helper = new EntityHelper<T>(_settings, typeof(T).Name);
            _modelname = this.GetType().Name.Replace("Controller", "");
            EntityViewHelper.CheckAndDefaultSettings(modelSettings);
            Init();
        }
        #endregion


        #region CRUD
        /// <summary>
        /// Create a new entity entity.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Create()
        {
            var result = _helper.Create();
            return BuildActionResult(result, _viewSettings.PageLocationForCreate);
        }


        /// <summary>
        /// Create the item by automatically building up the entity using the form data.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Create(T item)
        {
            var result = _helper.Create(item);
            return BuildRedirectResult(result, _viewSettings.ActionForCreationSuccess, routeValues: new { id = item.Id },
                viewLocationForErrors: _viewSettings.PageLocationForCreate, viewActionForErrors: "Create");
        }


        /// <summary>
        /// Display the entity.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Details(int id)
        {
            var result = _helper.Details(id);
            HandlePageTitle(result);
            return BuildActionResult(result, _viewSettings.PageLocationForDetails);
        }


        /// <summary>
        /// Show entity using optimized url
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public virtual ActionResult Show(string title)
        {
            int id = ComLib.Entities.EntityHelper.IdFromTitle(title);
            return Details(id);
        }
      

        /// <summary>
        /// Copy the entity associated w/ the id supplied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Copy(int id)
        {
            var result = _helper.Copy(id);
            return BuildActionResult(result, _viewSettings.PageLocationForCreate);
        }
      

        /// <summary>
        /// Edit the entity associated w/ the id supplied.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Edit(int id)
        {
            var result = _helper.Edit(id);
            return BuildActionResult(result, _viewSettings.PageLocationForEdit);
        }


        /// <summary>
        /// Edit the item by automatically building up the entity using the form data.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult Edit(T item)
        {
            var result = _helper.Edit(item);
            return BuildRedirectResult(result, successAction: _viewSettings.ActionForCreationSuccess, routeValues: new { id = item.Id },
                        viewLocationForErrors: _viewSettings.PageLocationForEdit, viewActionForErrors: "Edit");
        }


        /// <summary>
        /// Delete the entity associated with the supplied id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual ActionResult Delete(int id)
        {
            var result = _helper.Delete(id);
            return BuildActionResult(result, "Pages/Deleted");
        }


        /// <summary>
        /// Handles the indexing view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Index(int page = 1, int pagesize = 15)
        {
            var result = _helper.FindByRecent(page, pagesize);
            return BuildActionResult(result, _viewSettings.PageLocationForIndex);
        }


        /// <summary>
        /// Handles the indexing view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Find(IQuery<T> query = null)
        {
            var result = _helper.Find(query);

            // Needs to be a paged list.
            if(result.Item != null )
            {
                IList<T> all = result.ItemAs<IList<T>>();
                result.Item = new PagedList<T>(1, 10000, all.Count, all);
            }
            return BuildActionResult(result, _viewSettings.PageLocationForIndex);
        }

        
        /// <summary>
        /// Handles the indexing view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult FindPaged(int page = 1, int pagesize = 15, IQuery<T> query = null)
        {
            if (query == null)
                return Index(page, pagesize);

            var result = _helper.Find(page, pagesize, query);
            return BuildActionResult(result, _viewSettings.PageLocationForIndex);
        }


        /// <summary>
        /// Handles the management view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult Manage(int page = 1, int pagesize = 15)
        {
            DashboardLayout(true);
            return ManageUsingQuery(page, pagesize, null);
        }


        /// <summary>
        /// Handles the management view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult ManageUsingQuery(int page, int pagesize, IQuery<T> query = null)
        {
            EntityActionResult result = _helper.Manage(page, pagesize, query);
            return BuildActionResult(result, _viewSettings.PageLocationForManage);
        }
        #endregion


        #region RSS
        /// <summary>
        /// RSSs this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Rss()
        {
            return Feed("rss");
        }


        /// <summary>
        /// Atoms this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Atom()
        {
            return Feed("atom");
        }
        #endregion         


        /// <summary>
        /// Generate a feed result for for the specified format.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        protected ActionResult Feed(string format)
        {
            //var settings = ControllerFactoryHelper.Settings();
            //var feed = EntityHelper.AsFeed<T>(this.HttpContext.Request.Url, settings.Author, settings.Title, settings.Description);
            //return new FeedActionResult() { Feed = feed, Format = format };
            return null;
        }


        /// <summary>
        /// Converts the entity result to an actionresult.
        /// </summary>
        /// <param name="result">EntityActionResult</param>
        /// <param name="successPage">Page url if result is succcessful.</param>
        /// <param name="successAction"></param>
        /// <param name="failPage"></param>
        /// <param name="failAction"></param>
        /// <returns></returns>
        protected virtual ActionResult BuildActionResult(EntityActionResult result, string successPage = "", string successAction = "", string failPage = "", string failAction = "",
            Action<EntityBaseViewModel> onAfterViewModelCreated = null, bool useDefaultFormAction = false)
        {
            // Not authorized or Not Available ?
            var accessable = IsAccessable(result);
            if (!accessable.Success) return View(accessable.Item);

            PopulateErrors(result);
    
            // No action provided? default to action of url /model/action.
            if (string.IsNullOrEmpty(successAction))
            {
                if (string.IsNullOrEmpty(successPage))
                    successAction = "Manage";
                else
                    successAction = successPage.Substring(successPage.LastIndexOf("/") + 1);
            }
            
            // Build the appropriate viewmodel based on success/failure of entity action( create/edit ) etc.
            EntityBaseViewModel viewmodel = result.Success
                                          ? EntityViewHelper.BuildViewModel<T>(result, this._modelname, successAction, _settings)
                                          : EntityViewHelper.BuildViewModel<T>(result, this._modelname, failAction, _settings);

            // Option 1 : Default the form action to what ever it is currently ( typically used for create/edit pages for post backs ).
            if (useDefaultFormAction && viewmodel is EntityFormViewModel)
            {
                var formModel = viewmodel as EntityFormViewModel;
                var pathAndQuery = this.Request.Url.PathAndQuery;
                var actionAndQuery = pathAndQuery.Substring(pathAndQuery.IndexOf("/", 1) + 1);
                formModel.FormActionName = actionAndQuery;
                formModel.RouteValues = null;
            }

            // Allow caller to do some additional processing.
            if (onAfterViewModelCreated != null)
                onAfterViewModelCreated(viewmodel);

            string pageLocation = result.Success ? successPage : failPage;            
            return View(pageLocation, viewmodel);
        }


        /// <summary>
        /// Converts to a redirection result.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="failAction"></param>
        /// <param name="redirectUrl"></param>
        /// <param name="redirectUrlRouteValues"></param>
        /// <returns></returns>
        protected virtual ActionResult BuildRedirectResult(EntityActionResult result, string successAction = null, 
            string successUrl = null, object routeValues = null, string viewLocationForErrors = null, string viewActionForErrors = null)
        {
            // Not authorized or Not Available ?
            var accessable = IsAccessable(result);
            if (!accessable.Success) return View(accessable.Item);

            if (result.Success && !string.IsNullOrEmpty(successUrl))
                return Redirect(successUrl);

            if (result.Success && !string.IsNullOrEmpty(successAction))
                return RedirectToAction(successAction, routeValues);

            EntityViewHelper.PopulateErrors(result, ModelState);

            var viewmodel = EntityViewHelper.BuildViewModel<T>(result, this._modelname, viewActionForErrors, _settings);            
            return View(viewLocationForErrors, viewmodel);
        }
     

        /// <summary>
        /// Handles processing of the html pages title.
        /// </summary>
        /// <param name="result"></param>
        protected virtual void HandlePageTitle(EntityActionResult result)
        {
            // Set page title.
            if (result.Success && result.Item != null && _titleFetcher != null)
            {
                T entity = result.ItemAs<T>();
                SetPageTitle(_titleFetcher(entity));
            }
        }


        /// <summary>
        /// Populates errors into the model state dictionary.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="stringErrors"></param>
        /// <param name="errors"></param>
        protected virtual void PopulateErrors(EntityActionResult result, IList<string> stringErrors = null, IErrors errors = null)
        {
            EntityViewHelper.PopulateErrors(result, ModelState, stringErrors, errors);
        }


        protected BoolMessageItem<string> IsAccessable(EntityActionResult result)
        {
            if (!result.Success && !result.IsAuthorized)
                return new BoolMessageItem<string>("Pages/NotAuthorized", false, string.Empty);

            if (!result.Success && !result.IsAvailable)
                return new BoolMessageItem<string>("Pages/NotFound", false, string.Empty);

            return new BoolMessageItem<string>(string.Empty, true, string.Empty);
        }
    }
}
