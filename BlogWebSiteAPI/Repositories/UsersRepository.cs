using BlogWebSiteAPI.Contracts;
using BlogWebSiteAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSiteAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private BlogSiteContext _context;

        public UsersRepository(BlogSiteContext context)
        {
            _context = context;
        }
        public async Task<Users> Add(Users user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Users.AnyAsync(P => P.UserId == id);
        }

        public async Task<Users> Find(string userName, string password)
        {
            var dbuser = await _context.Users.SingleOrDefaultAsync(q => q.UserName == userName && q.PasswordHash == password);
            return dbuser;

        }
        public bool FindDuplicateEmail(string email)
        {
            if (_context.Users.Any(P => P.Email == email))
                return true;
            else
                return false;
        }

        public bool FindDuplicateUserName(string userName)
        {
            if (_context.Users.Any(P => P.UserName == userName))
                return true;
            else
                return false;
        }
        public IEnumerable<Users> GetAll()
        {
            return _context.Users;
        }

        public async Task<Users> Remove(int id)
        {
            var user = await _context.Users.SingleAsync(P => P.UserId == id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Users> Update(Users user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
