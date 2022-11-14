namespace GypooWebAPI.Models
{
    public class UserDTO
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;

        public string confirmPassword { get; set; } = null!;
    }
}