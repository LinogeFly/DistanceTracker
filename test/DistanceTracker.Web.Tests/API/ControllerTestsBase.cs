using DistanceTracker.API.Controllers;
using DistanceTracker.Data.Model;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Internal;
using Microsoft.AspNet.Mvc;
using NSubstitute;
using System.Security.Claims;
using System.Security.Principal;

namespace DistanceTracker.Web.Tests.API
{
    public abstract class ControllerTestsBase<TController, TFixture>
        where TController : Controller, IApiController, new()
        where TFixture : IDbFixture
    {
        private readonly IDbFixture Fixture;

        public ControllerTestsBase(IDbFixture fixture)
        {
            Fixture = fixture;
        }

        protected TController GetController()
        {
            return GetController(new DefaultHttpContext());
        }

        protected TController GetController(User user)
        {
            CreateUser(user);

            var httpContext = Substitute.For<HttpContext>();
            httpContext.User.Returns(GetUserPrincipal(user.UserName));

            return GetController(httpContext);
        }

        protected TController GetController(HttpContext httpContext)
        {
            return new TController()
            {
                UnitOfWork = Fixture.GetUnitOfWork(),
                HostingEnvironment = new HostingEnvironment { EnvironmentName = "Test" },
                ActionContext = new ActionContext
                {
                    HttpContext = httpContext
                }
            };
        }

        protected User GetTestUser(string userName)
        {
            return GetTestUser(userName, 0);
        }

        protected User GetTestUser(string userName, int progress)
        {
            return new User
            {
                UserName = userName,
                PasswordHash = "",
                PasswordSalt = "",
                Progress = progress
            };
        }

        protected ClaimsPrincipal GetUserPrincipal(string userName)
        {
            return new ClaimsPrincipal(
                new ClaimsIdentity(
                    new GenericIdentity(userName, CookieAuthenticationDefaults.AuthenticationScheme)
                )
            );
        }

        private void CreateUser(User user)
        {
            var unitOfWork = Fixture.GetUnitOfWork();

            if (unitOfWork.Users.FindByUserName(user.UserName) != null)
                return;

            unitOfWork.Users.Add(user);
            unitOfWork.Complete();
        }
    }
}
