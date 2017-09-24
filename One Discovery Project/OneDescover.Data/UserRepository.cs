using OneDiscovery.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDiscovery.Data
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(OneDiscoveryDbContext context) : base(context)
        {
        }

        //For selecting by lastName
        public User GetUserByLastName(string lastName)
        {
            var user = _context.Users.OfType<User>().FirstOrDefault(s => s.LastName == lastName);
            return user;
        }
    }

    public interface IUserRepository : IRepository<User>
    {
        User GetUserByLastName(string lastName);
    }
}
