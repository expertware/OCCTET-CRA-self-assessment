using System.Security.Claims;

namespace exp.backend.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserID(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;
        }
        public static string GetOrganisationID(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.NameIdentifier) ?? String.Empty;
        }
        public static Claim GetUser(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier)!;
        }

        public static string GetUserTenant(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue("tenant") ?? String.Empty;
        }

        public static string GetRole(this ClaimsPrincipal principal)
        {
            return principal?.FindFirstValue(ClaimTypes.Role) ?? String.Empty;
        }

        public static List<string> GetRoles(this ClaimsPrincipal principal)
        {
            return principal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}
