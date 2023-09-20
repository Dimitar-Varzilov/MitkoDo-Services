using Auth.Dto;
using Auth.Models.Users;
using AutoMapper;

namespace Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        public AuthService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public User? GetUserById(string id)
        {
            Console.WriteLine(id);
            return Program.users.Find(user => user.Id == id);
        }

        public User? GetUserByEmail(string email)
        {
            return Program.users.Find(user => user.Email == email);
        }

        public User? Register(RegisterDto user)
        {

            if (Program.users.Any(u => u.Email == user.Email))
            {
                return null;
            }
            int odd = Math.Abs(DateTime.Now.Millisecond);

            User newUser = _mapper.Map<User>(user);
            newUser = new()
            {
                Id = odd.ToString(),
                Email = user.Email,
                Password = PasswordUtils.HashPassword(user.Password),
            };
            Program.users.Add(newUser);

            return newUser;
        }

        public User? Login(LoginDto user)
        {
            User? userFinded = Program.users.Find(u => u.Email == user.Email);
            if (userFinded == null)
            {
                return null;
            }
            if (!PasswordUtils.VerifyPassword(user.Password, userFinded.Password))
            {
                return null;
            }
            return userFinded;
        }
    }
}
