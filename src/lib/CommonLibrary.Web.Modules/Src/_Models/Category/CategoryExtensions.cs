using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

using ComLib.Caching;
using ComLib.Data;
using ComLib.Entities;
using ComLib.ValidationSupport;
using ComLib.Patterns;
using ComLib.Web.Lib.Attributes;

namespace ComLib.Web.Modules.Categorys
{
    /// <summary>
    /// Extended behaviour of the category class.
    /// </summary>
    [Model(Id = 6, DisplayName = "Category", Description = "Category", IsPagable = true,
        IsExportable = true, IsImportable = true, FormatsForExport = "xml,csv,ini", FormatsForImport = "xml,csv,ini",
        RolesForCreate = "${Admin}", RolesForView = "?", RolesForIndex = "?", RolesForManage = "${Admin}",
        RolesForDelete = "${Admin}", RolesForImport = "${Admin}", RolesForExport = "${Admin}")]
    public partial class Category : ActiveRecordBaseEntity<Category>, IEntity, INodeWithIds
    {
        /// <summary>
        /// Title of the parent of this category.
        /// </summary>
        public string ParentTitle { get; set; }


        /// <summary>
        /// Is root category?
        /// </summary>
        public bool IsRoot
        {
            get { return ParentId <= 0; }
        }


        /// <summary>
        /// To Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ToUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return Id.ToString();

            return url + Id;
        }


        /// <summary>
        /// Set the parent id based on the parent title.
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static void Create(IList<Category> categories, bool checkFields, params Expression<Func<Category, object>>[] fields)
        {
            if (!checkFields)
                Create(categories);
            else
            {
                foreach (var category in categories)
                {
                    if (!string.IsNullOrEmpty(category.ParentTitle))
                    {
                        var parent = Category.Repository.First(Query<Category>.New().Where(c => c.Name).Is(category.ParentTitle).And( c => c.Group).Is(category.Group));
                        if (parent != null)
                            category.ParentId = parent.Id;
                        else
                            category.ParentTitle = string.Empty;
                    }
                    Create(new List<Category>() { category }, fields);
                }
            }
        }


        #region Validation
        protected override IValidator GetValidatorInternal()
        {
            var val = new EntityValidator((validationEvent) =>
            {
                int initialErrorCount = validationEvent.Results.Count;
                IValidationResults results = validationEvent.Results;
                Category entity = (Category)validationEvent.Target;
                Validation.IsNumericWithinRange(entity.AppId, false, false, -1, -1, results, "AppId");
                Validation.IsStringLengthMatch(entity.Name, false, true, true, 1, 25, results, "Name");
                Validation.IsStringLengthMatch(entity.Description, true, true, true, 1, 20, results, "Description");
                Validation.IsStringLengthMatch(entity.Group, true, false, true, -1, 25, results, "Group");
                Validation.IsNumericWithinRange(entity.SortIndex, false, false, -1, -1, results, "SortIndex");
                Validation.IsNumericWithinRange(entity.Count, false, false, -1, -1, results, "Count");
                Validation.IsNumericWithinRange(entity.ParentId, false, false, -1, -1, results, "ParentId");

                return initialErrorCount == validationEvent.Results.Count;
            });
            return val;
        }
        #endregion


        /// <summary>
        /// Get the categories in the group specified as nodes.
        /// </summary>
        /// <param name="group">The group the node belong in. e.g. "Posts", or "Events"</param>
        /// <param name="allCategories">If true, get all categories in node structure(parent, child), false, gets only parent level node.</param>
        /// <returns></returns>
        public static Node<Category> ToNodes(string group, bool allCategories)
        {
            string key = "category_nodes_" + group;
            Func<Node<Category>> fetcher = () =>
            {
                IList<Category> categories = null;
                // Get all the categories.
                if (allCategories)
                    categories = Category.Find(Query<Category>.New().Where(c => c.Group).Is(group).OrderBy(c => c.ParentId).OrderBy(c => c.SortIndex));
                else
                    categories = Category.Find(Query<Category>.New().Where(c => c.Group).Is(group).And(c => c.ParentId).Is(0).OrderBy(c => c.SortIndex));
            
                var nodes = Node<Category>.ToNodes<Category>(categories);
                return nodes;
            };
            Node<Category> groupNodes = Cacher.Get<Node<Category>>(key, 300, fetcher);
            if(groupNodes == null || !groupNodes.HasChildren )
            {
                groupNodes = fetcher();
                Cacher.Insert(key, groupNodes, 300, false);
            }
            
            return groupNodes;
        }


        /// <summary>
        /// Get a dictionary of id's to categorys that are associated w/ the TEntity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static LookupMulti<Category> LookupFor<TEntity>()
        {
            string key = "category_lookup_" + typeof(TEntity).Name;            
            var lookup = Cacher.Get<LookupMulti<Category>>(key, 300, () => LookupForInternal<TEntity>());
            if (lookup == null || lookup.Lookup1.Count == 0)
            {
                lookup = LookupForInternal<TEntity>();
                Cacher.Insert(key, lookup, 300, false);
            }

            return lookup;
        }


        /// <summary>
        /// Create a lookup by category id and by parent,child names.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        private static LookupMulti<Category> LookupForInternal<TEntity>()
        {
            var query = Query<Category>.New().Where(c => c.Group).Is(typeof(TEntity).Name).OrderBy(c => c.ParentId).OrderBy(c => c.SortIndex);
            var categories = Repository.Find(query);
            IDictionary<int, Category> idMap = new Dictionary<int, Category>();
            IDictionary<string, Category> nameMap = new Dictionary<string, Category>();

            // Store the ids.
            foreach (var category in categories) idMap[category.Id] = category;

            // Now store the names.
            foreach (var category in categories)
            {
                if (category.ParentId <= 0)
                    nameMap[category.Name] = category;
                else if(idMap.ContainsKey(category.ParentId))
                {
                    var parent = idMap[category.ParentId];
                    nameMap[parent.Name + "," + category.Name] = category;
                }
            }
            return new LookupMulti<Category>(idMap, nameMap);
        }
    }
}
