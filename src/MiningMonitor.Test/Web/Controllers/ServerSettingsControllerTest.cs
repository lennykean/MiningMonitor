using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        [Test]
        public async Task GetAll()
        {
            // Arrange
            var settings = new Dictionary<string, string> { ["test1"] = "test1", ["test2"] = "test2" };

            _service.Setup(m => m.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(() => settings)
                .Verifiable();

            var controller = new ServerSettingsController(_service.Object);

            // Act
            var result = await controller.GetAsync();

            // Assert
            _service.Verify();
            Assert.That(result, Is.EquivalentTo(settings));
        }

        [Test]
        public async Task PutSettings()
        {
            // Arrange
            var setting = new Dictionary<string, string>();
            var controller = new ServerSettingsController(_service.Object);

            _service.Setup(m => m.UpdateSettingsAsync(setting, CancellationToken.None))
                .ReturnsAsync(() => (new ModelStateDictionary(), setting))
                .Verifiable();

            // Act
            var result = await controller.Put(settings: setting);

            // Assert
            _service.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(setting));
        }

        [Test]
        public async Task PutSettingNotFound()
        {
            // Arrange
            var setting = new Dictionary<string, string>();
            var controller = new ServerSettingsController(_service.Object);
            var modelState = new ModelStateDictionary();

            modelState.AddModelError("", "");
            _service.Setup(m => m.UpdateSettingsAsync(setting, CancellationToken.None))
                .ReturnsAsync(() => (modelState, null))
                .Verifiable();

            // Act
            var result = await controller.Put(settings: setting);

            // Assert
            _service.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(400));
        }
    }
}