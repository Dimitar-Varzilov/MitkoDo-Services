using Auth.Models.Users;

namespace Auth.Dto
{
    public class LoginDto : IUser
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
