using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using ClaymoreMiner.RemoteManagement.Models;

using LiteDB;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MiningMonitor.Common;
using MiningMonitor.Common.Mapper;
using MiningMonitor.Data.LiteDb;
using MiningMonitor.Model;
using MiningMonitor.Security.Identity;
using MiningMonitor.Service;
using MiningMonitor.Service.Mapper;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Service
{
    [TestFixture]
    public class CollectorServiceTest
    {
        private MemoryStream _ms;
        private LiteDatabase _memoryDb;
        private LiteCollection<MiningMonitorUser> _userCollection;
        private Mock<ILogger<MiningMonitorUserStore>> _userStoreLogger;
        private MiningMonitorUserStore _userStore;
        private Mock<IOptions<IdentityOptions>> _optionsAccessor;
        private Mock<IPasswordHasher<MiningMonitorUser>> _passwordHasher;
        private Mock<ILookupNormalizer> _normalizer;
        private Mock<IServiceProvider> _serviceProvider;
        private Mock<ILogger<UserManager<MiningMonitorUser>>> _userManagerLogger;
        private UserManager<MiningMonitorUser> _userManager;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private Mock<ILogger<SignInManager<MiningMonitorUser>>> _signinManagerLogger;
        private Mock<IAuthenticationSchemeProvider> _authSchemeProvider;
        private Mock<IUserClaimsPrincipalFactory<MiningMonitorUser>> _userFactory;
        private SignInManager<MiningMonitorUser> _signinManager;
        private LoginService _loginService;
        private LiteCollection<Snapshot> _snapshotCollection;
        private SnapshotService _snapshotService;
        private LiteCollection<Miner> _minerCollection;
        private MinerService _minerService;
        private UserMapper _userMapper;
        private IdentityResultMapper _resultMapper;
        private CollectorService _subject;

        [SetUp]
        public void Setup()
        {
            _ms = new MemoryStream();
            _memoryDb = new LiteDatabase(_ms);
            _userCollection = _memoryDb.GetCollection<MiningMonitorUser>();
            _userStoreLogger = new Mock<ILogger<MiningMonitorUserStore>>();
            _userStore = new MiningMonitorUserStore(new LiteDbRepository<MiningMonitorUser>(_userCollection), _userStoreLogger.Object);
            _optionsAccessor = new Mock<IOptions<IdentityOptions>>();
            _passwordHasher = new Mock<IPasswordHasher<MiningMonitorUser>>();
            _normalizer = new Mock<ILookupNormalizer>();
            _serviceProvider = new Mock<IServiceProvider>();
            _userManagerLogger = new Mock<ILogger<UserManager<MiningMonitorUser>>>();
            _userManager = new UserManager<MiningMonitorUser>(_userStore, 
                _optionsAccessor.Object, 
                _passwordHasher.Object, 
                Enumerable.Empty<IUserValidator<MiningMonitorUser>>(), 
                Enumerable.Empty<IPasswordValidator<MiningMonitorUser>>(), 
                _normalizer.Object, 
                new IdentityErrorDescriber(), 
                _serviceProvider.Object,
                _userManagerLogger.Object);
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _signinManagerLogger = new Mock<ILogger<SignInManager<MiningMonitorUser>>>();
            _userFactory = new Mock<IUserClaimsPrincipalFactory<MiningMonitorUser>>();
            _authSchemeProvider = new Mock<IAuthenticationSchemeProvider>();
            _signinManager = new SignInManager<MiningMonitorUser>(_userManager,
                _httpContextAccessor.Object,
                _userFactory.Object,
                _optionsAccessor.Object,
                _signinManagerLogger.Object,
                _authSchemeProvider.Object);
            _loginService = new LoginService(_signinManager);
            _snapshotCollection = _memoryDb.GetCollection<Snapshot>();
            _snapshotService = new SnapshotService(new LiteDbRepository<Snapshot>(_snapshotCollection));
            _minerCollection = _memoryDb.GetCollection<Miner>();
            _minerService = new MinerService(new LiteDbRepository<Miner>(_minerCollection), _snapshotService);
            _userMapper = new UserMapper();
            _resultMapper = new IdentityResultMapper();
            _subject = new CollectorService(_userManager, 
                _loginService, 
                _minerService, 
                _snapshotService, 
                _userMapper, 
                _userMapper, 
                _resultMapper);
        }

        [TearDown]
        public void TearDown()
        {
            _ms.Dispose();
        }

        [Test]
        public async Task MinerSyncNewMiner()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var miner = new Miner {Id = Guid.NewGuid()};

            // Act
            var success = await _subject.MinerSyncAsync(collectorId, miner);

            // Assert
            var dbMiner = _minerCollection.FindById(miner.Id);
            Assert.That(success, Is.True);
            Assert.That(dbMiner, Is.Not.Null);
            Assert.That(dbMiner, Has.Property(nameof(Miner.CollectorId)).EqualTo(collectorId));
        }

        [Test]
        public async Task MinerSyncExistingMiner()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var existingMiner = new Miner {Id = Guid.NewGuid(), CollectorId = collectorId, DisplayName = "existing"};
            var updatedMiner = new Miner {Id = existingMiner.Id, DisplayName = "updated"};

            _minerCollection.Insert(existingMiner);

            // Act
            var success = await _subject.MinerSyncAsync(collectorId, updatedMiner);

            // Assert
            var dbMiner = _minerCollection.FindById(existingMiner.Id);
            Assert.That(success, Is.True);
            Assert.That(dbMiner, Has.Property(nameof(Miner.CollectorId)).EqualTo(collectorId));
            Assert.That(dbMiner, Has.Property(nameof(Miner.DisplayName)).EqualTo(updatedMiner.DisplayName));
        }

        [Test]
        public async Task MinerSyncWithoutCollectorId()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var existingMiner = new Miner { Id = Guid.NewGuid(), DisplayName = "existing" };
            var updatedMiner = new Miner { Id = existingMiner.Id, DisplayName = "updated" };

            _minerCollection.Insert(existingMiner);

            // Act
            var success = await _subject.MinerSyncAsync(collectorId, updatedMiner);

            // Assert
            var dbMiner = _minerCollection.FindById(existingMiner.Id);
            Assert.That(success, Is.False);
            Assert.That(dbMiner, Has.Property(nameof(Miner.DisplayName)).EqualTo(existingMiner.DisplayName));
        }

        [Test]
        public async Task MinerSyncCollectorIdMismatch()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var existingMiner = new Miner { Id = Guid.NewGuid(), CollectorId = Guid.NewGuid().ToString(), DisplayName = "existing" };
            var updatedMiner = new Miner { Id = existingMiner.Id, DisplayName = "updated" };

            _minerCollection.Insert(existingMiner);

            // Act
            var success = await _subject.MinerSyncAsync(collectorId, updatedMiner);

            // Assert
            var dbMiner = _minerCollection.FindById(existingMiner.Id);
            Assert.That(success, Is.False);
            Assert.That(dbMiner, Has.Property(nameof(Miner.DisplayName)).EqualTo(existingMiner.DisplayName));
        }

        [Test]
        public async Task SnapshotSyncNewSnapshot()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var minerId = Guid.NewGuid();
            var snapshot = new Snapshot { Id = Guid.NewGuid() };

            _minerCollection.Insert(new Miner {Id = minerId, CollectorId = collectorId});

            // Act
            var success = await _subject.SnapshotSyncAsync(collectorId, minerId, snapshot);

            // Assert
            var dbSnapshot = _snapshotCollection.FindById(snapshot.Id);
            Assert.That(success, Is.True);
            Assert.That(dbSnapshot, Is.Not.Null);
            Assert.That(dbSnapshot, Has.Property(nameof(Snapshot.MinerId)).EqualTo(minerId));
        }

        [Test]
        public async Task SnapshotSyncExistingSnapshot()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var minerId = Guid.NewGuid();
            var existingSnapshot = new Snapshot { Id = Guid.NewGuid(), SnapshotTime = DateTime.MinValue };
            var updatedSnapshot = new Snapshot { Id = existingSnapshot.Id, SnapshotTime = DateTime.MinValue.AddMinutes(1) };

            _minerCollection.Insert(new Miner { Id = minerId, CollectorId = collectorId });
            _snapshotCollection.Insert(existingSnapshot);

            // Act
            var success = await _subject.SnapshotSyncAsync(collectorId, minerId, updatedSnapshot);

            // Assert
            var dbSnapshot = _snapshotCollection.FindById(existingSnapshot.Id);
            Assert.That(success, Is.True);
            Assert.That(dbSnapshot, Has.Property(nameof(Snapshot.MinerId)).EqualTo(minerId));
            Assert.That(dbSnapshot, Has.Property(nameof(Snapshot.SnapshotTime)).EqualTo(updatedSnapshot.SnapshotTime));
        }

        [Test]
        public async Task SnapshotSyncWithoutMiner()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var minerId = Guid.NewGuid();
            var snapshot = new Snapshot { Id = Guid.NewGuid() };
            
            // Act
            var success = await _subject.SnapshotSyncAsync(collectorId, minerId, snapshot);

            // Assert
            var dbSnapshot = _snapshotCollection.FindById(snapshot.Id);
            Assert.That(success, Is.False);
            Assert.That(dbSnapshot, Is.Null);
        }

        [Test]
        public async Task SnapshotSyncMinerCollectorIdMismatch()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var minerId = Guid.NewGuid();
            var snapshot = new Snapshot { Id = Guid.NewGuid() };

            _minerCollection.Insert(new Miner { Id = minerId, CollectorId = Guid.NewGuid().ToString() });

            // Act
            var success = await _subject.SnapshotSyncAsync(collectorId, minerId, snapshot);

            // Assert
            var dbSnapshot = _snapshotCollection.FindById(snapshot.Id);
            Assert.That(success, Is.False);
            Assert.That(dbSnapshot, Is.Null);
        }

        [Test]
        public async Task SnapshotSyncMinerWithoutCollectorId()
        {
            // Arrange
            var collectorId = Guid.NewGuid().ToString();
            var minerId = Guid.NewGuid();
            var snapshot = new Snapshot { Id = Guid.NewGuid() };

            _minerCollection.Insert(new Miner { Id = minerId });

            // Act
            var success = await _subject.SnapshotSyncAsync(collectorId, minerId, snapshot);

            // Assert
            var dbSnapshot = _snapshotCollection.FindById(snapshot.Id);
            Assert.That(success, Is.False);
            Assert.That(dbSnapshot, Is.Null);
        }
    }
}
