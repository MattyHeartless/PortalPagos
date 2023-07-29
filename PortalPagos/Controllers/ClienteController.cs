using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PortalPagos.Controllers
{
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Dashboard()
        {
            if (Session != null && Session["Session"] != null && Session["Session"].ToString() == "session_created")
            {
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}