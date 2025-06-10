using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Collections.Generic;

namespace BuildingBlocks.Authorization.Helpers
{
    public static class JwtHelper
    {
        public static (string UserId, string Username, List<string> Roles, List<string> Workgroups) ParseClaims(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var userId = token.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            var username = token.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            var roles = token.Claims.Where(x => x.Type == "roles").Select(x => x.Value).ToList();
            var workgroups = token.Claims.Where(x => x.Type == "workgroups").Select(x => x.Value).ToList();

            return (userId, username, roles, workgroups);
        }
    }
}