using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib.Entities;
using ComLib.Entities.Extensions;
using ComLib.Authentication;
using ComLib.Web.Lib.Models;
using ComLib.Data;


namespace ComLib.Web.Lib.Core
{
    /// <summary>
    /// Controller Helper class for CRUD operations on entities.
    /// </summary>
    /// <remarks>
    /// ***************************************************************************
    /// This functionality is in this class becuse :
    /// 
    ///     1. It's much more unit-testable.
    ///     2. This code in controller requires mock-objects for unit-tests.
    ///     2. It simplifies/reduces the code in the controller.
    /// ***************************************************************************
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class EntityHelper<T> where T : IEntity, new()
    {
        private EntitySettingsHelper _settings;
        private string _modelName;


        /// <summary>
        /// Initialize with optional security helper.
        /// </summary>
        /// <param name="securityHelper"></param>
        public EntityHelper(EntitySettingsHelper settingsHelper = null, string modelName = "")
        {
            Init(settingsHelper, modelName);
        }        
            

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="securityHelper"></param>
        /// <param name="modelName"></param>
        public void Init(EntitySettingsHelper settingsHelper = null, string modelName = "")
        {
            _settings = settingsHelper;
            if (string.IsNullOrEmpty(modelName))
                modelName = typeof(T).Name;

            _modelName = modelName;
        }


        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Create(Func<T> creator = null)
        {
            if(!HasAccessTo("Create"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            T entity = creator == null ? new T() : creator();
            return new EntityActionResult(true, string.Empty, item: entity);
        }


        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Create(T entity, Action<T> onBeforeUpdateCallback = null)
        {
            // Check item is there.
            if (entity == null)
                return new EntityActionResult(success: false, message: "Item not supplied.");

            if (!HasAccessTo("Create"))
                return new EntityActionResult(success:false, message: "Not authorized to perform this action", isAuthorized: false);

            if (onBeforeUpdateCallback != null)
                onBeforeUpdateCallback(entity);

            if (entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to create item", errors: entity.Errors, item: entity);

            var svc = EntityRegistration.GetService<T>();
            svc.Create(entity);

            if (entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to create item", errors: entity.Errors, item: entity);

            return new EntityActionResult(true, string.Empty, null, entity);
        }


        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="onBeforeUpdateCallback">Callback with the updated and original entity respectively.</param>
        /// <returns></returns>
        public EntityActionResult Edit(T entity, Action<T> onBeforeUpdateCallback = null)
        {            
            // Check item is there.
            if (entity == null)
                return new EntityActionResult(success: false, message: "Item not supplied.", item: entity);

            if (!HasAccessTo("Edit"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            // Prevent override of audit fields.
            var svc = EntityRegistration.GetService<T>();
            var orig = svc.Get(entity.Id);

            // apply back any audit fields from original.
            entity.DoUpdateModel<T>(orig);

            // Check security.
            if (!entity.IsOwnerOrAdmin())
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", item: entity, isAuthorized: false);

            if (onBeforeUpdateCallback != null)
            {
                onBeforeUpdateCallback(orig);
                if (entity.Errors.HasAny)
                    return new EntityActionResult(false, "Unable to edit item.", errors: entity.Errors, item: entity);
            }

            // Callback may have done some actions.
            if (entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to edit item.", errors: entity.Errors, item: entity);

            svc.Update(entity);

            // Check again.
            if (entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to edit item.", errors: entity.Errors, item: entity);

            return new EntityActionResult(true, string.Empty, null, entity);
        }


        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Edit(int id)
        {
            return Edit(() =>
            {
                var svc = EntityRegistration.GetService<T>();
                var entity = svc.Get(id);
                return entity;
            });
        }


        /// <summary>
        /// Edit using the fetcher to get the entity to edit.
        /// </summary>
        /// <param name="fetcher"></param>
        /// <returns></returns>
        public EntityActionResult Edit(Func<T> fetcher)
        {
            if (!HasAccessTo("Edit"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            T entity = fetcher();

            // Check item is there.
            if (entity == null)
                return new EntityActionResult(success: false, message: "Entity has been deleted.");

            // Check security.
            if (!entity.IsOwnerOrAdmin())
                return new EntityActionResult(success: false, message: "Not authorized to perform this action",isAuthorized: false);

            return new EntityActionResult(true, string.Empty, null, entity);
        }


        /// <summary>
        /// Turns an entity on or off.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityActionResult OnOff(int id)
        {
            var result = Edit(id);
            if (!result.Success) return result;

            T test = new T();
            if (!(test is IEntityActivatable))
                return new EntityActionResult(false, "This model is not activatable", isAuthorized: true, isAvailable: true);

            T entity = (T)result.Item;
            var onoff = entity as IEntityActivatable;
            onoff.IsActive = !onoff.IsActive;
            var svc = EntityRegistration.GetService<T>();
            svc.Update(entity);

            // Check again.
            if (entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to activate/deactivate item.", errors: entity.Errors, item: entity);
            
            var message = onoff.IsActive ? "Activated" : "Deactivated";
            return new EntityActionResult(true, "Item has been " + message);
        }


        /// <summary>
        /// Turns an entity on or off.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityActionResult Clone(int id)
        {
            var result = Copy(id);
            if (!result.Success) return result;

            T test = new T();
            if (!(test is IEntityClonable))
                return new EntityActionResult(false, "This model is not cloneable", isAuthorized: true, isAvailable: true);

            T entity = (T)result.Item;
            var svc = EntityRegistration.GetService<T>();
            svc.Create(entity);

            // Check again.
            if (entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to clone item.", errors: entity.Errors, item: entity);

            return new EntityActionResult(true, "Item has been cloned.");
        }


        /// <summary>
        /// Save the orderings.
        /// </summary>
        /// <param name="orderings"></param>
        /// <returns></returns>
        public virtual BoolMessage SaveOrdering(string orderings)
        {
            if (!HasAccessTo("Edit"))
                return new BoolMessage(false, "Not authorized to perform this action");

            T test = new T();
            if (!(test is IEntitySortable))
                return new BoolMessage(false, "This model is not sortable");


            List<int> ids = new List<int>();
            List<int> sorts = new List<int>();
            string[] entries = orderings.Split(',');

            // Each ordering id, sortindex
            foreach (string ordering in entries)
            {
                string[] props = ordering.Split(':');

                // Get id and sortindex.
                if (props != null && props.Length > 0 && !string.IsNullOrEmpty(props[0]))
                {
                    // Get the id, zone and sort index.
                    int id = System.Convert.ToInt32(props[0]);
                    int sortIndex = System.Convert.ToInt32(props[1]);
                    ids.Add(id);
                    sorts.Add(sortIndex);
                }
            }
            var idArr = ids.ToArray();
            var svc = EntityRegistration.GetService<T>();

            // Get only the items w/ matching ids.
            var items = svc.Repository.ToLookUp(Query<T>.New().Where(i => i.Id).In<int>(idArr));

            // Now save the new sortindex.
            for (int ndx = 0; ndx < ids.Count; ndx++)
            {
                T item = items[ids[ndx]];
                ((IEntitySortable)item).SortIndex = sorts[ndx];
                svc.Update(item);
            }

            return new BoolMessage(true, "Order of items has been saved.");
        }



        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Copy(int id)
        {
            if (!HasAccessTo("Create"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            var svc = EntityRegistration.GetService<T>();
            var entity = svc.Get(id);

            // Check item is there.
            if (entity == null)
                return new EntityActionResult(success: false, message: "Entity has been deleted.");

            // Check security.
            if (!entity.IsOwnerOrAdmin())
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);
                        
            // Reset the id so it's not marked as persistant.
            entity.ToNewCopy();
            return new EntityActionResult(true, string.Empty, null, entity);
        }


        /// <summary>
        /// Show the post with an seo-optimized title.
        /// e.g. my-first-how-to-472 where 472 is the post id.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public virtual EntityActionResult Show(string title)
        {
            if (!HasAccessTo("Read"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            T entity = FindByUrl(title);

            if (entity.IsOkToServe())
                return new EntityActionResult(true, string.Empty, null, entity);

            return new EntityActionResult(success:false, message: "Item not found", isAvailable: false);
        }


        /// <summary>
        /// Show the details of the entity using the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityActionResult Details(int id, Func<int, T> fetcher = null)
        {
            if (!HasAccessTo("Read"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            T entity = fetcher == null ? FindById(id) : fetcher(id);

            if (entity.IsOkToServe())
                return new EntityActionResult(true, string.Empty, null, entity);
            
            return new EntityActionResult(success: false, message: "Item not found", isAvailable: false);
        }


        /// <summary>
        /// Show the details of the entity using the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityActionResult Details(Func<T> fetcher)
        {
            if (!HasAccessTo("Read"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            T entity = fetcher();

            if (entity.IsOkToServe())
                return new EntityActionResult(true, string.Empty, null, entity);

            return new EntityActionResult(success: false, message: "Item not found", isAvailable: false);
        }


        /// <summary>
        /// Get the details using a query based fetcher.
        /// </summary>
        /// <param name="queryFetcher"></param>
        /// <returns></returns>
        public virtual EntityActionResult Details(Func<IQuery<T>> queryFetcher)
        {
            var svc = EntityRegistration.GetService<T>();
            var query = queryFetcher();
            return Details(() => svc.Repository.First(query));
        }


        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult First()
        {
            if (!HasAccessTo("Read"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            var svc = EntityRegistration.GetService<T>();
            var entity = svc.First(null);

            if (entity.IsOkToServe())
                return new EntityActionResult(true, string.Empty, null, entity);

            return new EntityActionResult(true, "Item not found", null, entity, true, false);
        }


        /// <summary>
        /// Finds Items by owner if non-admin is logged in otherwise, all items if admin.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Find(IQuery<T> query)
        {
            var svc = EntityRegistration.GetService<T>();
            var results = svc.Find(query);
            return new EntityActionResult(true, string.Empty, null, results);
        }


        /// <summary>
        /// Finds Items by owner if non-admin is logged in otherwise, all items if admin.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Find(int page, int pageSize, IQuery<T> query)
        {
            var svc = EntityRegistration.GetService<T>();
            var results = svc.Find(query, page, pageSize);
            return new EntityActionResult(true, string.Empty, null, results);
        }


        /// <summary>
        /// Find paged items by using the query, current page, and items per page.
        /// </summary>
        /// <param name="query">The query used to get recent items.</param>
        /// <param name="page">The page of data to get.</param>
        /// <param name="pageSize">The number of items per page to get.</param>
        /// <returns></returns>
        public virtual T FindById(int id)
        {
            var service = EntityRegistration.GetService<T>();
            return service.Get(id);
        }


        /// <summary>
        /// Find paged items by using the query, current page, and items per page.
        /// </summary>
        /// <param name="query">The query used to get recent items.</param>
        /// <param name="page">The page of data to get.</param>
        /// <param name="pageSize">The number of items per page to get.</param>
        /// <returns></returns>
        public virtual T FindByUrl(string url)
        {
            string strId = url.Substring(url.LastIndexOf("-") + 1);
            int id = Convert.ToInt32(strId);
            var service = EntityRegistration.GetService<T>();
            T entity = service.Get(id);
            return entity;
        }

        

        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult FindByRecent(int page, int pageSize, IQuery<T> query = null)
        {
            if (query == null)
                query = Query<T>.New().Where(t => t.Id).MoreThan(1).OrderByDescending(t => t.Id).Limit(pageSize);

            var svc = EntityRegistration.GetService<T>();
            var recents = svc.Find(query, page, pageSize);
            if (recents == null)
                recents = new PagedList<T>(page, pageSize, 0, new List<T>());

            return new EntityActionResult(true, string.Empty, null, recents);
        }


        /// <summary>
        /// Finds Items by owner if non-admin is logged in otherwise, all items if admin.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult FindByOwner(int page, int pageSize, string orderByColumns = null)
        {   
            var svc = EntityRegistration.GetService<T>();
            IQuery<T> query = Auth.IsAdmin()
                            ? Query<T>.New().Where(t => t.CreateUser).NotNull()
                            : Query<T>.New().Where(t => t.CreateUser).Is(Auth.UserShortName);

            if (!string.IsNullOrEmpty(orderByColumns))
            {
                string col = ComLib.Data.DataUtils.BuildSafeColumn(orderByColumns);
                if (!string.IsNullOrEmpty(col))
                    query.OrderBy(col);
            }
            else
                query.OrderByDescending(t => t.CreateDate);

            var results = svc.Find(query, page, pageSize);
            return new EntityActionResult(true, string.Empty, null, results);
        }


        /// <summary>
        /// Find recent items in Json based format.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public EntityActionResult FindByAsJson(int page, int pagesize, IList<PropertyInfo> columnProps, 
            Func<IEntityService<T>, int, int, PagedList<T>> pagedListFetcher = null, Func<int, int,EntityActionResult> actionResultFetcher = null)
        {
            PagedList<T> result = null;
            string message = string.Empty;
            string resultText = string.Empty;
            bool success = false;
            int totalPages = 0;

            try
            {
                EntityActionResult entityresult = null;
                if (actionResultFetcher != null)
                {
                    entityresult = actionResultFetcher(page, pagesize);
                }
                else if (pagedListFetcher != null)
                {
                    var svc = EntityRegistration.GetService<T>();
                    var results = pagedListFetcher(svc, page, pagesize);
                    entityresult = new EntityActionResult(results != null, string.Empty, null, results);
                }
                else
                {
                    entityresult = FindByRecent(page, pagesize);
                }
                result = entityresult.ItemAs<PagedList<T>>();
                totalPages = result.TotalPages;

                try
                {
                    resultText = ComLib.Web.Helpers.JsonHelper.ConvertToJsonString<T>(result, columnProps);
                    success = true;
                }
                catch (Exception ex)
                {
                    message = "Unable to convert items of : " + typeof(T).Name + " to JSON format.";
                    ComLib.Logging.Logger.Error(message, ex);
                }
            }
            catch (Exception ex)
            {
                message = "Unable to get items for : " + typeof(T).Name;
                ComLib.Logging.Logger.Error(message, ex);
                success = false;
            }
            string jsontext = EntityJsonHelper.BuildManageJsonResult<T>(success, message, result, resultText, null);
            return new EntityActionResult(success, message, null, jsontext);
        }


        /// <summary>
        /// Delete the item represented by the id.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Delete(int id)
        {
            if (!HasAccessTo("Delete"))
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            var svc = EntityRegistration.GetService<T>();
            var entity = svc.Get(id);

            // Check item is there.
            if (entity == null)
                return new EntityActionResult(success:false, message : "Entity has been deleted.");

            // Check security.
            if (!entity.IsOwnerOrAdmin())
                return new EntityActionResult(success:false, message : "Not authorized to perform this action", item: entity, isAuthorized: false);
                 
            // Issues deleting?
            svc.Delete(entity);
            if(entity.Errors.HasAny)
                return new EntityActionResult(false, "Unable to delete item.", errors: entity.Errors, item: entity);

            return new EntityActionResult(true, "Item has been deleted.", item: entity);
        }


        /// <summary>
        /// Finds Items by owner if non-admin is logged in otherwise, all items if admin.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public EntityActionResult Manage(int page, int pageSize, IQuery<T> query = null)
        {
            if (!Auth.IsAuthenticated())
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            if(query == null)
               return FindByOwner(page, pageSize);

            return Find(page, pageSize, query);
        }


        /// <summary>
        /// Manage using json. Checks that user is authenticated.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public EntityActionResult ManageUsingJson(int page, int pagesize, IList<PropertyInfo> columnProps,
            Func<IEntityService<T>, int, int, PagedList<T>> pagedListFetcher = null, Func<int, int, EntityActionResult> actionResultFetcher = null)
        {
            if (!Auth.IsAuthenticated())
                return new EntityActionResult(success: false, message: "Not authorized to perform this action", isAuthorized: false);

            return FindByAsJson(page, pagesize, columnProps, pagedListFetcher, actionResultFetcher);
        }


        /// <summary>
        /// Whether or not the current user has access to the following action.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        protected virtual bool HasAccessTo(string action)
        {
            if (_settings == null || string.IsNullOrEmpty(action))
                return false;
            
            string actionlcase = action.ToLower();

            if (actionlcase == "create")
                return _settings.HasAccessToCreate<T>();
            else if (actionlcase == "edit")
                return _settings.HasAccessToEdit<T>();
            else if (actionlcase == "read" || actionlcase == "view")
                return _settings.HasAccessToView<T>();
            else if (actionlcase == "delete")
                return _settings.HasAccessToDelete<T>();
            else if (actionlcase == "index")
                return _settings.HasAccessToIndex<T>();
            else if (actionlcase == "import")
                return _settings.HasAccessToImport<T>();

            return _settings.HasAccessTo(typeof(T).Name, action);
        }
    }
}
