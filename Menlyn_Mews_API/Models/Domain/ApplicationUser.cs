using Microsoft.AspNetCore.Identity;

namespace Menlyn_Mews_API.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Client? Client { get; set; }
        public virtual Employee? Employee { get; set; } 
    }
}
