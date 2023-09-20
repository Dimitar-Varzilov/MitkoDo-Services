using Auth.Models.Users;

namespace Auth.Dto
{
    public interface IRegisterDto : IBaseUser
    {

    }
    public class RegisterDto : IRegisterDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
