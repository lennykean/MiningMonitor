using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MiningMonitor.Data.Repository;
using MiningMonitor.Model;
using MiningMonitor.Service;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Service
{
    [TestFixture]
    public class SettingsServiceTest
    {
        private Mock<ISettingRepository> _repository;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<ISettingRepository>();
        }

        [TestCase(TestName = "SettingsService.GetAllAsync() returns defaults")]
        public async Task SettingsServiceGetAllAsyncDefaults()
        {
            // Arrange
            _repository.Setup(m => m.GetAllAsync()).ReturnsAsync(() => new Setting[0]).Verifiable();

            var service = new SettingsService(_repository.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(SettingsService.DefaultSettings.Count));
        }

        [TestCase(TestName = "SettingsService.GetAllAsync() merges settings")]
        public async Task SettingsServiceGetAllAsyncMerges()
        {
            // Arrange
            var setting = new Setting {Key = "EnableSecurity", Value = "true"};
            _repository.Setup(m => m.GetAllAsync()).ReturnsAsync(() => new[] {setting}).Verifiable();

            var service = new SettingsService(_repository.Object);

            // Act
            var result = (await service.GetAllAsync()).ToList();

            // Assert
            Assert.That(result, Has.Count.EqualTo(SettingsService.DefaultSettings.Count));
            Assert.That(result.FirstOrDefault(s => s.Key == "EnableSecurity"),
                Has.Property(nameof(Setting.Value)).EqualTo("true"));
        }

        [TestCase(TestName = "SettingsService.GetSettingAsync(string)")]
        public async Task SettingsServiceGetSettingAsync()
        {
            // Arrange
            var service = new SettingsService(_repository.Object);

            _repository.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => new Setting {Key = "EnableSecurity", Value = "test"}).Verifiable();

            // Act
            var (success, setting) = await service.GetSettingAsync("EnableSecurity");

            // Assert
            Assert.That(success, Is.True);
            Assert.That(setting, Is.EqualTo("test"));
        }

        [TestCase(TestName = "SettingsService.GetSettingAsync(string) fails on non-existant")]
        public async Task SettingsServiceGetSettingAsyncFails()
        {
            // Arrange
            var setting = new Setting();
            var service = new SettingsService(_repository.Object);

            _repository.Setup(m => m.GetSettingAsync("fake")).ReturnsAsync(() => setting).Verifiable();

            // Act
            var (success, _) = await service.GetSettingAsync("fake");

            // Assert
            Assert.That(success, Is.False);
        }

        [TestCase(TestName = "SettingsService.GetSettingAsync(string) returns default")]
        public async Task SettingsServiceGetSettingAsyncDefault()
        {
            // Arrange
            var service = new SettingsService(_repository.Object);

            // Act
            var (success, setting) = await service.GetSettingAsync("EnableSecurity");

            // Assert
            Assert.That(success, Is.True);
            Assert.That(setting, Is.Not.Null);
        }

        [TestCase(TestName = "SettingsService.UpdateSettingAsync() inserts")]
        public async Task SettingsServiceUpdateSettingAsyncInsert()
        {
            // Arrange
            var settings = new Dictionary<string, string> {["EnableSecurity"] = "true"};
            var service = new SettingsService(_repository.Object);

            // Act
            var (success, _) = await service.UpdateSettingsAsync(settings);

            // Assert
            _repository.Verify(m => m.AddAsync(It.IsAny<Setting>()));
            Assert.That(success, Is.True);
        }

        [TestCase(TestName = "SettingsService.UpdateSettingAsync() updates")]
        public async Task SettingsServiceUpdateSettingAsyncUpdates()
        {
            // Arrange
            var originalSetting = new Setting {Key = "EnableSecurity", Value = "true"};
            var settings = new Dictionary<string, string> {["EnableSecurity"] = "true"};
            var service = new SettingsService(_repository.Object);

            _repository.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => originalSetting)
                .Verifiable();
            _repository.Setup(m => m.UpdateAsync(It.Is<Setting>(s => s.Key == "EnableSecurity" && s.Value == "true")))
                .ReturnsAsync(() => true)
                .Verifiable();

            // Act
            var (success, _) = await service.UpdateSettingsAsync(settings);

            // Assert
            _repository.Verify();
            Assert.That(success, Is.True);
        }

        [TestCase(TestName = "SettingsService.UpdateSettingAsync() fails on non-existant")]
        public async Task SettingsServiceUpdateSettingAsyncFails()
        {
            // Arrange
            var setting = new Dictionary<string, string> {["fake"] = "notreal"};
            var service = new SettingsService(_repository.Object);

            // Act
            var (success, _) = await service.UpdateSettingsAsync(setting);

            // Assert
            Assert.That(success, Is.False);
        }
    }
}