using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class UserAdditionalInfo : BaseEntity
    {
        [JsonIgnore]
        public Guid RoleId { get; set; }
        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public bool IsVerify { get; set; } = false;

        public string? ImageUri { get; set; }
        [JsonIgnore]
        public string SecretKey { get; set; } = null!;

        [JsonIgnore]
        public User? User { get; set; }
        [JsonIgnore]
        public Role? Role { get; set; }
    }
}
