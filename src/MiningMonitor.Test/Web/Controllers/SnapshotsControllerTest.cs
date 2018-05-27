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

        [SetUp]
        public void Setup()
        {
            _snapshotService = new Mock<ISnapshotService>();
        }

        [TestCase("56f5fb3a-4b59-417c-aae0-ace175bb7c5b", TestName = "SnapshotsController.Get{a}")]
        public async Task SnapshotsControllerGet(string minerIdString)
        {
            // Arrange
            var minerId = new Guid(minerIdString);
            var schedule = new SnapshotDataCollectorSchedule {Interval = TimeSpan.FromMinutes(1)};
            var snapshots = Enumerable.Range(0, 3).Select(i => new Snapshot {SnapshotTime = DateTime.Now}).ToList();
            _snapshotService.Setup(m => m.GetByMinerAsync(minerId, It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<TimeSpan>()))
                .ReturnsAsync(() => snapshots)
                .Verifiable();
            var controller = new SnapshotsController(_snapshotService.Object, schedule);

            // Act
            var result = await controller.Get(minerId);

            // Assert
            _snapshotService.Verify();
            Assert.That(result, Is.Not.Empty);
        }
    }
}