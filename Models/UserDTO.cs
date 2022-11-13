namespace GypooWebAPI.Models
{
    public class UserDTO
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;

        public string ConfirmPassword { get; set; } = null!;
    }
}