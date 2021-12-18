using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using ProcessManagement.Core;
using ProcessManagement.Core.Entities;
using ProcessManagement.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManagement.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
        }
        public User GetLoggedInUser()
        {
            var userNameIdentifier = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var specification = new Specification<User>
            {
                Criteria = (u) => u.Email == userNameIdentifier,
            };
            var user = _unitOfWork.UserRepository.Get(specification).FirstOrDefault();

            return user;
        }

        public Task SignInAsync(string email, string password)
        {
            var user = _unitOfWork.UserRepository.Get(new Specification<User>
            {
                Criteria = (u) => u.Email == email,
            }).FirstOrDefault();

            if (user == null)
            {
                throw new Exception();
            }

            if (HashPassword(password, Convert.FromBase64String(user.Salt)) != user.Password)
            {
                throw new Exception();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return _httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
        }

        public Task SignOutAsync()
        {
            return _httpContextAccessor.HttpContext.SignOutAsync();
        }

        // SignUp:
        //var salt = CreatePasswordSalt();
        //_unitOfWork.UserRepository.Add(new User
        //{
        //    Email = authLogin.Email,
        //    Name = "Test1",
        //    Password = HashPassword(authLogin.Password, salt),
        //    Salt = Convert.ToBase64String(salt),
        //});
        //await _unitOfWork.CompleteAsync();

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
