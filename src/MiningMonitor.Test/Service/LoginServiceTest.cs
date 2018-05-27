using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using AspNetCore.Identity.LiteDB.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using MiningMonitor.Model;
using MiningMonitor.Service;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Service
{
    [TestFixture]
    public class LoginServiceTest
    {
        private Mock<IUserPasswordStore<MiningMonitorUser>> _userStore;
        private Mock<IPasswordHasher<MiningMonitorUser>> _passwordHasher;
        private Mock<ILogger<UserManager<MiningMonitorUser>>> _userManagerLogger;
        private Mock<IHttpContextAccessor> _contextAccessor;
        private Mock<HttpContext> _context;
        private Mock<IServiceProvider> _services;
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IUserClaimsPrincipalFactory<MiningMonitorUser>> _claimsFactory;
        private Mock<ILogger<SignInManager<MiningMonitorUser>>> _signinManagerLogger;

        private UserManager<MiningMonitorUser> _userManager;
        private SignInManager<MiningMonitorUser> _signinManager;

        [SetUp]
        public void Setup()
        {
            _userStore = new Mock<IUserPasswordStore<MiningMonitorUser>>();
            _passwordHasher = new Mock<IPasswordHasher<MiningMonitorUser>>();
            _userManagerLogger = new Mock<ILogger<UserManager<MiningMonitorUser>>>();
            _contextAccessor = new Mock<IHttpContextAccessor>();
            _context = new Mock<HttpContext>();
            _services = new Mock<IServiceProvider>();
            _authenticationService = new Mock<IAuthenticationService>();
            _claimsFactory = new Mock<IUserClaimsPrincipalFactory<MiningMonitorUser>>();
            _signinManagerLogger = new Mock<ILogger<SignInManager<MiningMonitorUser>>>();

            _contextAccessor.SetupGet(m => m.HttpContext).Returns(() => _context.Object);
            _context.Setup(m => m.RequestServices).Returns(() => _services.Object);
            _services.Setup(m => m.GetService(typeof(IAuthenticationService)))
                .Returns(() => _authenticationService.Object);

            _userManager = new UserManager<MiningMonitorUser>(_userStore.Object, null, _passwordHasher.Object, null, null,
                null, null, null, _userManagerLogger.Object);
            _signinManager = new SignInManager<MiningMonitorUser>(_userManager, _contextAccessor.Object,
                _claimsFactory.Object, null, _signinManagerLogger.Object, null);
        }

        [TestCase(TestName = "LoginService.LoginAsync()")]
        public async Task AuthenticationServiceLoginAsync()
        {
            // Arrange
            const string username = "test-user";
            const string password = "hunter2";
            const string passwordHash = "passwordHash";

            var applicationUser = new MiningMonitorUser();
            var principal = new ClaimsPrincipal();
            var tokenHander = new JwtSecurityTokenHandler();
            var authenticationService = new LoginService(_signinManager);

            _userStore.Setup(m => m.FindByNameAsync(username, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => applicationUser);
            _userStore.Setup(m => m.GetPasswordHashAsync(applicationUser, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => passwordHash);
            _passwordHasher.Setup(m => m.VerifyHashedPassword(applicationUser, passwordHash, password))
                .Returns(() => PasswordVerificationResult.Success);
            _claimsFactory.Setup(m => m.CreateAsync(applicationUser)).ReturnsAsync(() => principal);

            // Act
            var result = await authenticationService.LoginUserAsync(username, password);

            // Assert
            Assert.That(result.success, Is.True);
            Assert.That(tokenHander.ReadJwtToken(result.token),
                Has.Property(nameof(JwtSecurityToken.Subject)).EqualTo(username));
            _authenticationService.Verify(m =>
                m.SignInAsync(_context.Object, It.IsAny<string>(), principal, It.IsAny<AuthenticationProperties>()));
        }

        [TestCase(TestName = "LoginService.LoginAsync() fails")]
        public async Task AuthenticationServiceLoginAsyncFails()
        {
            // Arrange
            const string username = "test-user";
            const string password = "hunter2";

            var authenticationService = new LoginService(_signinManager);

            _userStore.Setup(m => m.FindByNameAsync(username, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            // Act
            var result = await authenticationService.LoginUserAsync(username, password);

            // Assert
            Assert.That(result.success, Is.False);
        }
    }
}