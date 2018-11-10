using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using LiteDB;

using MiningMonitor.Data;
using MiningMonitor.Data.LiteDb;
using MiningMonitor.Model;
using MiningMonitor.Model.Identity;
using MiningMonitor.Security.Identity;
using MiningMonitor.Service;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Service
{
    [TestFixture]
    public class SettingsServiceTest
    {
        private MemoryStream _ms;
        private LiteDatabase _memoryDb;
        private LiteCollection<Setting> _collection;
        private Mock<IRepository<MiningMonitorUser, Guid>> _userRepo;
        private SettingsService _subject;

        [SetUp]
        public void Setup()
        {
            _ms = new MemoryStream();
            _memoryDb = new LiteDatabase(_ms);
            _collection = _memoryDb.GetCollection<Setting>();
            _userRepo = new Mock<IRepository<MiningMonitorUser, Guid>>();
            _subject = new SettingsService(new LiteDbRepository<Setting, string>(_collection), _userRepo.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _ms.Dispose();
        }

        [Test]
        public async Task GetAllDefaults()
        {
            // Act
            var result = (await _subject.GetAllAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(SettingsService.DefaultSettings.Count));
        }

        [Test]
        public async Task GetAll()
        {
            // Arrange
            _collection.Insert(new Setting {Key = "EnableSecurity", Value = "true"});

            // Act
            var result = (await _subject.GetAllAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(SettingsService.DefaultSettings.Count));
            Assert.That(result.FirstOrDefault(s => s.Key == "EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo("true"));
        }

        [Test]
        public async Task GetByName()
        {
            // Arrange
            _collection.Insert(new Setting {Key = "EnableSecurity", Value = "test"});

            // Act
            var (success, setting) = await _subject.GetSettingAsync("EnableSecurity");

            // Assert
            Assert.That(success, Is.True);
            Assert.That(setting, Is.EqualTo("test"));
        }

        [Test]
        public async Task GetByNameNotFound()
        {
            // Act
            var (success, _) = await _subject.GetSettingAsync("fake");

            // Assert
            Assert.That(success, Is.False);
        }

        [Test]
        public async Task GetByNameWithDefault()
        {
            // Act
            var (success, setting) = await _subject.GetSettingAsync("EnableSecurity");

            // Assert
            Assert.That(success, Is.True);
            Assert.That(setting, Is.Not.Null);
        }

        [Test]
        public async Task UpdateExistingSetting()
        {
            // Arrange
            const string firstValue = "true";
            const string newValue = "false";
            _collection.Insert(new Setting { Key = "EnableSecurity", Value = firstValue });

            // Act
            var (modelState, _) = await _subject.UpdateSettingsAsync(new Dictionary<string, string> { ["EnableSecurity"] = newValue });

            // Assert
            Assert.That(_collection.FindById("EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo(newValue));
            Assert.That(modelState.IsValid, Is.True);
        }

        [Test]
        public async Task UpdateDefaultSetting()
        {
            // Arrange
            const string value = "true";

            // Act
            var (modelState, _) = await _subject.UpdateSettingsAsync(new Dictionary<string, string> { ["IsDataCollector"] = value });

            // Assert
            Assert.That(_collection.FindById("IsDataCollector"), Has.Property(nameof(Setting.Value)).EqualTo(value));
            Assert.That(modelState.IsValid, Is.True);
        }

        [Test]
        public async Task CannotEnableSecurityWithNoUsers()
        {
            // Arrange
            _userRepo.Setup(m => m.FindAllAsync(CancellationToken.None))
                .ReturnsAsync(Enumerable.Empty<MiningMonitorUser>);

            // Act
            var modelState = await _subject.UpdateSettingAsync("EnableSecurity", "true");

            // Assert
            Assert.That(modelState.IsValid, Is.False);
            Assert.That(_collection.FindById("EnableSecurity"), Is.Null);
        }

        [Test]
        public async Task UpdateSettingNotFound()
        {
            // Act
            var (modelState, _) = await _subject.UpdateSettingsAsync(new Dictionary<string, string> { ["fake"] = "notreal" });

            // Assert
            Assert.That(modelState.IsValid, Is.False);
        }
    }
}