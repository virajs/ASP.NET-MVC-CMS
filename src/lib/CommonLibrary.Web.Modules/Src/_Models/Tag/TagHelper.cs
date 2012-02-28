using System;
using System.Collections.Generic;
using System.Linq;
using ComLib;
using ComLib.Data;
using ComLib.Web.Lib.Core;
using ComLib.Entities;

namespace ComLib.Web.Modules.Tags
{
    /// <summary>
    /// DTO object for handling tags on any entity.
    /// </summary>
    public class TagsEntry
    {
        public string Tags;
        public int GroupId;
        public int RefId;
    }



    /// <summary>
    /// Helper class for Tags.
    /// </summary>
    public class TagHelper
    {
        /// <summary>
        /// Iterate over each widget/instance in the specified zone.
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="executor"></param>
        public static List<Tag> ConvertToTags(string tagsAsString, int groupid, int refid)
        {
            if (string.IsNullOrEmpty(tagsAsString))
                return null;

            string[] tokens = tagsAsString.ToLower().Trim().Split(' ');
            if (tokens == null || tokens.Length == 0)
                return null;

            var tags = new List<Tag>();
            foreach (string token in tokens)
                tags.Add(new Tag() { GroupId = groupid, RefId = refid, Name = token });

            return tags;
        }


        /// <summary>
        /// Process the entries.
        /// </summary>
        /// <param name="itemsToProcess"></param>
        public static void ProcessQueue(IList<TagsEntry> itemsToProcess)
        {
            if (itemsToProcess == null || itemsToProcess.Count == 0)
                return;

            foreach (var entry in itemsToProcess)
            {
                string error = string.Format("Unable to apply tags for GroupId[{0}], RefId[{1}], Tags[{2}]", entry.GroupId, entry.RefId, entry.Tags);
                Try.CatchLog(error, () =>
                {
                    // Delete existing tags for the associated group/ref id combination.
                    Tag.Repository.Delete(Query<Tag>.New().Where(t => t.GroupId).Is(entry.GroupId)
                                                                  .And(t => t.RefId).Is(entry.RefId));

                    // Convert string to Tag objects.
                    List<Tag> tags = TagHelper.ConvertToTags(entry.Tags, entry.GroupId, entry.RefId);
                    if (tags != null && tags.Count > 0)
                    {
                        foreach (var tag in tags) Tag.Create(tag);
                    }
                });
            }
        }


        /// <summary>
        /// Gets entity ids associated with the tag name and group specified by type (T).
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static int[] GetReferenceIdsForTagsFor<T>(string tag)
        {
            int groupId = ModuleMap.Instance.GetId(typeof(T));
            return GetReferenceIdsForTags(groupId, tag);
        }


        /// <summary>
        /// Gets entity ids associated with the tag name and group specified by type (T).
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static int[] GetReferenceIdsForTags(int groupId, string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return new int[] { };

            tag = tag.Trim().ToLower();
            string[] tagsTokens = null;

            if (tag.Contains(","))
                tagsTokens = tag.Split(',');
            else if (tag.Contains(' '))
                tagsTokens = tag.Split(' ');
            else
                tagsTokens = new string[1] { tag };

            IList<Tag> tags = null;

            if (tagsTokens.Length == 1)
            {
                tags = Tag.Find(Query<Tag>.New().Where(t => t.GroupId).Is(groupId).And(t => t.Name).Is(tagsTokens[0]));
            }
            else if (tagsTokens.Length > 1)
            {
                tags = Tag.Find(Query<Tag>.New().Where(t => t.GroupId).Is(groupId).And(t => t.Name).In<string>(tagsTokens));
            }


            // Check if tags available.
            if (tags == null || tags.Count == 0)
                return new int[] { };

            var ids = from t in tags select t.RefId;
            int[] idsarray = ids.ToArray();

            // Check for empty ids.
            if (idsarray == null || idsarray.Length == 0)
                return new int[] { };

            return idsarray;
        }


        /// <summary>
        /// Gets the by tags.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns></returns>
        public static IList<T> GetByTags<T>(string tag, Func<int[], IQuery<T>> queryBuilder)
        {
            int groupId = ModuleMap.Instance.GetId(typeof(T));

            // This is an NON-Optimized query done in 2 steps.
            // This is just to have compatibility between both the in-memory repository and real database.
            // Step 1: Get all the tags to get the refid's
            IList<Tag> tags = Tag.Find(Query<Tag>.New().Where(t => t.GroupId).Is(groupId).And(t => t.Name).Is(tag));

            // Check if tags available.
            if (tags == null || tags.Count == 0)
                return new List<T>();

            var ids = from t in tags select t.RefId;
            int[] idsarray = ids.ToArray();

            // Check for empty ids.
            if (idsarray == null || idsarray.Length == 0)
                return new List<T>();

            // Step 2:  Now get blogposts.
            var service = EntityRegistration.GetService<T>();
            var query = queryBuilder(idsarray);
            IList<T> items = service.Find(query);
            return items;
        }
    }
}
