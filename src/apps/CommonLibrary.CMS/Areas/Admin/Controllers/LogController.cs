using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib;
using ComLib.Data;
using ComLib.Entities;
using ComLib.Caching;
using ComLib.Logging;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Attributes;


namespace ComLib.CMS.Areas.Admin
{
    public class LogViewModel
    {
        public EntityListViewModel<LogEventEntity> PagerView;
        public SelectList LogLevels;


        public LogViewModel(PagedList<LogEventEntity> items, string selectedLevel)
        {
            PagerView = new EntityListViewModel<LogEventEntity>(items, "/admin/log/index/", true);
            var levels = new List<string>() { "Debug", "Error", "Fatal", "Info", "Message", "Warn" };
            LogLevels = new SelectList(levels, selectedLevel);
        }
    }


    [AdminAuthorization]
    public class LogController : Controller
    {
        /// <summary>
        /// Get list of all the entries in the Cache.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int ? page)
        {
            
            // Check for deleting by loglevel.
            // Doing it this way actually defeats the purpose of MVC.
            // However, this was done to just get the initial functionality in without messing 
            // w/ too much java script.
            string logLevel = this.Request.Params["LogLevel"];
            if (string.IsNullOrEmpty(logLevel)) logLevel = "Debug";
            string action = this.Request.Params["LogAction"];
            
            // Check action.
            if (!string.IsNullOrEmpty(action))
                if (string.Compare(action, "DeleteByLevel", true) == 0)
                    DeleteByLevel(logLevel);

            int pageIndex = (page == null) ? 1 : (int)page;
            var query = Query<LogEventEntity>.New().Where(l => l.Id).MoreThan(1).OrderByDescending(l => l.CreateDate);
            PagedList<LogEventEntity> entries = LogEventEntity.Find(query, pageIndex, 30);
            return View("List", new LogViewModel(entries, logLevel));
        }


        /// <summary>
        /// Remove all the log entries.
        /// </summary>
        /// <param name="cacheKey"></param>
        /// <returns></returns>
        public ActionResult DeleteAll()
        {
            // Need to handle errors.
            LogEventEntity.DeleteAll();
            return Index(null);
        }


        public ActionResult FlushAll()
        {
            Logger.Flush();
            return Index(null);
        }


        /// <summary>
        /// Remove all the logs based on their log level.
        /// </summary>
        /// <returns></returns>        
        public void DeleteByLevel(string level)
        {
            // Need to handle errors.
            //string level = form["LogLevel"];
            LogLevel levelToDelete = (LogLevel)Enum.Parse(typeof(LogLevel), level, true);
            LogEventEntity.Delete(levelToDelete);
        }


        /// <summary>
        /// Remove a log entry by it's id.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult DeleteById(int id)
        {
            // Need to handle errors.
            LogEventEntity.Delete(id);
            return Index(null);
        }


        /// <summary>
        /// Remove all the log entries before the specified date.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult DeleteByDate(string date)
        {
            // Need to handle errors.
            try
            {
                DateTime dateFilter = DateTime.Parse(date);
                LogEventEntity.Delete(dateFilter, true);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
            }
            return Index(null);
        }


        /// <summary>
        /// Get the total number of log entries in the system.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTotal()
        {
            return Index(null);
        }
    }
}
