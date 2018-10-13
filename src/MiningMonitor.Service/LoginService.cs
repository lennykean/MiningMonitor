using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using MiningMonitor.Security.Identity;

namespace MiningMonitor.Service
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<MiningMonitorUser> _signinManager;

        public LoginService(SignInManager<MiningMonitorUser> signinManager)
        {
            _signinManager = signinManager;
        }

        public async Task<(bool success, string token)> LoginUserAsync(string username, string password)
        {
            var result = await _signinManager.PasswordSignInAsync(username, password, false, false);
            
            if (!result.Succeeded)
                return (success: false, token: null);
            
            return (success: true, token: CreateToken(username));
        }

        public string LoginCollector(string collectorId)
        {
            return CreateToken(collectorId, new Claim(ClaimTypes.Role, "Collector"));
        }

        private static string CreateToken(string username, params Claim[] additionalClaims)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Name, username),
            }.Concat(additionalClaims);

            var token = new JwtSecurityToken(claims: claims);
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}