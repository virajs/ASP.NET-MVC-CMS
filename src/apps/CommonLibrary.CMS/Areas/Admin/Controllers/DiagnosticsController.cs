using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Collections;

using ComLib.Diagnostics;
using ComLib.Web.Lib.Attributes;


namespace ComLib.CMS.Areas.Admin
{
    [AdminAuthorization]
    public class DiagnosticsController : Controller
    {
        //
        // GET: /Diagnostics/

        public ActionResult Index()
        {
            List<KeyValuePair<string, IDictionary>> data = new List<KeyValuePair<string, IDictionary>>();
            var machinedata = Diagnostics.Diagnostics.GetDataAsDictionary(DiagnosticGroup.MachineInfo);
            var machinedatamap = machinedata["MachineInfo"] as IDictionary;
            data.Add(new KeyValuePair<string,IDictionary>( "Machine Info", machinedatamap));
            return View("List", data);
        }
    }
}
