using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib;
using ComLib.Caching;
using ComLib.Account;
using ComLib.Notifications;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Attributes;


namespace ComLib.CMS.Areas.Admin
{
    [AdminAuthorization]
    public class UserController : JsonController<User>
    {
        /// <summary>
        /// OVerride the url to use after creation.
        /// </summary>
        protected override void Init()
        {
            InitColumns(
               columnNames: new List<string>() { "Id", "Name", "Email", "Roles", "Approved", "Locked", "Last Login" },
               columnProps: new List<Expression<Func<User, object>>>() { a => a.Id, a => a.UserName, a => a.Email, a => a.Roles, a => a.IsApproved, a => a.IsLockedOut, a => a.LastLoginDate },
               columnWidths: null, customRowBuilder: "UserGridBuilder");
        }


        /// <summary>
        /// Deleting the user involves several steps. This needs to be tested before being made available.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override ActionResult Delete(int id)
        {
            return Json(new { Success = false, Message = "Delete not currently supported" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Remove the cache entry with the supplied name.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public ActionResult Approve(string userName)
        {
            var result = ComLib.Account.User.Approve(userName);
            var message = result.Success ? "User has been approved" : "Failed to approve user";
            return Json(new { Success = result.Success, Message = message }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Lock the user out of the system.
        /// </summary>
        /// <returns></returns>
        public ActionResult LockOut(string userName)
        {
            ComLib.Account.User.LockOut(userName, string.Empty);
            return Json(new { Success = true, Message = "User has been locked out" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Unlock a user from the system if he is locked out.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult UndoLockOut(string userName)
        {
            ComLib.Account.User.UndoLockOut(userName, string.Empty);
            return Json(new { Success = true, Message = "Undo lockout successful" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Send the user a password reminder.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult SendPassword(string userName)
        {
            User user = ComLib.Account.User.Get(userName);
            Notifier.RemindUserPassword(user.Email, "Password reminder", user.UserName, user.UserName, user.Password);
            return Json(new { Success = true, Message = "Reminder has been sent" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Increment the users warning count which goes up when he does something not allowed.
        /// e.g. Spamming users.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult Warn(string userName)
        {
            // Not implemented.
            return Json(new { Success = true, Message = "This feature is not yet implemented." }, JsonRequestBehavior.AllowGet);
        }
    }
}
