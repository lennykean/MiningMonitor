using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using LiteDB;

using MiningMonitor.Data.LiteDb;
using MiningMonitor.Model;
using MiningMonitor.Service;

using NUnit.Framework;

namespace MiningMonitor.Test.Service
{
    [TestFixture]
    public class SettingsServiceTest
    {
        private MemoryStream _ms;
        private LiteDatabase _memoryDb;
        private LiteCollection<Setting> _collection;
        private SettingsService _subject;

        [SetUp]
        public void Setup()
        {
            _ms = new MemoryStream();
            _memoryDb = new LiteDatabase(_ms);
            _collection = _memoryDb.GetCollection<Setting>();
            _subject = new SettingsService(new LiteDbRepository<Setting>(_collection));
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
            var (success, _) = await _subject.UpdateSettingsAsync(new Dictionary<string, string> { ["EnableSecurity"] = newValue });

            // Assert
            Assert.That(_collection.FindById("EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo(newValue));
            Assert.That(success, Is.True);
        }

        [Test]
        public async Task UpdateDefaultSetting()
        {
            // Arrange
            const string value = "true";

            // Act
            var (success, _) = await _subject.UpdateSettingsAsync(new Dictionary<string, string> { ["EnableSecurity"] = value });

            // Assert
            Assert.That(_collection.FindById("EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo(value));
            Assert.That(success, Is.True);
        }

        [Test]
        public async Task UpdateSettingNotFound()
        {
            // Act
            var (success, _) = await _subject.UpdateSettingsAsync(new Dictionary<string, string> { ["fake"] = "notreal" });

            // Assert
            Assert.That(success, Is.False);
        }
    }
}