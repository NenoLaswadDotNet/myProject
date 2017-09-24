using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneDiscovery.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult AccessDenied()
        {
            return View();
        }
        // GET: Error
        public ActionResult ErrorPage()
        {
            return View("ErrorPage");
        }
        public ActionResult NotFound()
        {
            return View("ErrorPage");
        }
    }
}