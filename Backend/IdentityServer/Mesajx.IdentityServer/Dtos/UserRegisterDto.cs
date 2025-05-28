namespace Mesajx.IdentityServer.Dtos
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; } 
        public string? ProfileImageUrl { get; set; }
        public DateTime BirthDate { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
