using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDiscovery.Model;

namespace OneDiscovery.Data
{
    //Code first DbContext
    public class OneDiscoveryDbContext : DbContext
    {
        public OneDiscoveryDbContext() : base("Name=OneDiscoveryDbContext")
        {
            //this.Configuration.LazyLoadingEnabled = false;
        }
        public DbSet<User> Users { get; set; }
    }
}
