using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_CoreAPI.DTOs
{
    public class LoginRequestDTO
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
