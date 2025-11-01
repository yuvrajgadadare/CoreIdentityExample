using Microsoft.AspNetCore.Identity;

namespace ERP_Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
