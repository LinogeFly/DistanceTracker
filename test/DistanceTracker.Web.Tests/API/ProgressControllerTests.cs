using DistanceTracker.API.Controllers;
using DistanceTracker.Data.Model;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DistanceTracker.Web.Tests.API
{
    public class ProgressControllerTests : ControllerTestsBase<ProgressController, SQLiteDbFixture>, IClassFixture<SQLiteDbFixture>
    {
        public ProgressControllerTests(SQLiteDbFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Get_returns_progress()
        {
            using (var ctrl = GetController(GetTestUser("user1", 100)))
            {
                var result = ctrl.Get();

                Assert.True(result is HttpOkObjectResult);
                Assert.True(int.Parse(((HttpOkObjectResult)result).Value.ToString()) == 100);
            }
        }

        [Fact]
        public void Post_updates_progress()
        {
            using (var ctrl = GetController(GetTestUser("user2", 100)))
            {
                var result = ctrl.Post(100);

                Assert.True(result is HttpOkResult);
                Assert.True(ctrl.UnitOfWork.Users.FindByUserName("user2").Progress == 200);
            }
        }

        [Fact]
        public void Post_creates_history_item()
        {
            using (var ctrl = GetController(GetTestUser("user3", 100)))
            {
                var result = ctrl.Post(100);

                var user = ctrl.UnitOfWork.Users.FindByUserName("user3");
                var historyItems = ctrl.UnitOfWork.History
                    .Find(x => x.UserId == user.Id)
                    .ToArray();

                Assert.True(result is HttpOkResult);
                Assert.True(historyItems.Count() == 1);
            }
        }

        [Fact]
        public void Delete_resets_progress()
        {
            using (var ctrl = GetController(GetTestUser("user4", 100)))
            {
                var result = ctrl.Delete();

                Assert.True(result is HttpOkResult);
                Assert.True(ctrl.UnitOfWork.Users.FindByUserName("user4").Progress == 0);
            }
        }

        [Fact]
        public void Delete_removes_history_items()
        {
            using (var ctrl = GetController(GetTestUser("user5", 100)))
            {
                var result = ctrl.Delete();

                var user = ctrl.UnitOfWork.Users.FindByUserName("user5");
                var historyItems = ctrl.UnitOfWork.History
                    .Find(x => x.UserId == user.Id)
                    .ToArray();

                Assert.True(result is HttpOkResult);
                Assert.True(historyItems.Count() == 0);
            }
        }
    }
}
