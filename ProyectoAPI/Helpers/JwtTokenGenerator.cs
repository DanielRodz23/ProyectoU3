using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProyectoAPI.Helpers
{
    public static class JwtTokenGenerator
    {
        public static string GetToken(string token)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sid, ""));
            return "";
        }
    }
}
