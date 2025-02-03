using Microsoft.AspNetCore.Identity;

namespace BmisApi.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
