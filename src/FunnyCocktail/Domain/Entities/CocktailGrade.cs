using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CocktailGrade : BaseEntity
    {
        public Guid CocktailId { get; set; }
        public Guid UserId { get; set; }

        public int Grade { get; set; }

        [JsonIgnore]
        public Cocktail? Cocktail { get; set; }
        [JsonIgnore]
        public User? User { get; set; } 
    }
}
