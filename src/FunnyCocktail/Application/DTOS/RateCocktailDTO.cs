namespace Application.DTOS
{
    public class RateCocktailDTO
    {
        public Guid CocktailId { get; set; }
        public Guid UserId { get; set; }

        public int Grade { get; set; }
    }
}
