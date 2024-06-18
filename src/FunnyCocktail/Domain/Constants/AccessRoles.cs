namespace Domain.Constants
{
    public class AccessRoles
    {
        public const string Owner = "owner";
        public const string Admin = "admin";
        public const string User = "user";
        public const string Staff = $"owner, admin";
        public const string Everyone = $"owner, admin, user";
    }
}
