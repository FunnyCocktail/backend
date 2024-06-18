namespace Application.DTOS
{
    public class CreateCocktailDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Base64Image { get; set; }

        public Guid AuthorId { get; set; }

        public List<Guid> Ingredients { get; set; } = null!;
    }
}
