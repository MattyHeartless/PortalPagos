using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalPagos.Controllers
{
    public class SessionController : Controller
    {
        // GET: Session
        public JsonResult StartSession(string username, string key)
        {
            if (username == "15975346" && key == "jaja")
            {

                System.Web.HttpContext.Current.Session["Usuario"] = "15975346";
                System.Web.HttpContext.Current.Session["Nombre"] = "Pedro Paramo";
                System.Web.HttpContext.Current.Session["Session"] = "session_created";
                System.Web.HttpContext.Current.Session.Timeout = 99999;
                return Json("OK");
            }
            else
                return Json("Error");

        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}