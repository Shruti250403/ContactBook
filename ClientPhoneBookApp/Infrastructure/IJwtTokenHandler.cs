using System.IdentityModel.Tokens.Jwt;

namespace ClientPhoneBookApp.Infrastructure
{
    public interface IJwtTokenHandler
    {
        JwtSecurityToken ReadJwtToken(string token);
    }
}
