using System.Threading.Tasks;

using ClaymoreMiner.RemoteManagement;
using ClaymoreMiner.RemoteManagement.Models;

using Microsoft.Extensions.Logging;

using MiningMonitor.BackgroundWorker.DataCollector;
using MiningMonitor.Model;
using MiningMonitor.Service;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.BackgroundWorker.DataCollector
{
    [TestFixture]
    public class SnapshotDataCollectorTest
    {
        private Mock<ILogger<SnapshotDataCollector>> _logger;
        private Mock<IMinerService> _minerService;
        private Mock<ISnapshotService> _snapshotService;
        private Mock<IRemoteManagementClientFactory> _clientFactory;
        private Mock<IRemoteManagementClient> _client;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<SnapshotDataCollector>>();
            _minerService = new Mock<IMinerService>();
            _snapshotService = new Mock<ISnapshotService>();
            _clientFactory = new Mock<IRemoteManagementClientFactory>();
            _client = new Mock<IRemoteManagementClient>();

            _clientFactory.Setup(m => m.Create(It.IsAny<Miner>())).Returns(() => _client.Object);
        }

        [Test]
        public async Task Collect()
        {
            // Arrange
            var statisitcs = new MinerStatistics();
            var miners = new[] {new Miner()};

            _minerService.Setup(m => m.GetEnabledMinersAsync()).ReturnsAsync(() => miners);
            _client.Setup(m => m.GetStatisticsAsync()).ReturnsAsync(() => statisitcs);

            var dataCollector = new SnapshotDataCollector(
                _minerService.Object,
                _snapshotService.Object,
                _clientFactory.Object,
                _logger.Object);

            // Act
            await dataCollector.Collect();

            // Assert
            _snapshotService.Verify(m => m.AddAsync(It.Is<Snapshot>(s => s.MinerStatistics == statisitcs)));
        }
    }
}