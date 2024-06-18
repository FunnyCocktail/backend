using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CocktailIngredient : BaseEntity
    {
        public Guid CocktailId { get; set; }
        public Guid IngredientId { get; set; }

        [JsonIgnore]
        public Cocktail? Cocktail { get; set; }
        [JsonIgnore]
        public Ingredient? Ingredient { get; set; }
    }
}
