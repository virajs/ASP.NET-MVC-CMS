using System.Web.Mvc;

namespace ComLib.CMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute("Admin_index", "Admin/{controller}/index/{page}", new { action = "Index", page = "" });
            context.MapRoute("Admin_default", "Admin/{controller}/{action}/{id}", new { action = "", id = "" });
        }
    }
}
