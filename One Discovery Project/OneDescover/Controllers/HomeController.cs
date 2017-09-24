using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using OneDiscovery.ViewModels;
using OneDiscovery.Infrastructure;
using Contoso.Filters;

namespace OneDiscovery.Controllers
{
    //Custom Exception Handling Filter
    [OneDiscoveryExceptionFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null)
                {
                    Session["UserName"] = authTicket.Name;
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}