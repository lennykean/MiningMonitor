using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

using MiningMonitor.Service;
using MiningMonitor.Web.Security;

using Moq;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Security
{
    [TestFixture]
    public class MiningMonitorAuthorizationHandlerTest
    {
        private Mock<ISettingsService> _settingsService;

        [SetUp]
        public void Setup()
        {
            _settingsService = new Mock<ISettingsService>();
        }

        [Test]
        public async Task SecurityDisabled()
        {
            // Arrange
            var requirements = new[] {new AuthenticatedWhenEnabledRequirement()};
            var context = new AuthorizationHandlerContext(requirements, null, null);
            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).True);
        }

        [Test]
        public async Task AuthenticatedWhenEnabledRequirement()
        {
            // Arrange
            var requirements = new[] {new AuthenticatedWhenEnabledRequirement()};
            var principal = new GenericPrincipal(new GenericIdentity("test", "test"), new string[0]);
            var context = new AuthorizationHandlerContext(requirements, principal, null);

            _settingsService.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => (true, "true"));

            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).True);
        }

        [Test]
        public async Task AuthenticatedWhenEnabledRequirementFailsWhenNotAuthenticated()
        {
            // Arrange
            var requirements = new[] {new AuthenticatedWhenEnabledRequirement()};
            var principal = new GenericPrincipal(new GenericIdentity(""), null);
            var context = new AuthorizationHandlerContext(requirements, principal, null);

            _settingsService.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => (true, "true"));

            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).False);
        }

        [Test]
        public async Task HasRoleWhenEnabledRequirement()
        {
            // Arrange
            const string role = "test-role";
            var requirements = new[] {new HasRoleWhenEnabledRequirement(role)};
            var principal = new GenericPrincipal(new GenericIdentity("test", "test"), new[] {role});
            var context = new AuthorizationHandlerContext(requirements, principal, null);

            _settingsService.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => (true, "true"));

            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).True);
        }

        [Test]
        public async Task HasRoleWhenEnabledRequirementFailsWhenNotInRole()
        {
            // Arrange
            const string role = "test-role";
            var requirements = new[] {new HasRoleWhenEnabledRequirement(role)};
            var principal = new GenericPrincipal(new GenericIdentity("test", "test"), new string[0]);
            var context = new AuthorizationHandlerContext(requirements, principal, new ActionDescriptor());

            _settingsService.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => (true, "true"));

            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).False);
        }

        [Test]
        public async Task OwnResourceWhenEnabledRequirement()
        {
            // Arrange
            var requirements = new[] { new OwnResourceWhenEnabledRequirement("id") };
            var principal = new GenericPrincipal(new GenericIdentity("test-id"), null);
            var routeData = new RouteData();
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, routeData, new ActionDescriptor());
            var filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            var context = new AuthorizationHandlerContext(requirements, principal, filterContext);

            routeData.Values.Add("id", "test-id");
            _settingsService.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => (true, "true"));

            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).True);
        }

        [Test]
        public async Task OwnResourceWhenEnabledRequirementFailsWhenResourceIsNotOwned()
        {
            // Arrange
            var requirements = new[] { new OwnResourceWhenEnabledRequirement("id") };
            var principal = new GenericPrincipal(new GenericIdentity("test-id"), null);
            var routeData = new RouteData();
            var httpContext = new Mock<HttpContext>();
            var actionContext = new ActionContext(httpContext.Object, routeData, new ActionDescriptor());
            var filterContext = new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
            var context = new AuthorizationHandlerContext(requirements, principal, filterContext);

            routeData.Values.Add("id", "other-id");
            _settingsService.Setup(m => m.GetSettingAsync("EnableSecurity"))
                .ReturnsAsync(() => (true, "true"));

            var handler = new MiningMonitorAuthorizationHandler(_settingsService.Object);

            // Act
            await handler.HandleAsync(context);

            // Assert
            Assert.That(context, Has.Property(nameof(context.HasSucceeded)).False);
        }
    }
}