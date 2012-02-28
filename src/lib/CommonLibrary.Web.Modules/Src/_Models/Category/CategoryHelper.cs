using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.Data;
using ComLib.Patterns;
using ComLib.Entities;
using ComLib;

namespace ComLib.Web.Modules.Categorys
{
    public class CategoryHelper
    {
        /// <summary>
        /// Find items by category
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static Tuple2<PagedList<T>, Category> FindCategoryAndItems<T>(int id, int page, int pageSize, string categoryIdFieldName)
        {
            IList<T> posts = null;
            Node<Category> nodes = Categorys.Category.ToNodes(typeof(T).Name, true);
            if (nodes.Root[id] == null)
                return new Tuple2<PagedList<T>, Category>(new PagedList<T>(1, 1, 0, posts), null);

            Node<Category> category = nodes.Root[id];
            IQuery<T> query = !category.Item.IsRoot
                                ? Query<T>.New().Where(categoryIdFieldName).Is(id)
                                : Query<T>.New().Where(categoryIdFieldName).Is(id).Or(categoryIdFieldName).In<int>(CategoryHelper.GetChildIdsFor(nodes, id));

            var service = EntityRegistration.GetService<T>();
            PagedList<T> items = service.Find(query, page, pageSize);
            return new Tuple2<PagedList<T>, Category>(items, category.Item);
        }


        /// <summary>
        /// Find items by cateogry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryIdFieldName"></param>
        /// <returns></returns>
        public static PagedList<T> FindByCategory<T>(int id, int page, int pageSize, string categoryIdFieldName)
        {
            var result = FindCategoryAndItems<T>(id, page, pageSize, categoryIdFieldName);
            return result.First;
        }


        /// <summary>
        /// Get the Parent category name for the category id supplied.
        /// </summary>
        /// <typeparam name="T">The group the category is associated with. eg. Event, Post.</typeparam>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static string FindParentCategoryNameFor<T>(int categoryId)
        {
            var lookup = Category.LookupFor<T>();
            Category category = lookup[categoryId];
            if (category == null) return string.Empty;

            if (category.IsRoot) return category.Name;

            Category parent = lookup[category.ParentId];
            if (parent == null) return category.Name;

            return parent.Name;
        }


        /// <summary>
        /// Builds a select option list of categories for the type specified.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string BuildCategoriesFor<T>(string id, string childOptionSpacer, bool includeEmptyCategory = false, string emptyCategoryName = "", int emptyCategoryValue = -1)
        {   
            //<select id="SkillText" name="SkillText"><option selected="selected" value="All">All</option> 
            //   <option value="Beginner">Beginner</option> 
            //   <option value="Intermediate">Intermediate</option> 
            //   <option value="Advanced">Advanced</option> 
            //</select> 
            string group = typeof(T).Name;
            if (string.IsNullOrEmpty(childOptionSpacer))
                childOptionSpacer = "&nbsp;&nbsp;&nbsp;";

            Node<Category> nodes = Category.ToNodes(group, true);
            StringBuilder buffer = new StringBuilder();

            if (nodes == null || !nodes.HasChildren) return string.Empty;

            if(includeEmptyCategory)
                buffer.Append("<option value=\"" + emptyCategoryValue + "\">" + emptyCategoryName + "</option>");

            // Parent categories.            
            for (int ndx = 0; ndx < nodes.Children.Count; ndx++)
            {
                Node<Category> node = nodes.Children[ndx];
                buffer.Append("<option value=\"" + node.Item.Id + "\">" + node.Item.Name + "</option>");

                if (node.HasChildren)
                {
                    // Iterate over children.
                    for (int subndx = 0; subndx < node.Children.Count; subndx++)
                    {
                        Node<Category> child = node.Children[subndx];
                        buffer.Append("<option value=\"" + child.Item.Id + "\">" + childOptionSpacer + child.Item.Name + "</option>");
                    }
                }
            }
            string options = "<select id=\"" + id + "\" name=\"" + id + "\">" + buffer.ToString()
                           + "</select>";
            return options;
        }


        /// <summary>
        /// Get the id and the ids of the children nodes of the category w/ the id.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int[] GetIdAndChildIdsFor(Node<Category> nodes, int id)
        {
            Node<Category> category = nodes.Root[id];
            if (category == null) return null;

            int[] ids = new int[category.Count + 1];
            ids[0] = id;
            if (category.HasChildren)
            {
                for (int ndx = 0; ndx < category.Count; ndx++)
                {
                    var cat = category.Children[ndx];
                    ids[ndx] = cat.Item.Id;
                }
            }
            return ids;
        }


        /// <summary>
        /// Gets all the child ids of the node specified by id.
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static int[] GetChildIdsFor(Node<Category> nodes, int id)
        {
            Node<Category> category = nodes.Root[id];
            if (category == null) return null;

            int[] ids = new int[category.Count];
            if (category.HasChildren)
            {
                for (int ndx = 0; ndx < category.Count; ndx++)
                {
                    var cat = category.Children[ndx];
                    ids[ndx] = cat.Item.Id;
                }
            }
            return ids;
        }
    }
}
