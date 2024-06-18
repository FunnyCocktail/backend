using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class UserVerification : BaseEntity
    {
        public Guid UserId { get; set; }
        public long Code { get; set; }
        public bool Status { get; set; }
        public DateTime DateTimeVerification { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
    }
}
