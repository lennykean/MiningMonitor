using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Web.Controllers;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class UsersControllerTest
    {
        private Mock<IUserService> _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new Mock<IUserService>();
        }

        [Test]
        public async Task Get()
        {
            // Arrange
            var users = Enumerable.Range(0, 3).Select(i => new UserListItem()).ToList();

            _userService.Setup(m => m.GetUsersAsync(null, CancellationToken.None))
                .ReturnsAsync(() => users)
                .Verifiable();

            var controller = new UsersController(_userService.Object);

            // Act
            var result = await controller.GetAsync(null);

            // Assert
            _userService.Verify();
            Assert.That(result, Is.EquivalentTo(users));
        }

        [Test]
        public async Task PostNewUser()
        {
            // Arrange
            var user = new User();

            _userService.Setup(m => m.CreateUserAsync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(() => new ModelStateDictionary())
                .Verifiable();

            var controller = new UsersController(_userService.Object);

            // Act
            var result = await controller.PostAsync(user);

            // Assert
            _userService.Verify();
            Assert.That(result, Has.Property(nameof(StatusCodeResult.StatusCode)).EqualTo(204));
        }

        [Test]
        public async Task PostInvalidUser()
        {
            // Arrange
            var user = new User();
            var state = new ModelStateDictionary();
            state.AddModelError("err", "error1");
            state.AddModelError("err", "error2");
            _userService.Setup(m => m.CreateUserAsync(It.IsAny<User>(), CancellationToken.None))
                .ReturnsAsync(() => state)
                .Verifiable();

            var controller = new UsersController(_userService.Object);

            // Act
            var result = await controller.PostAsync(user);

            // Assert
            _userService.Verify();
            Assert.That(result, Has.Property(nameof(ObjectResult.StatusCode)).EqualTo(400));
            Assert.That(result, Has.Property(nameof(ObjectResult.Value)).ContainKey("err"));
        }
    }
}