using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTrackerEtf.Controllers;
using TimeTrackerEtf.Data;
using TimeTrackerEtf.Models;
using TimeTrackerEtf.Tests.UnitTests;
using Xunit;

namespace TimeTrackerEtf.Tests.UnitTests
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        public UsersControllerTests()
        {
            var options = new DbContextOptionsBuilder<TimeTrackerDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var dbContext = new TimeTrackerDbContext(options);

            dbContext.Users.Add(new Domain.User { Id = 1, Name = "User 1", HourRate = 15 });
            dbContext.Users.Add(new Domain.User { Id = 2, Name = "User 2", HourRate = 25 });
            dbContext.Users.Add(new Domain.User { Id = 3, Name = "User 3", HourRate = 35 });
            dbContext.SaveChanges();

            var logger = new FakeLogger<UsersController>();

            _controller = new UsersController(dbContext, logger);
        }
    
        [Fact(Skip = "Doesn't work with EF Core 3 Preview 6")]
        public void GetById_IdDoesNotExist_ReturnsNotFound()
        {
            //Act
            var result = _controller.GetById(0);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);


        }

        [Fact]
        public async void GetById_IdExists_ReturnsCorrectResult()
        {
            const string expectedName = "User 1";

            var result = await _controller.GetById(1);

            Assert.IsType<ActionResult<UserModel>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedName, result.Value.Name);

        }

        [Fact]
        public async Task GetPage_FirstPage_ReturnsExpectedResult()
        {
            const int expectedCount = 3;
            const int expectedTotalCount = 3;

            var result = await _controller.GetPage(1, 10);

            Assert.IsType<ActionResult<PagedList<UserModel>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(expectedCount, result.Value.Items.Count());
            Assert.Equal(expectedTotalCount, result.Value.TotalCount);
        }


        [Fact]
        public async Task GetPage_SecondPage_ReturnsExpectedResult()
        {
            const int expectedTotalCount = 3;

            var result = await _controller.GetPage(2, 10);

            Assert.IsType<ActionResult<PagedList<UserModel>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.Items);
            Assert.Equal(expectedTotalCount, result.Value.TotalCount);
        }

        [Fact]
        public async Task Delete_IdExist_ReturnsOkResult()
        {
            var result = await _controller.Delete(1);

            Assert.IsType<OkResult>(result);
        }
    }
}
