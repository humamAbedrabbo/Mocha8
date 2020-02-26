using Microsoft.AspNetCore.Identity;

namespace AMS.Models
{
    public class AmsRole : IdentityRole<int>
    {
        public int? TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}
