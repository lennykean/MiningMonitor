using System;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Web.Controllers;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class MinersControllerTest
    {
        private Mock<IMinerService> _minerService;
        private Mock<ICollectorService> _collectorService;

        [SetUp]
        public void Setup()
        {
            _minerService = new Mock<IMinerService>();
            _collectorService = new Mock<ICollectorService>();
        }

        [TestCase(TestName = "MinersController.Get{a}")]
        public async Task MinersControllerGetAll()
        {
            // Arrange
            var miners = Enumerable.Range(0, 3).Select(i => new Miner()).ToList();

            _minerService.Setup(m => m.GetAllAsync()).ReturnsAsync(() => miners).Verifiable();

            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = await controller.Get();

            // Assert
            _minerService.Verify();
            Assert.That(result, Is.EquivalentTo(miners));
        }

        [TestCase(TestName = "MinersController.Get(int) returns miner")]
        public async Task MinersControllerGetOk()
        {
            // Arrange
            var miner = new Miner();

            _minerService.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => miner).Verifiable();

            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = await controller.Get(Guid.NewGuid());

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(miner));
        }

        [TestCase(TestName = "MinersController.Get(Guid) returns 404")]
        public async Task MinersControllerGetNotFound()
        {
            // Arrange
            _minerService.Setup(m => m.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(() => null).Verifiable();

            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = await controller.Get(Guid.NewGuid());

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(404));
        }

        [TestCase(TestName = "MinersController.Post(Miner) adds miner")]
        public async Task MinersControllerPostAdds()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = await controller.Post(miner);

            // Assert
            _minerService.Verify(m => m.AddAsync(It.IsAny<Miner>()));
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(miner));
        }

        [TestCase(TestName = "MinersController.Post(Miner) returns validatation message")]
        public async Task MinersControllerPostValidates()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            controller.ModelState.AddModelError("test-key", "test-validation-message");

            // Act
            var result = await controller.Post(miner);

            // Assert
            _minerService.Verify(m => m.AddAsync(It.IsAny<Miner>()), Times.Never);
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(400));
            Assert.That(result, Has.Property(nameof(result.Value)).ContainKey("test-key"));
        }

        [TestCase(TestName = "MinersController.Put(Miner) updates miner")]
        public async Task MinersControllerPutUpdates()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            _minerService.Setup(m => m.UpdateAsync(miner)).ReturnsAsync(() => true).Verifiable();

            // Act
            var result = await controller.Put(miner);

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(miner));
        }

        [TestCase(TestName = "MinersController.Put(Miner) returns 404")]
        public async Task MinersControllerPutNotFound()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            _minerService.Setup(m => m.UpdateAsync(miner)).ReturnsAsync(() => false).Verifiable();

            // Act
            var result = await controller.Put(miner);

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(404));
        }

        [TestCase(TestName = "MinersController.Put(Miner) returns validatation message")]
        public async Task MinersControllerPutValidates()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            controller.ModelState.AddModelError("test-key", "test-valation-message");

            // Act
            var result = await controller.Put(miner);

            // Assert
            _minerService.Verify(m => m.UpdateAsync(It.IsAny<Miner>()), Times.Never);
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(400));
            Assert.That(result, Has.Property(nameof(result.Value)).ContainKey("test-key"));
        }

        [TestCase(true, 204, TestName = "MinersController.Delete(int) deletes miner")]
        [TestCase(false, 404, TestName = "MinersController.Delete(int) returns 404")]
        public async Task MinersControllerDelete(bool deleted, int statusCode)
        {
            // Arrange
            var minerId = Guid.NewGuid();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            _minerService.Setup(m => m.DeleteAsync(minerId)).ReturnsAsync(() => deleted).Verifiable();

            // Act
            var result = await controller.Delete(minerId);

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(statusCode));
        }
    }
}