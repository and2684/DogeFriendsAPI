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

        public static async Task<List<UserRole>?> GetLoggedUserRoles(this ClaimsPrincipal user, DataContext context)
        {
            var users = await context.Users.Include(r => r.Roles).Where(x => x.Username == GetLoggedUsername(user)).ToListAsync();
            var roles = users?.FirstOrDefault()?.Roles?.ToList();
            return roles;
        }

    }
}