using BlogWebSiteAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebSiteAPI.Contracts
{
    public interface IBlogCommentsRepository
    {
        Task<BlogComments> Add(BlogComments blogcomment);

        IEnumerable<BlogComments> GetAll(int blogId);

        Task<BlogComments> Find(int id);

        Task<BlogComments> Update(BlogComments blogcomment);

        Task<BlogComments> Remove(int id);

        Task<bool> Exist(int id);
    }
}
