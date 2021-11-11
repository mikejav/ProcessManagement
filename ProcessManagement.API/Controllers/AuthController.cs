using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProcessManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login()
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    Guid.NewGuid().ToString()
                ),

                new Claim("MY_CUSTOM_CLAIM",
                    Guid.NewGuid().ToString() + Guid.NewGuid().ToString() + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                    + Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()+Guid.NewGuid().ToString()
                )
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }
    }
}
