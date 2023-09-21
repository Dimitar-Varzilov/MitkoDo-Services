using Auth.Dto;
using Auth.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
			return Program.Users.Find(user => user.Id == id);
		}

		public User? GetUserByEmail(string email)
		{
			return Program.Users.Find(user => user.Email == email);
		}

		public User Register(RegisterDto user)
		{


			return new User();
		}

		public User? Login(LoginDto user)
		{
			User newUser = _mapper.Map<User>(user);
			newUser = new();

			User? userFound = Program.Users.Find(u => u.Email == user.Email);

			return userFound;
		}

	}
}
