using System.Collections.Generic;
using System.IO;
using System.Linq;

using LiteDB;

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
            _subject = new SettingsService(_collection);
        }

        [TearDown]
        public void TearDown()
        {
            _ms.Dispose();
        }

        [Test]
        public void GetAllDefaults()
        {
            // Act
            var result = _subject.GetAll().ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(SettingsService.DefaultSettings.Count));
        }

        [Test]
        public void GetAll()
        {
            // Arrange
            _collection.Insert(new Setting {Key = "EnableSecurity", Value = "true"});

            // Act
            var result = _subject.GetAll().ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(SettingsService.DefaultSettings.Count));
            Assert.That(result.FirstOrDefault(s => s.Key == "EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo("true"));
        }

        [Test]
        public void GetByName()
        {
            // Arrange
            _collection.Insert(new Setting {Key = "EnableSecurity", Value = "test"});

            // Act
            var (success, setting) = _subject.GetSetting("EnableSecurity");

            // Assert
            Assert.That(success, Is.True);
            Assert.That(setting, Is.EqualTo("test"));
        }

        [Test]
        public void GetByNameNotFound()
        {
            // Act
            var (success, _) = _subject.GetSetting("fake");

            // Assert
            Assert.That(success, Is.False);
        }

        [Test]
        public void GetByNameWithDefault()
        {
            // Act
            var (success, setting) = _subject.GetSetting("EnableSecurity");

            // Assert
            Assert.That(success, Is.True);
            Assert.That(setting, Is.Not.Null);
        }

        [Test]
        public void UpdateExistingSetting()
        {
            // Arrange
            const string firstValue = "true";
            const string newValue = "false";
            _collection.Insert(new Setting { Key = "EnableSecurity", Value = firstValue });

            // Act
            var (success, _) = _subject.UpdateSettings(new Dictionary<string, string> { ["EnableSecurity"] = newValue });

            // Assert
            Assert.That(_collection.FindById("EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo(newValue));
            Assert.That(success, Is.True);
        }

        [Test]
        public void UpdateDefaultSetting()
        {
            // Arrange
            const string value = "true";

            // Act
            var (success, _) = _subject.UpdateSettings(new Dictionary<string, string> { ["EnableSecurity"] = value });

            // Assert
            Assert.That(_collection.FindById("EnableSecurity"), Has.Property(nameof(Setting.Value)).EqualTo(value));
            Assert.That(success, Is.True);
        }

        [Test]
        public void UpdateSettingNotFound()
        {
            // Act
            var (success, _) = _subject.UpdateSettings(new Dictionary<string, string> { ["fake"] = "notreal" });

            // Assert
            Assert.That(success, Is.False);
        }
    }
}