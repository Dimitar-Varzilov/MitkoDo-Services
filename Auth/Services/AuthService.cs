using Auth.Dto;
using Auth.Models.Users;
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
			return Program.users.Find(user => user.Id == id);
		}

		public User? GetUserByEmail(string email)
		{
			return Program.users.Find(user => user.Email == email);
		}

		public User Register(RegisterDto user)
		{


			return new User();
		}

		public User? Login(LoginDto user)
		{
			User newUser = _mapper.Map<User>(user);
			newUser = new();

			User? userFound = Program.users.Find(u => u.Email == user.Email);

			return userFound;
		}

	}
}
