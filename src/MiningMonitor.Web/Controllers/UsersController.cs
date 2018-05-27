using System.Collections.Generic;
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
        public async Task<IEnumerable<User>> Get()
        {
            return await _userService.GetUsersAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            var result = await _userService.CreateUserAsync(user);

            if (!result.IsValid)
                return BadRequest(result);

            return NoContent();
        }
    }
}