using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using ComLib.Web.Lib.Models;
using ComLib.Web.Lib.Core;
using ComLib.Web.Lib.Helpers;
using ComLib.Web.Modules.Themes;
using ComLib.Web.Modules.Flags;
using ComLib.Web.Modules.Media;
using ComLib.Web.Lib.ViewModels;

namespace ComLib.CMS.Controllers
{
    [HandleError]
    public class HomeController : CommonController
    {
        public ActionResult Index()
        {
            SetPageTitle("Home");
            ViewData["Message"] = "Welcome to ASP.NET MVC 2 App Template - Using CommonLibrary.NET!";
            return View();
        }


        public ActionResult CssTest()
        {
            return View();
        }



        public ActionResult Frame()
        {
            return View();
        }


        public ActionResult Recents()
        {
            return View();
        }


        public ActionResult Location()
        {
            return View();
        }


        public ActionResult Images()
        {
            return View();
        }


        public ActionResult DragDrop()
        {
            return View();
        }


        [HttpGet]
        public ActionResult HiddenVal()
        {
            return View();
        }


        [HttpPost]
        public ActionResult HiddenVal2()
        {
            string val = this.Request.Params["hdnModelName"];
            return View("HiddenVal");
        }


        public ActionResult AjaxTest()
        {
            return View();
        }


        public ActionResult PagerAjax()
        {
            return View();
        }


        /// <summary>
        /// 2, "comment", "refid".
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modeltype"></param>
        /// <returns></returns>
        public ActionResult ReportFlag(int id, string modeltype, int refid)
        {

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);    
        }


        /// <summary>
        /// Previews the theme.
        /// </summary>
        /// <param name="themeName">Name of the theme.</param>
        /// <returns></returns>
        public ActionResult PreviewTheme(string themename)
        {
            Theme theme = Theme.Lookup[themename];
            
            // Handle this better.
            if (theme == null)
            {
                Logging.Logger.Error("Unknow theme, can not preview : " + themename);
                return View("Index");
            }

            // Update the theme in the request context.
            string masterpage = theme != null ? theme.SelectedLayout : "Site";
            string nameofTheme = theme != null ? theme.Name : "Sapphire";
            ViewHelper.SetThemeInCurrentRequest(nameofTheme, masterpage, theme);
            return View("Index");
        }
    }
}
