using Microsoft.AspNetCore.Identity;

namespace exp.Backend.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
