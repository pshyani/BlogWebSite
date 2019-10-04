using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlogWebSiteAPI.Contracts;
using BlogWebSiteAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogWebSiteAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/BlogComments")]
    public class BlogCommentsController : Controller
    {
        // GET: api/<controller>
        private readonly IBlogCommentsRepository _blogCommentsRepository;
        private readonly IUsersRepository _bUsersRepository;
        public BlogCommentsController(IBlogCommentsRepository blogCommentsRepository, IUsersRepository bUsersRepository)
        {
            _blogCommentsRepository = blogCommentsRepository;
            _bUsersRepository = bUsersRepository;
        }

        private async Task<bool> BlogCommentExists(int id)
        {
            return await _blogCommentsRepository.Exist(id);
        }

        [HttpGet]
        [Route("GetAll/{blogid:int}")]
        [Produces(typeof(DbSet<BlogComments>))]
        public IActionResult GetAll([FromRoute] int blogId)
        {
            var results = new ObjectResult(_blogCommentsRepository.GetAll(blogId))
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            Request.HttpContext.Response.Headers.Add("X-Total-Count", _blogCommentsRepository.GetAll(blogId).Count().ToString());

            return results;
        }

        [HttpGet]
        [Route("GetBlogComment/{id:int}")]
        [Produces(typeof(BlogComments))]
        public async Task<IActionResult> GetBlogComments([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var blogComment = await _blogCommentsRepository.Find(id);

            if (blogComment == null)
            {
                return NotFound();
            }

            //var results = new ObjectResult(blogComment)
            //{
            //    StatusCode = (int)HttpStatusCode.OK
            //};

            return Ok(blogComment);
        }

        [HttpPut("{id}")]
        [Produces(typeof(BlogComments))]
        public async Task<IActionResult> PutBlogComment([FromRoute] int id, [FromBody] BlogComments blogComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blogComment.BlogCommentsId)
            {
                return BadRequest();
            }

            try
            {
                await _blogCommentsRepository.Update(blogComment);
                return Ok(blogComment);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BlogCommentExists(id))
                    return NotFound();
                else
                    throw;
            }
        }

        [HttpPost]
        [Produces(typeof(BlogComments))]
        public async Task<IActionResult> PostBlogComment([FromBody] BlogComments blogComment, [FromHeader] string UserName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Models.Users user = _bUsersRepository.GetAll().Where(P => P.UserName == UserName).FirstOrDefault();

            blogComment.UserId = user.UserId;
            await _blogCommentsRepository.Add(blogComment);

            return CreatedAtAction("GetBlogComments", new { id = blogComment.BlogId }, blogComment);
        }

        [HttpDelete("{id}")]
        [Produces(typeof(BlogComments))]
        public async Task<IActionResult> DeleteBlogComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await BlogCommentExists(id))
            {
                return NotFound();
            }

            await _blogCommentsRepository.Remove(id);

            return Ok();
        }
    }
}
