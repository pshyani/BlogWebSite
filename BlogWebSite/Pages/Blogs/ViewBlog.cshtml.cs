using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebSite.Pages.Blogs
{
    public class ViewBlogModel : PageModel
    {
        [FromRoute]
        public int? BlogId { get; set; }

        [BindProperty]
        public BlogWebSite.Models.Blogs blog { get; set; }

        [BindProperty]
        public string Comment { get; set; }

        public IEnumerable<BlogWebSite.Models.BlogComments> blogComments { get; private set; }

        private readonly IHttpClientFactory _clientFactory;
        public ViewBlogModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGet()
        {
            await GetBlog();
            await GetBlogComments();
        }

        private async Task GetBlogComments()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44319/api/blogComments/GetAll/" + BlogId))
            {
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    blogComments = await response.Content.ReadAsAsync<IEnumerable<BlogWebSite.Models.BlogComments>>();
                }
                else
                    blogComments = Array.Empty<BlogWebSite.Models.BlogComments>();
            }
        }

        private async Task GetBlog()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44319/api/blogs/GetBlog/" + BlogId))
            {
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    blog = await response.Content.ReadAsAsync<BlogWebSite.Models.Blogs>();                   
                    if (blog.User.UserName == HttpContext.User.Identity.Name)
                        ViewData["bIsAuthor"] = true;
                    else
                        ViewData["bIsAuthor"] = false;
                }
                else
                    blog = new BlogWebSite.Models.Blogs();
            }
        }

        public async Task<IActionResult> OnPostDelete()
        {   
            HttpResponseMessage response = new HttpResponseMessage();
            var client = _clientFactory.CreateClient();     
            response = await client.DeleteAsync("https://localhost:44319/api/blogs/" + BlogId);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
                return RedirectToPage("Index");

        }

        public async Task<IActionResult> OnPostSaveComment()
        { 
            if(HttpContext.User.Identity.Name == null)
                return Redirect("/Account/Login");

            HttpResponseMessage response = new HttpResponseMessage();
            var client = _clientFactory.CreateClient();

            var blogComment = new BlogWebSite.Models.BlogComments();
            blogComment.BlogId = BlogId.GetValueOrDefault(-1);
            blogComment.Comment = Comment;
            blogComment.UserId = 1;

            client.DefaultRequestHeaders.Add("UserName", HttpContext.User.Identity.Name);
            response = await client.PostAsJsonAsync("https://localhost:44319/api/blogComments/", blogComment);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("ViewBlog", new { id = BlogId });
            }
            else
                return RedirectToPage("ViewBlog", new { id = BlogId });
        }


        public IActionResult OnPostCancel()
        {
            return RedirectToPage("Index");
        }

        public IActionResult OnPostEdit()
        {
            return RedirectToPage("AddEditBlogs", new { id = BlogId });
        }

        public async Task<IActionResult> OnPostDeleteComment(int BlogCommentsId)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var client = _clientFactory.CreateClient();

            response = await client.DeleteAsync("https://localhost:44319/api/blogComments/" + BlogCommentsId);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("ViewBlog", new { id = BlogId });
            }
            else
                return RedirectToPage("Index", new { id = BlogId });
        }
    }
}