using System.Threading;
using System.Threading.Tasks;

using ClaymoreMiner.RemoteManagement;
using ClaymoreMiner.RemoteManagement.Models;

using Microsoft.Extensions.Logging;

using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Workers.DataCollector;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Workers
{
    [TestFixture]
    public class DataCollectorWorkerTest
    {
        private Mock<ILogger<DataCollectorWorker>> _logger;
        private Mock<IMinerService> _minerService;
        private Mock<ISnapshotService> _snapshotService;
        private Mock<IRemoteManagementClientFactory> _clientFactory;
        private Mock<IRemoteManagementClient> _client;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<DataCollectorWorker>>();
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
            var statistics = new MinerStatistics();
            var miners = new[] { new Miner() };

            _minerService.Setup(m => m.GetEnabledMinersAsync(CancellationToken.None)).ReturnsAsync(() => miners);
            _client.Setup(m => m.GetStatisticsAsync()).ReturnsAsync(() => statistics);

            var dataCollector = new DataCollectorWorker(
                _minerService.Object,
                _snapshotService.Object,
                _clientFactory.Object,
                _logger.Object);

            // Act
            await dataCollector.DoWorkAsync(CancellationToken.None);

            // Assert
            _snapshotService.Verify(m => m.AddAsync(It.Is<Snapshot>(s => s.MinerStatistics == statistics), CancellationToken.None));
        }
    }
}