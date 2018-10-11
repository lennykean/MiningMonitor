using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAsync(CancellationToken token = default)
        {
            return await _userService.GetUsersAsync(token);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]User user, CancellationToken token = default)
        {
            var result = await _userService.CreateUserAsync(user, token);

            if (!result.IsValid)
                return BadRequest(result);

            return NoContent();
        }
    }
}