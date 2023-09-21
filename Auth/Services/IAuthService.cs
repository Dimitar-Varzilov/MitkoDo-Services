using Auth.Dto;
using Auth.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Services
{
    public interface IAuthService
	{
		User? GetUserById(string id);
		User Register(RegisterDto user);
		User? Login(LoginDto user);
		User? GetUserByEmail(string email);
	}
}
