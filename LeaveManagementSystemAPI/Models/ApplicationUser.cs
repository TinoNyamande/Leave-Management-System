using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystemAPI.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string?Firstname { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
