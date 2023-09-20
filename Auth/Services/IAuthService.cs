using Auth.Dto;
using Auth.Models.Users;

namespace Auth.Services
{
    public interface IAuthService
    {
        public User? GetUserById(string id);
        public User? Register(RegisterDto user);
        public User? Login(LoginDto user);
    }
}
