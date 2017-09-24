using OneDiscovery.Data;
using OneDiscovery.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace OneDiscovery.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        //Constructor dependency injection
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public User GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }
    

        public IEnumerable<User> GetUserByName(string name)
        {
            return _userRepository.GetMany(s => s.LastName.Contains(name) || s.FirstName.Contains(name)).ToList();
        }

        public void UpdateUser(User user)
        {
            //use transaction for actomisity and consistency
            using (var transaction = new TransactionScope())
            {
                _userRepository.Update(user);
                _userRepository.SaveChanges();
                transaction.Complete();
            }
        }

        //use email as username for login or registion purpose
        public User GetUserByUserName(string username)
        {
            var user = _userRepository.Get(p => p.Email == username);
            return user;
        }

        public void CreateUser(User user)
        {
            using (var transaction = new TransactionScope())
            {
                _userRepository.Add(user);
                _userRepository.SaveChanges();
                transaction.Complete();
            }
        }
    }
    public interface IUserService
    {
        
        User GetUserById(int id);
        User GetUserByUserName(string username);
        IEnumerable<User> GetUserByName(string name);
        void CreateUser(User user);
        void UpdateUser(User user);
    }
}
