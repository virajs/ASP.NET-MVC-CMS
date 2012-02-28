using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using ComLib.Authentication;
using ComLib.Entities;
using ComLib.Data;
using ComLib.Extensions;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Core;

namespace ComLib.Web.Modules.Favorites
{
    /// <summary>
    /// Controller for Dynamic Html content pages.
    /// </summary>
    [HandleError]
    public class FavoriteController : JsonController<Favorite>
    {

        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Title", "Model", "RefId", "Url" },
               columnProps: new List<Expression<Func<Favorite, object>>>() { a => a.Id, a => a.Title, a => a.Model, a => a.RefId, a => a.Url },
               columnWidths: null, customRowBuilder: "FavoriteGridBuilder");
        }


        /// <summary>
        /// Adds the specified model.
        /// </summary>
        /// <param name="model">The model name. e.g. "Post" or "Event".</param>
        /// <param name="refid">The refid or id of the post being favored.</param>
        /// <param name="title">The title. The title of the post.</param>
        /// <param name="url">The URL of the post.</param>
        [HttpGet]
        public ActionResult Add(string model, int refid, string title, string url)
        {
            // Already favored?
            int count = Favorite.Repository.Count(Query<Favorite>.New().Where(f => f.RefId).Is(refid).And(f => f.Model).Is(model).And(f => f.UserId).Is(Auth.UserId));
            if (count >= 1)
                return Json(new { Success = true, Message = "You have already added this as a favorite" }, JsonRequestBehavior.AllowGet);

            // This is called asychronously via JSON.
            // No need to return anything although a success/failure message would be good.
            Favorite fav = new Favorite() { Model = model, RefId = refid, Title = title, Url = url };
            fav.Create();
            bool success = !fav.Errors.HasAny;
            string message = success ? "Added " + model + " as a favorite" : "Unable to add " + model + " as a favorite. " + fav.Errors.Message("<br/>");
            return Json(new { Success = success, Message = message }, JsonRequestBehavior.AllowGet);
        }
    }
}
