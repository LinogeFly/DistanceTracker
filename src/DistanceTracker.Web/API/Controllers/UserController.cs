using System;
using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Authorization;
using System.Security.Claims;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Http;
using System.Linq;
using DistanceTracker.Data.UnitOfWork;
using DistanceTracker.Data.Model;
using System.Security.Principal;

namespace DistanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : Controller, IApiController
    {
        [FromServices]
        public IHostingEnvironment HostingEnvironment { get; set; }

        [FromServices]
        public IUnitOfWork UnitOfWork { get; set; }

        public UserController()
        {
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public IActionResult Create([FromBody]UserRegistration user)
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            if (user == null)
                return HttpBadRequest();

            // User already exists
            if (UnitOfWork.Users.FindByUserName(user.UserName) != null)
                return new ObjectResult("User already exists.") { StatusCode = StatusCodes.Status409Conflict };

            // Password doesn't matche ConfirmPassword
            if (user.Password != user.ConfirmPassword)
                return HttpBadRequest("Password and confirm password don't match.");

            // Create user
            var salt = CreatePasswordSalt();
            UnitOfWork.Users.Add(new User()
            {
                UserName = user.UserName,
                PasswordSalt = salt,
                PasswordHash = HashPassword(user.Password, salt)
            });

            UnitOfWork.Complete();

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            return Ok(new UserInfo()
            {
                UserName = User.Identity.Name
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public IActionResult LogIn([FromBody]UserCredentials userCreds)
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            if (userCreds == null)
                return HttpBadRequest();

            // Check username
            var user = UnitOfWork.Users.FindByUserName(userCreds.UserName);
            if (user == null)
                return HttpBadRequest("Invalid username or password.");

            // Check password
            if (user.PasswordHash != HashPassword(userCreds.Password, user.PasswordSalt))
                return HttpBadRequest("Invalid username or password.");

            // Sign-in
            HttpContext.Authentication.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new GenericIdentity(user.UserName, CookieAuthenticationDefaults.AuthenticationScheme))),
                new AuthenticationProperties
                {
                    IsPersistent = userCreds.RememberMe
                }
            );

            return Ok();
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult LogOut()
        {
            if (HostingEnvironment.IsDevelopment())
                System.Threading.Thread.Sleep(1000);

            HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }

        private string HashPassword(string password, string salt)
        {
            using (SHA256 sha = SHA256.Create())
            {
                var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
                return Convert.ToBase64String(hash);
            }
        }

        private string CreatePasswordSalt()
        {
            // Generate a cryptographic random number
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[16];
            rng.GetBytes(buff);

            // Return a Base64 string representation of the random number.
            return Convert.ToBase64String(buff);
        }

        #region Internal types

        public class UserInfo
        {
            public string UserName { get; set; }
        }

        public class UserCredentials
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        public class UserRegistration
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        #endregion
    }
}
