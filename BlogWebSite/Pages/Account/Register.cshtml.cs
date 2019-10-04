using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebSite.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        [BindProperty]
        public BlogWebSite.Models.Users user { get; set; }

        public RegisterModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            HttpResponseMessage response = new HttpResponseMessage();
            var client = _clientFactory.CreateClient();
          
            user.PasswordHash = user.PasswordHash;
            response = await client.PostAsJsonAsync("https://localhost:44319/api/Account/RegisterAccount", user);

            if (response.IsSuccessStatusCode)
                return RedirectToPage("Login");
            else 
                return Page();            
        }


        public IActionResult OnPostCancel()
        {
            return RedirectToPage("Index");
        }
    }
}