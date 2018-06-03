using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using MiningMonitor.Model;

namespace MiningMonitor.Service
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<MiningMonitorUser> _userManager;

        public LoginService(SignInManager<MiningMonitorUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(bool success, string token)> LoginUserAsync(string username, string password)
        {
            var result = await _userManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded)
                return (success: false, token: null);

            return (success: true, token: CreateToken(username));
        }

        public string CreateToken(string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Name, username),
            };
            var token = new JwtSecurityToken(claims: claims);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }

        public Task<string> LoginCollectorAsync(string name)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, name),
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, "Collector")
            };
            var token = new JwtSecurityToken(claims: claims);
            var tokenHandler = new JwtSecurityTokenHandler();

            return Task.FromResult(tokenHandler.WriteToken(token));
        }
    }
}