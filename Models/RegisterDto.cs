namespace BookApi.Models
{
    public class RegisterDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; } = "User"; //default role is User
    }
}
