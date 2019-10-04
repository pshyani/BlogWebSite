using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebSiteAPI.Models;
using BlogWebSiteAPI.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BlogWebSiteAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Blogs")]
    public class BlogsController : Controller
    {
        private readonly IBlogsRepository _blogsRepository;
        private readonly IUsersRepository _bUsersRepository;
        public BlogsController(IBlogsRepository blogsRepository, IUsersRepository bUsersRepository)
        {
            _blogsRepository = blogsRepository;
            _bUsersRepository = bUsersRepository;
        }

        private async Task<bool> BlogExists(int id)
        {
            return await _blogsRepository.Exist(id);
        }

        [HttpGet]
        [Route("GetBlogs/{UserName?}")]
        [Produces(typeof(DbSet<Blogs>))]
        public IActionResult GetBlogs([FromRoute] string UserName)
        {
            var results = new ObjectResult(_blogsRepository.GetAll(UserName))
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            Request.HttpContext.Response.Headers.Add("X-Total-Count", _blogsRepository.GetAll(UserName).Count().ToString());

            return results;
        }

        [HttpGet("GetBlog/{id}")]
        [Produces(typeof(Blogs))]
        public async Task<IActionResult> GetBlog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blog = await _blogsRepository.Find(id);

            if (blog == null)
            {
                return NotFound();
            }

            var results = new ObjectResult(blog)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            return results;
        }

        [HttpPut("{id}")]
        [Produces(typeof(Blogs))]
        public async Task<IActionResult> PutBlog([FromRoute] int id, [FromBody] Blogs blog, [FromHeader] string UserName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.BlogId)
            {
                return BadRequest();
            }

            Models.Users user = _bUsersRepository.GetAll().Where(P => P.UserName == UserName).FirstOrDefault();
            if (blog.UserId != user.UserId)
                return BadRequest();

            try
            {
                await _blogsRepository.Update(blog);
                return Ok(blog);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BlogExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpPost]
        //[Route("PostBlog")]
        [Produces(typeof(Blogs))]
        public async Task<IActionResult> PostBlog([FromBody] Blogs blog, [FromHeader] string UserName )
        {            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.Users user = _bUsersRepository.GetAll().Where(P => P.UserName == UserName).FirstOrDefault();

            blog.UserId = user.UserId;
            await _blogsRepository.Add(blog);

            var results = new ObjectResult(blog)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            return results;
        }

        [HttpDelete("{id}")]
        [Produces(typeof(Blogs))]
        public async Task<IActionResult> DeleteBlog([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await BlogExists(id))
            {
                return NotFound();
            }

            await _blogsRepository.Remove(id);

            return Ok();
        }
    }
}