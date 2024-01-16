using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystemAPI.Models.Authentication.Login
{
    public class LoginUser
    {
        [Required (ErrorMessage = "Username is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
