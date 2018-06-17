using System.Collections.Generic;

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
        public void GetAll()
        {
            // Arrange
            var settings = new Dictionary<string, string> { ["test1"] = "test1", ["test2"] = "test2" };

            _service.Setup(m => m.GetAll()).Returns(() => settings).Verifiable();

            var controller = new ServerSettingsController(_service.Object);

            // Act
            var result = controller.Get();

            // Assert
            _service.Verify();
            Assert.That(result, Is.EquivalentTo(settings));
        }

        [Test]
        public void PutSettings()
        {
            // Arrange
            var setting = new Dictionary<string, string>();
            var controller = new ServerSettingsController(_service.Object);

            _service.Setup(m => m.UpdateSettings(setting)).Returns(() => (true, setting)).Verifiable();

            // Act
            var result = controller.Put(setting);

            // Assert
            _service.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(setting));
        }

        [Test]
        public void PutSettingNotFound()
        {
            // Arrange
            var setting = new Dictionary<string, string>();
            var controller = new ServerSettingsController(_service.Object);

            _service.Setup(m => m.UpdateSettings(setting)).Returns(() => (false, null)).Verifiable();

            // Act
            var result = controller.Put(setting);

            // Assert
            _service.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(404));
        }
    }
}