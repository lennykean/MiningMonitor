using System.Collections.Generic;
using System.Threading.Tasks;

using MiningMonitor.Service;
using MiningMonitor.Web.Controllers;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class ServerSettingsControllerTest
    {
        private Mock<ISettingsService> _service;

        [SetUp]
        public void Setup()
        {
            _service = new Mock<ISettingsService>();
        }

        [TestCase(TestName = "ServerSettingsController.Get()")]
        public async Task ServerSettingsControllerGetAll()
        {
            // Arrange
            var settings = new Dictionary<string, string> {["test1"] = "test1", ["test2"] = "test2"};

            _service.Setup(m => m.GetAllAsync()).ReturnsAsync(() => settings).Verifiable();

            var controller = new ServerSettingsController(_service.Object);

            // Act
            var result = await controller.Get();

            // Assert
            _service.Verify();
            Assert.That(result, Is.EquivalentTo(settings));
        }

        [TestCase(TestName = "ServerSettingsController.Put(Setting) updates setting")]
        public async Task ServerSettingsControllerPutUpdates()
        {
            // Arrange
            var setting = new Dictionary<string, string>();
            var controller = new ServerSettingsController(_service.Object);

            _service.Setup(m => m.UpdateSettingsAsync(setting)).ReturnsAsync(() => (true, setting)).Verifiable();

            // Act
            var result = await controller.Put(setting);

            // Assert
            _service.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(setting));
        }

        [TestCase(TestName = "ServerSettingsController.Put(Setting) returns 404")]
        public async Task ServerSettingsControllerPutNotFound()
        {
            // Arrange
            var setting = new Dictionary<string, string>();
            var controller = new ServerSettingsController(_service.Object);

            _service.Setup(m => m.UpdateSettingsAsync(setting)).ReturnsAsync(() => (false, null)).Verifiable();

            // Act
            var result = await controller.Put(setting);

            // Assert
            _service.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(404));
        }
    }
}