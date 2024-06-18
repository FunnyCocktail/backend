using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Review : BaseEntity
    {
        public string Value { get; set; } = null!;

        public Guid UserId { get; set; }
        public Guid CocktailId { get; set; }

        [JsonIgnore]
        public Cocktail? Cocktail { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
