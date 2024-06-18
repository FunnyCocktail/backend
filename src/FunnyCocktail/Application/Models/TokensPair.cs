namespace Application.Models
{
    public class TokensPair
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;

        public DateTime AccessExp { get; set; }
        public DateTime RefreshExp { get; set; }
    }
}
