using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class UserRequest
    {
        [MaxLength(100)]
        public string Email { get; set; } = null!;
        [MaxLength(20)]
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
