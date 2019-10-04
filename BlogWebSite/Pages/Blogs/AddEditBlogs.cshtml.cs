using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlogWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebSite.Pages.Blogs
{
    public class AddEditBlogsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [FromRoute]
        public int? BlogId { get; set; }


        [BindProperty]
        public BlogWebSite.Models.Blogs blog { get; set; }

        public bool IsNewBlog
        {
            get { return BlogId == null; }
        }

        public AddEditBlogsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        
        public async Task OnGetAsync()
        {
            if (!IsNewBlog)
                blog = await GetBlog();              
        }

        private async Task<Models.Blogs> GetBlog()
        {          
            var blogInfo = new Models.Blogs();
            using (var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44319/api/blogs/GetBlog/" + BlogId))
            {
                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    blogInfo = await response.Content.ReadAsAsync<BlogWebSite.Models.Blogs>();
                }
                else
                    blogInfo = new BlogWebSite.Models.Blogs();
            }
            return blogInfo;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }     

            HttpResponseMessage response = new HttpResponseMessage();
            var client = _clientFactory.CreateClient();

            if (IsNewBlog)
            {                
                client.DefaultRequestHeaders.Add("UserName", HttpContext.User.Identity.Name);
                response = await client.PostAsJsonAsync("https://localhost:44319/api/blogs/", blog);
            }
            else
            {
                var blogInfo = await GetBlog();
                blogInfo.Title = blog.Title;
                blogInfo.Description = blog.Description;
                client.DefaultRequestHeaders.Add("UserName", HttpContext.User.Identity.Name);
                response = await client.PutAsJsonAsync("https://localhost:44319/api/blogs/" + BlogId, blogInfo);
            }
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                blog = await response.Content.ReadAsAsync<BlogWebSite.Models.Blogs>();
                BlogId = blog.BlogId;
                return RedirectToPage("ViewBlog", new { BlogId });
            }
            else
                return RedirectToPage("Index");
        }

        public IActionResult OnPostCancel()
        {
            if(BlogId != null)
                return RedirectToPage("ViewBlog", new { BlogId });
            else
                return RedirectToPage("Index");
        }
    }
}