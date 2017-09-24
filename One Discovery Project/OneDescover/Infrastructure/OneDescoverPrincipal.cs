using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace OneDiscovery.Infrastructure
{
    //Inherient from Iprincipal to add more property for authorization
    public class OneDiscoveryPrincipal : IPrincipal
    {
        public bool IsInRole(string role)
        {
            return Roles.Any(r => r.Contains(role));
        }

        public OneDiscoveryPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);
        }

        public IIdentity Identity { get; private set; }
        public string UserName { get; set; }
        public string[] Roles { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}