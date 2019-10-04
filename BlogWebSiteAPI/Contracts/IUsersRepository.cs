using BlogWebSiteAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSiteAPI.Contracts
{
    public interface IUsersRepository
    {
        Task<Users> Add(Users user);

        IEnumerable<Users> GetAll();

        Task<Users> Find(string userName, string password);

        bool FindDuplicateEmail(string Email);

        bool FindDuplicateUserName(string userName);

        Task<Users> Update(Users user);

        Task<Users> Remove(int id);

        Task<bool> Exist(int id);
    }
}
