using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsentManagement.Models;
using ConsentManagement;
using System.IO;

namespace ConsentManagement.Controllers
{
    public class FormController : Controller
    {
        [AspAuthen]
        public ActionResult Index()
        {
            Session["ProCode"] = ConsentManagement.Controllers.LoginController.ProgramCode;
            Session["ProVersion"] = ConsentManagement.Controllers.LoginController.ProgramVersion;
            //ViewBag.MKTIPermission = UserManagement.ReadMenuPermissionForMKTI(Session["usr"].ToString());
            //ViewBag.eDoc_eTaxPermission = UserManagement.ReadProgramCode(Session["usr"].ToString(),"e-Doc&e-Tax");
            return View("MainMenu");
        }
    }
}