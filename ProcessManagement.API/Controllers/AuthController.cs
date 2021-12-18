using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProcessManagement.API.ApiModels;
using ProcessManagement.Core;
using ProcessManagement.Core.Entities;
using ProcessManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] AuthLogin authLogin)
        {
            var user = _unitOfWork.UserRepository.Get(new Specification<User>
            {
                Criteria = (u) => u.Email == authLogin.Email,
            }).FirstOrDefault();

            if (user == null)
            {
                //var salt = CreatePasswordSalt();
                //_unitOfWork.UserRepository.Add(new User
                //{
                //    Email = authLogin.Email,
                //    Name = "Test1",
                //    Password = HashPassword(authLogin.Password, salt),
                //    Salt = Convert.ToBase64String(salt),
                //});
                //await _unitOfWork.CompleteAsync();

                return Unauthorized();
            }

            if (HashPassword(authLogin.Password, Convert.FromBase64String(user.Salt)) != user.Password)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,
                    user.Email
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

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] CreateUser createUser)
        {
            _unitOfWork.UserRepository.Add(createUser.ToUser());

            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        private string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

            return hashed;
        }

        private byte[] CreatePasswordSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            return salt;
        }
    }
}
