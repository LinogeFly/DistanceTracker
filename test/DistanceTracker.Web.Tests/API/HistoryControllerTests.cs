using DistanceTracker.API.Controllers;
using DistanceTracker.Data.Model;
using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DistanceTracker.Web.Tests.API
{
    public class HistoryControllerTests : ControllerTestsBase<HistoryController, SQLiteDbFixture>, IClassFixture<SQLiteDbFixture>
    {
        public HistoryControllerTests(SQLiteDbFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void Get_returns_history_items()
        {
            var user = GetTestUser("user1");
            using (var ctrl = GetController(user))
            {
                // Arrange 
                ctrl.UnitOfWork.History.Add(new History { UserId = user.Id, Date = DateTime.Now, Distance = 100 });
                ctrl.UnitOfWork.Complete();

                // Act
                var result = ctrl.Get();

                // Assert
                var resultValue = ((HttpOkObjectResult)result).Value as History[];
                Assert.True(result is HttpOkObjectResult);
                Assert.True(resultValue != null);
                Assert.True(resultValue.Count() == 1);
                Assert.True(resultValue[0].Distance == 100);
            }
        }

        [Fact]
        public void Get_returns_latest_15_history_items()
        {
            var user = GetTestUser("user2");
            using (var ctrl = GetController(user))
            {
                // Arrange
                ctrl.UnitOfWork.History.AddRange(new[] {
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 1), Distance = 100 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 2), Distance = 101 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 3), Distance = 102 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 4), Distance = 103 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 5), Distance = 104 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 6), Distance = 105 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 7), Distance = 106 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 8), Distance = 107 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 9), Distance = 108 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 10), Distance = 109 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 11), Distance = 110 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 12), Distance = 111 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 13), Distance = 112 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 14), Distance = 113 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 15), Distance = 114 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 16), Distance = 115 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 17), Distance = 116 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 18), Distance = 117 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 19), Distance = 118 },
                    new History { UserId = user.Id, Date = new DateTime(2000, 1, 20), Distance = 119 }
                });
                ctrl.UnitOfWork.Complete();

                // Act
                var result = ctrl.Get();

                // Assert
                var resultValue = ((HttpOkObjectResult)result).Value as History[];
                Assert.True(result is HttpOkObjectResult);
                Assert.True(resultValue != null);
                Assert.True(resultValue.Count() == 15);
                Assert.True(resultValue[0].Distance == 119);
                Assert.True(resultValue[14].Distance == 105);
            }
        }

        [Fact]
        public void Get_returns_empty_array_if_no_history_yet()
        {
            var user = GetTestUser("user3");
            using (var ctrl = GetController(user))
            {
                // Act
                var result = ctrl.Get();

                // Assert
                var resultValue = ((HttpOkObjectResult)result).Value as History[];
                Assert.True(result is HttpOkObjectResult);
                Assert.True(resultValue != null);
                Assert.True(resultValue.Count() == 0);
            }
        }
    }
}
