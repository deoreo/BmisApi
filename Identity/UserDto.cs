namespace BmisApi.Identity
{
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Role {  get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public int AccessFailedCount { get; set; }
    }
}
