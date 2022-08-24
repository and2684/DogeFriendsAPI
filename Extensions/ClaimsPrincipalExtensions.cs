using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetLoggedUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value!;
        }

        public static int GetLoggedUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }    
    }
}