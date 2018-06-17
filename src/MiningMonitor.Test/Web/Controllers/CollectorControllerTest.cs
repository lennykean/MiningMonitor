using System.Linq;
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
    public class CollectorControllerTest
    {
        private Mock<ICollectorService> _collectorService;
        private CollectorController _controller;

        [SetUp]
        public void Setup()
        {
            _collectorService = new Mock<ICollectorService>();
            _controller = new CollectorController(_collectorService.Object);
        }

        [Test]
        public async Task GetAll()
        {
            // Arrange
            var collectors = Enumerable.Range(0, 3).Select(i => new Collector()).ToList();
            _collectorService.Setup(m => m.GetAllAsync())
                .ReturnsAsync(() => collectors)
                .Verifiable();

            // Act
            var result = await _controller.Get();

            // Assert
            _collectorService.Verify();
            Assert.That(result, Is.EquivalentTo(collectors));
        }

        [Test]
        public async Task GetByCollectorId()
        {
            const string collectorId = "12345";

            // Arrange
            var collector = new Collector();
            _collectorService.Setup(m => m.GetAsync(collectorId))
                .ReturnsAsync(() => (true, collector))
                .Verifiable();

            // Act
            var result = await _controller.Get(collectorId);

            // Assert
            _collectorService.Verify();
            Assert.That(result, Has.Property(nameof(ObjectResult.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(ObjectResult.Value)).EqualTo(collector));
        }

        [Test]
        public async Task GetByCollectorIdNotFound()
        {
            const string collectorId = "12345";

            // Arrange
            _collectorService.Setup(m => m.GetAsync(collectorId))
                .ReturnsAsync(() => (false, null))
                .Verifiable();

            // Act
            var result = await _controller.Get(collectorId);

            // Assert
            _collectorService.Verify();
            Assert.That(result, Has.Property(nameof(StatusCodeResult.StatusCode)).EqualTo(404));
        }

        [Test]
        public async Task PostNewCollector()
        {
            // Arrange
            var collector = new Collector();

            var state = new ModelStateDictionary();
            var response = new RegistrationResponse();

            _collectorService.Setup(m => m.CreateCollectorAsync(collector))
                .ReturnsAsync(() => (state, response))
                .Verifiable();

            // Act
            var result = await _controller.Post(collector);

            // Assert
            _collectorService.Verify(m => m.CreateCollectorAsync(collector));
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(response));
        }

        [Test]
        public async Task PostInvalidCollector()
        {
            // Arrange
            var collector = new Collector();

            var state = new ModelStateDictionary();
            state.AddModelError("test-key", "test-validation-message");

            _collectorService.Setup(m => m.CreateCollectorAsync(collector))
                .ReturnsAsync(() => (state, null))
                .Verifiable();

            // Act
            var result = await _controller.Post(collector);

            // Assert
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(400));
            Assert.That(result, Has.Property(nameof(result.Value)).ContainKey("test-key"));
        }

        [TestCase(true, 204)]
        [TestCase(false, 404)]
        public async Task DeleteByCollectorId(bool deleted, int statusCode)
        {
            const string collectorId = "12345";

            // Arrange
            _collectorService.Setup(m => m.DeleteAsync(collectorId))
                .ReturnsAsync(() => deleted)
                .Verifiable();

            // Act
            var result = await _controller.Delete(collectorId);

            // Assert
            _collectorService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(statusCode));
        }
    }
}