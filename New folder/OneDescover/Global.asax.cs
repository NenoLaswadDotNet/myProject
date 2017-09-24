using Newtonsoft.Json;
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

namespace OneDiscovery
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //Check if the user is authenticated by the cookie
        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                if (authTicket != null)
                {
                    //var serializeModel = JsonConvert.DeserializeObject<OneDiscoveryPrincipleModel>(authTicket.UserData);
                    var newUser = new OneDiscoveryPrincipal(authTicket.Name)
                    {
                        UserName = authTicket.Name
                        
                    };
                    
                    HttpContext.Current.User = newUser;
                }
            }
        }

    }
}
