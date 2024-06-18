namespace Application.Models.Email
{
    public class EmailTemplate
    {
        public string Email { get; set; } = null!;
        public string Subject { get; set;} = null!;
        public EmailBodyTemplate Body { get; set; } = null!;
    }
}
