using System;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.BackgroundWorker.DataCollector;
using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Web.Controllers;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class SnapshotsControllerTest
    {
        private Mock<ISnapshotService> _snapshotService;
        private Mock<ICollectorService> _collectorService;

        [SetUp]
        public void Setup()
        {
            _snapshotService = new Mock<ISnapshotService>();
            _collectorService = new Mock<ICollectorService>();
        }

        [Test]
        public async Task GetByMinerId()
        {
            // Arrange
            var minerId = new Guid("56f5fb3a-4b59-417c-aae0-ace175bb7c5b");
            var schedule = new SnapshotDataCollectorSchedule {Interval = TimeSpan.FromMinutes(1)};
            var snapshots = Enumerable.Range(0, 3).Select(i => new Snapshot {SnapshotTime = DateTime.Now}).ToList();
            _snapshotService.Setup(m => m.GetByMinerAsync(minerId, It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(() => snapshots)
                .Verifiable();
            var controller = new SnapshotsController(_snapshotService.Object, _collectorService.Object, schedule);

            // Act
            var result = await controller.Get(minerId);

            // Assert
            _snapshotService.Verify();
            Assert.That(result, Is.Not.Empty);
        }

        [TestCase(true, 200)]
        [TestCase(false, 400)]
        public async Task PostCollectorSync(bool success, int expectedStatus)
        {
            // Arrange
            var collector = "12345";
            var minerId = new Guid("56f5fb3a-4b59-417c-aae0-ace175bb7c5b");
            var schedule = new SnapshotDataCollectorSchedule { Interval = TimeSpan.FromMinutes(1) };
            var snapshot = new Snapshot();
            _collectorService.Setup(m => m.SnapshotSyncAsync(collector, minerId, snapshot))
                .ReturnsAsync(() => success)
                .Verifiable();
            var controller = new SnapshotsController(_snapshotService.Object, _collectorService.Object, schedule);

            // Act
            var result = await controller.Post(collector, minerId, snapshot);

            // Assert
            _snapshotService.Verify();
            Assert.That(result, Has.Property(nameof(result.StatusCode)).EqualTo(expectedStatus));
        }
    }
}