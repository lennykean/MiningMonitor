using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Web.Controllers;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class LoginControllerTest
    {
        private Mock<ILoginService> _loginService;
        private Mock<ISettingsService> _settingsService;

        [SetUp]
        public void Setup()
        {
            _loginService = new Mock<ILoginService>();
            _settingsService = new Mock<ISettingsService>();
        }

        [Test]
        public async Task PostPerformsLogin()
        {
            // Arrange
            const string token = "test-token";

            _loginService.Setup(m => m.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => (success: true, token: token))
                .Verifiable();

            var controller = new LoginController(_loginService.Object, _settingsService.Object);

            // Act
            var result = await controller.PostAsync(credentials: new LoginCredentials { Username = "test-user", Password = "hunter2" });

            // Assert
            _loginService.Verify();
            Assert.That(result, Has.Property(nameof(JsonResult.Value)).EqualTo(token));
        }

        [Test]
        public async Task PostFailsAuthentication()
        {
            // Arrange
            _loginService.Setup(m => m.LoginUserAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => (success: false, token: null))
                .Verifiable();

            var controller = new LoginController(_loginService.Object, _settingsService.Object);

            // Act
            var result = await controller.PostAsync(credentials: new LoginCredentials { Username = "test-user", Password = "hunter2" });

            // Assert
            _loginService.Verify();
            Assert.That(result, Has.Property(nameof(StatusCodeResult.StatusCode)).EqualTo(401));
        }
    }
}