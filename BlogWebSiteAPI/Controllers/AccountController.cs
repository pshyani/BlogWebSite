using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BlogWebSiteAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebSiteAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        public AccountController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        [HttpPost]
        [Route("login")]
        [Produces(typeof(Models.Users))]
        public async Task<IActionResult> PostAccount([FromBody] Models.Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dbUser = await _usersRepository.Find(user.UserName, user.PasswordHash);

            if (dbUser != null)
            {
                var results = new ObjectResult(dbUser)
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
                return results;
            }
            else
                return NotFound();
        }


        [HttpPost]
        [Route("RegisterAccount")]
        [Produces(typeof(Models.Users))]
        public async Task<IActionResult> RegisterAccount([FromBody] Models.Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool bDuplicateEmail = _usersRepository.FindDuplicateEmail(user.Email);
            if (bDuplicateEmail)          
                return Conflict("Duplicate Email");          

            bool bDuplicateaUserName = _usersRepository.FindDuplicateEmail(user.UserName);
            if (bDuplicateaUserName)
                return Conflict("Duplicate UserName");

            user = await _usersRepository.Add(user);
            var results = new ObjectResult(user)
            {
                StatusCode = (int)HttpStatusCode.OK
            };
            return results;
         
        }
    }
}