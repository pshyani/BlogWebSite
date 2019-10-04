using BlogWebSiteAPI.Contracts;
using BlogWebSiteAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSiteAPI.Repositories
{
    public class BlogsRepository : IBlogsRepository
    {
        private BlogSiteContext _context;

        public BlogsRepository(BlogSiteContext context)
        {
            _context = context;
        }
        public async Task<Blogs> Add(Blogs blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return blog;
        }

        public async Task<bool> Exist(int id)
        {
            return await _context.Blogs.AnyAsync(P => P.BlogId == id);
        }

        public async Task<Blogs> Find(int id)
        {
            var dbBlogs = await _context.Blogs.Include(p=>p.User).SingleOrDefaultAsync(q => q.BlogId == id);       
            return dbBlogs;

        }

        public IEnumerable<Blogs> GetAll(string strUserName)
        {
            var blogs = _context.Blogs;

            if (!string.IsNullOrEmpty(strUserName))
            {
                var userBlogs = new List<Blogs>();
                foreach(var blog in blogs)
                {
                    if (blog.User.UserName == strUserName)
                        userBlogs.Add(blog);                   
                }
                return userBlogs;
            }

            return blogs;
        }

        public async Task<Blogs> Remove(int id)
        {
            try
            {
                var blog = await _context.Blogs.Include(P =>P.BlogComments).SingleAsync(P => P.BlogId == id);

                foreach (var bc in blog.BlogComments)
                    _context.BlogComments.Remove(bc);

                _context.Blogs.Remove(blog);
                await _context.SaveChangesAsync();
                return blog;
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<Blogs> Update(Blogs blog)
        {
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return blog;
        }
    }
}
