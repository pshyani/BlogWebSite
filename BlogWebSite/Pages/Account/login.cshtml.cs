using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlogWebSite.Pages.Account
{  

    public class loginModel : PageModel
    {
        [BindProperty]
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        private readonly IHttpClientFactory _clientFactory;
        public loginModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public void OnGet()
        {           
        }        
        public async Task<IActionResult> OnPost()
        {       
            HttpResponseMessage response = new HttpResponseMessage();
            var client = _clientFactory.CreateClient();

            var user = new BlogWebSite.Models.Users();
            user.UserName = UserName;
            user.PasswordHash = Password;

            response = await client.PostAsJsonAsync("https://localhost:44319/api/account/login", user);

            if (response.IsSuccessStatusCode)
            {
                var dbuser = await response.Content.ReadAsAsync<BlogWebSite.Models.Users>();

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Email, dbuser.Email));
                claims.Add(new Claim(ClaimTypes.Name, dbuser.UserName));

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal).Wait();
                return Redirect("/Blogs/Index");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password!");
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnGetLogout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Index");
        }

   
        public IActionResult OnGetRegister()
        {
            return RedirectToPage("Register");
        }
    }
}