namespace BmisApi.Identity
{
    public class RegisterModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
