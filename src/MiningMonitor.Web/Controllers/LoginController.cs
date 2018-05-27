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

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]LoginCredentials credentials)
        {
            var result = await _loginService.LoginUserAsync(credentials.Username, credentials.Password);

            if (result.success)
                return Json(result.token);

            return Unauthorized();
        }
    }
}