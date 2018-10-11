using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly ISettingsService _settingsService;

        public LoginController(ILoginService loginService, ISettingsService settingsService)
        {
            _loginService = loginService;
            _settingsService = settingsService;
        }

        [HttpGet("required")]
        public async Task<bool> GetRequiredAsync(CancellationToken token = default)
        {
            var result = await _settingsService.GetSettingAsync("EnableSecurity", token);

            if (!result.success || !bool.TryParse(result.setting, out var enableSecurity) || !enableSecurity)
                return false;

            return !User.Identity.IsAuthenticated;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]LoginCredentials credentials, CancellationToken token = default)
        {
            var result = await _loginService.LoginUserAsync(credentials.Username, credentials.Password);

            if (result.success)
                return Json(result.token);

            return Unauthorized();
        }
    }
}