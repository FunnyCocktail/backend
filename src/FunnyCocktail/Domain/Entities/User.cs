using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        [JsonIgnore]
        public string PasswordHash { get; set; } = null!;

        public UserAdditionalInfo? UserAdditionalInfo { get; set; }
    }
}
