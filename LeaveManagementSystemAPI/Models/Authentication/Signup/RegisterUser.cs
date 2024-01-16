using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystemAPI.Models.Authentication.Signup
{
    public class RegisterUser
    {
        
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }


    }
}
