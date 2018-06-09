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

        [Test]
        public async Task GetAll()
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

        [Test]
        public async Task GetByMinerId()
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

        [Test]
        public async Task GetByMinerIdNotFound()
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

        [Test]
        public async Task PostNewMiner()
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

        [Test]
        public async Task PostInvalidMiner()
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

        [Test]
        public async Task PutUpdatedMiner()
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

        [Test]
        public async Task PutUpdatedMinerNotFound()
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

        [Test]
        public async Task PutInvalidMiner()
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

        //[TestCase(true, 204)]
        //[TestCase(false, 404)]
        public async Task DeleteByMinerId(bool deleted, int statusCode)
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