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
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public IEnumerable<BlogWebSite.Models.Blogs> Blogs { get; private set; }

        //public IEnumerable<GitHubBranch> Branches { get; private set; }
        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGet()
        {
            await GetBlogs("");
        }

        public async Task<ActionResult> OnGetMyBlogs()
        {
            if (HttpContext.User.Identity.Name == null)
                return Redirect("/Account/Login");
            
            await GetBlogs(HttpContext.User.Identity.Name);
            return Page();
        }

        public async Task GetBlogs(string strUserName)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:44319/api/blogs/GetBlogs/" + strUserName);
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Blogs = await response.Content.ReadAsAsync<IEnumerable<BlogWebSite.Models.Blogs>>();
            }
            else
                Blogs = Array.Empty<BlogWebSite.Models.Blogs>();
        }
    }
}