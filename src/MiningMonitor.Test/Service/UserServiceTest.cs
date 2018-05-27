using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AspNetCore.Identity.LiteDB.Models;

using Microsoft.AspNetCore.Identity;

using MiningMonitor.Model;
using MiningMonitor.Service;
using MiningMonitor.Service.Mapper;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Service
{
    [TestFixture]
    public class UserServiceTest
    {
        private Mock<IQueryableUserStore<MiningMonitorUser>> _userStore;
        private Mock<IUserPasswordStore<MiningMonitorUser>> _passwordStore;
        private Mock<IPasswordHasher<MiningMonitorUser>> _passwordHasher;
        private UserManager<MiningMonitorUser> _userManager;
        private UserMapper _mapper;
        private IdentityResultMapper _resultMapper;

        [SetUp]
        public void Setup()
        {
            _userStore = new Mock<IQueryableUserStore<MiningMonitorUser>>();
            _passwordStore = _userStore.As<IUserPasswordStore<MiningMonitorUser>>();
            _passwordHasher = new Mock<IPasswordHasher<MiningMonitorUser>>();
            _mapper = new UserMapper();
            _resultMapper = new IdentityResultMapper();
            _userManager =
                new UserManager<MiningMonitorUser>(_userStore.Object, null, _passwordHasher.Object, null, null, null,
                    null, null, null);
        }

        [TestCase(TestName = "UserService.GetAllAsync()")]
        public async Task UserServiceGetAllAsync()
        {
            // Arrange
            var applicationUsers = Enumerable.Range(0, 3).Select(u => new MiningMonitorUser());

            _userStore.SetupGet(m => m.Users).Returns(() => applicationUsers.AsQueryable());

            var userService = new UserService(_userManager, _mapper, _mapper, _resultMapper);

            // Act
            var users = await userService.GetUsersAsync();

            // Assert
            Assert.That(users.Count(), Is.EqualTo(3));
        }

        [TestCase(TestName = "UserService.CreateUser()")]
        public async Task CreateuserAsync()
        {
            // Arrange
            var user = new User {Username = "test-user", Email = "test@test.test", Password = "hunter2"};
            var identityResult = IdentityResult.Success;
            var userService = new UserService(_userManager, _mapper, _mapper, _resultMapper);

            _passwordStore.Setup(m => m.CreateAsync(
                    It.Is<MiningMonitorUser>(u => u.UserName == user.Username && u.Email.Address == user.Email),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => identityResult)
                .Verifiable();

            // Act
            var result = await userService.CreateUserAsync(user);

            // Assert
            _passwordStore.Verify();
            _passwordStore.Verify(m => m.SetPasswordHashAsync(
                It.Is<MiningMonitorUser>(u => u.UserName == user.Username && u.Email.Address == user.Email),
                It.IsAny<string>(), It.IsAny<CancellationToken>()));
            Assert.That(result, Has.Property(nameof(result.IsValid)).EqualTo(true));
        }
    }
}