using System;
using System.Linq;

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
        public void GetAll()
        {
            // Arrange
            var miners = Enumerable.Range(0, 3).Select(i => new Miner()).ToList();

            _minerService.Setup(m => m.GetAll()).Returns(() => miners).Verifiable();

            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = controller.Get();

            // Assert
            _minerService.Verify();
            Assert.That(result, Is.EquivalentTo(miners));
        }

        [Test]
        public void GetByMinerId()
        {
            // Arrange
            var miner = new Miner();

            _minerService.Setup(m => m.GetById(It.IsAny<Guid>())).Returns(() => miner).Verifiable();

            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = controller.Get(Guid.NewGuid());

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(miner));
        }

        [Test]
        public void GetByMinerIdNotFound()
        {
            // Arrange
            _minerService.Setup(m => m.GetById(It.IsAny<Guid>())).Returns(() => null).Verifiable();

            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = controller.Get(Guid.NewGuid());

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(404));
        }

        [Test]
        public void PostNewMiner()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            // Act
            var result = controller.Post(miner);

            // Assert
            _minerService.Verify(m => m.Add(It.IsAny<Miner>()));
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(miner));
        }

        [Test]
        public void PostInvalidMiner()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            controller.ModelState.AddModelError("test-key", "test-validation-message");

            // Act
            var result = controller.Post(miner);

            // Assert
            _minerService.Verify(m => m.Add(It.IsAny<Miner>()), Times.Never);
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(400));
            Assert.That(result, Has.Property(nameof(result.Value)).ContainKey("test-key"));
        }

        [Test]
        public void PutUpdatedMiner()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            _minerService.Setup(m => m.Update(miner)).Returns(() => true).Verifiable();

            // Act
            var result = controller.Put(miner);

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(200));
            Assert.That(result, Has.Property(nameof(result.Value)).EqualTo(miner));
        }

        [Test]
        public void PutUpdatedMinerNotFound()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            _minerService.Setup(m => m.Update(miner)).Returns(() => false).Verifiable();

            // Act
            var result = controller.Put(miner);

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(404));
        }

        [Test]
        public void PutInvalidMiner()
        {
            // Arrange
            var miner = new Miner();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            controller.ModelState.AddModelError("test-key", "test-valation-message");

            // Act
            var result = controller.Put(miner);

            // Assert
            _minerService.Verify(m => m.Update(It.IsAny<Miner>()), Times.Never);
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(400));
            Assert.That(result, Has.Property(nameof(result.Value)).ContainKey("test-key"));
        }

        [TestCase(true, 204)]
        [TestCase(false, 404)]
        public void DeleteByMinerId(bool deleted, int statusCode)
        {
            // Arrange
            var minerId = Guid.NewGuid();
            var controller = new MinersController(_minerService.Object, _collectorService.Object);

            _minerService.Setup(m => m.Delete(minerId)).Returns(() => deleted).Verifiable();

            // Act
            var result = controller.Delete(minerId);

            // Assert
            _minerService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(statusCode));
        }
    }
}