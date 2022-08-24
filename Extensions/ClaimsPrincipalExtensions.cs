using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetLoggedUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(JwtRegisteredClaimNames.Name)?.Value!;
        }

        public static int GetLoggedUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }         

        public static string GetLoggedUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value!;
        }

    }
}