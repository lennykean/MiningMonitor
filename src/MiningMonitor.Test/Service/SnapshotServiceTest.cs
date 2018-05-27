using System;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.Data.Repository;
using MiningMonitor.Model;
using MiningMonitor.Service;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class SnapshotsServiceTest
    {
        private Mock<ISnapshotRepository> _snapshotRepository;
        private Mock<IMinerService> _minerService;

        [SetUp]
        public void Setup()
        {
            _snapshotRepository = new Mock<ISnapshotRepository>();
            _minerService = new Mock<IMinerService>();
        }

        [TestCase("56f5fb3a-4b59-417c-aae0-ace175bb7c5b", TestName = "SnapshotsService.GetByMinerAsync{a}")]
        public async Task SnapshotServiceGetByMinerAsync(string minerIdString)
        {
            // Arrange
            var minerId = new Guid(minerIdString);
            var snapshots = Enumerable.Range(0, 3)
                .Select(i => new Snapshot {SnapshotTime = DateTime.Now})
                .ToList();
            _snapshotRepository.Setup(m => m.GetByMinerAsync(minerId, It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(() => snapshots)
                .Verifiable();

            var service = new SnapshotService(_snapshotRepository.Object, _minerService.Object);

            // Act
            var result = await service.GetByMinerAsync(minerId, null, null, TimeSpan.FromMinutes(1));

            // Assert
            _snapshotRepository.Verify();
            Assert.That(result, Is.Not.Empty);
        }

        [TestCase("56f5fb3a-4b59-417c-aae0-ace175bb7c5b", TestName = "SnapshotsService.GetByMinerAsync{a} fills gaps")]
        public async Task SnapshotsControllerGetFillsGaps(string minerIdString)
        {
            // Arrange
            var minerId = new Guid(minerIdString);
            var from = new DateTime(2018, 1, 1, 12, 0, 0);
            var to = new DateTime(2018, 1, 1, 12, 10, 0);
            var actualStart = from.AddMinutes(3);
            var snapshots = Enumerable.Range(0, 3)
                .Select(i => new Snapshot {SnapshotTime = actualStart.AddMinutes(i * 2)})
                .ToList();
            _snapshotRepository.Setup(m => m.GetByMinerAsync(minerId, It.IsAny<DateTime?>(), It.IsAny<DateTime?>()))
                .ReturnsAsync(() => snapshots)
                .Verifiable();

            var service = new SnapshotService(_snapshotRepository.Object, _minerService.Object);

            // Act
            var result = await service.GetByMinerAsync(minerId, from, to, TimeSpan.FromMinutes(1));

            // Assert
            _snapshotRepository.Verify();
            Assert.That(result, Is.SupersetOf(snapshots));
            Assert.That(result, Has.Count.EqualTo(9));
        }
    }
}