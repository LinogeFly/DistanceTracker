using DistanceTracker.API.Controllers;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Mvc;
using NSubstitute;
using System;
using Xunit;

namespace DistanceTracker.Web.Tests.API
{
    public class UserControllerTests : ControllerTestsBase<UserController, SQLiteDbFixture>, IClassFixture<SQLiteDbFixture>
    {
        public UserControllerTests(SQLiteDbFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Create_creates_user()
        {
            using (var ctrl = GetController())
            {
                var result = ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user1",
                    Password = "Passw0rd1",
                    ConfirmPassword = "Passw0rd1"
                });

                Assert.True(result is HttpOkResult);
                Assert.True(ctrl.UnitOfWork.Users.FindByUserName("user1") != null);
            }
        }

        [Fact]
        public void Create_fails_if_user_already_exists()
        {
            using (var ctrl = GetController())
            {
                ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user2",
                    Password = "Passw0rd2",
                    ConfirmPassword = "Passw0rd2"
                });

                var result = (ObjectResult)ctrl.Create(new UserController.UserRegistration()
                {
                    UserName = "user2",
                    Password = "Passw0rd2",
                    ConfirmPassword = "Passw0rd2"
                });

                Assert.True(result.StatusCode == StatusCodes.Status409Conflict);
            }
        }

        [Fact]
        public void Create_fails_if_passwords_dont_match()
        {
            using (var ctrl = GetController())
            {
                var result = ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user3",
                    Password = "Passw0rd3_1",
                    ConfirmPassword = "Passw0rd3_2"
                });

                Assert.True(result is BadRequestObjectResult);
            }
        }

        [Fact]
        public void Create_generates_different_hashes_for_different_users_with_same_password()
        {
            using (var ctrl = GetController())
            {
                // Act
                ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user4",
                    Password = "Passw0rd",
                    ConfirmPassword = "Passw0rd"
                });
                ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user5",
                    Password = "Passw0rd",
                    ConfirmPassword = "Passw0rd"
                });

                // Assert
                var user1 = ctrl.UnitOfWork.Users.FindByUserName("user4");
                var user2 = ctrl.UnitOfWork.Users.FindByUserName("user5");
                Assert.True(user1.PasswordHash != user2.PasswordHash);
            }
        }

        [Fact]
        public void Create_fails_if_empty_input_data()
        {
            using (var ctrl = GetController())
            {
                var result = ctrl.Create(null);

                Assert.True(result is BadRequestResult);
            }
        }

        [Fact]
        public void LogIn_logs_in_if_valid_credentials()
        {
            var userPrincipal = GetUserPrincipal("user6");
            var httpContext = Substitute.For<HttpContext>();
            httpContext.User.Returns(userPrincipal);

            using (var ctrl = GetController(httpContext))
            {
                ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user6",
                    Password = "Passw0rd6",
                    ConfirmPassword = "Passw0rd6"
                });

                var result = ctrl.LogIn(new UserController.UserCredentials
                {
                    UserName = "user6",
                    Password = "Passw0rd6",
                    RememberMe = true
                });

                Assert.True(result is HttpOkResult);
                httpContext.Authentication.ReceivedWithAnyArgs().SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true
                    });
            }
        }

        [Fact]
        public void LogIn_fails_if_invalid_username()
        {
            using (var ctrl = GetController())
            {
                var result = ctrl.LogIn(new UserController.UserCredentials
                {
                    UserName = "non_existent_username",
                    Password = "Passw0rd"
                });

                Assert.True(result is BadRequestObjectResult);
            }
        }

        [Fact]
        public void LogIn_fails_if_invalid_password()
        {
            using (var ctrl = GetController())
            {
                ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user7",
                    Password = "Passw0rd7",
                    ConfirmPassword = "Passw0rd7"
                });

                var result = ctrl.LogIn(new UserController.UserCredentials
                {
                    UserName = "user7",
                    Password = "not_Passw0rd7"
                });

                Assert.True(result is BadRequestObjectResult);
            }
        }

        [Fact]
        public void LogIn_fails_if_empty_input_data()
        {
            using (var ctrl = GetController())
            {
                var result = ctrl.LogIn(null);

                Assert.True(result is BadRequestResult);
            }
        }

        [Fact]
        public void Get_returns_userinfo_for_logged_in_user()
        {
            var httpContext = Substitute.For<HttpContext>();
            httpContext.User.Returns(GetUserPrincipal("user8"));

            using (var ctrl = GetController(httpContext))
            {
                ctrl.Create(new UserController.UserRegistration
                {
                    UserName = "user8",
                    Password = "Passw0rd8",
                    ConfirmPassword = "Passw0rd8"
                });

                var result = (HttpOkObjectResult)ctrl.Get();
                var resultObj = (UserController.UserInfo)result.Value;

                Assert.True(result is HttpOkObjectResult);
                Assert.True(resultObj.UserName == "user8");
            }
        }

        [Fact]
        public void LogOut_logs_out_logged_in_user()
        {
            var httpContext = Substitute.For<HttpContext>();
            httpContext.User.Returns(GetUserPrincipal("user9"));

            using (var ctrl = GetController(httpContext))
            {
                var result = ctrl.LogOut();

                Assert.True(result is HttpOkResult);
                httpContext.Authentication.ReceivedWithAnyArgs().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
