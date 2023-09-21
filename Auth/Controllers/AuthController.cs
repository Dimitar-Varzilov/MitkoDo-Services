using Auth.Dto;
using Auth.Models;
using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		private readonly IAuthService _authService = authService;

		[HttpGet("{id}")]
		public ActionResult Get(string id)
		{
			User? userFound = _authService.GetUserById(id);
			if (userFound == null)
			{
				return NotFound();
			}
			return Ok(userFound);
		}

		[HttpPost("register")]
		public int Register(RegisterDto request)
		{
			return _authService.RegisterUser(request);
		}


		[HttpPost("login")]
		public string Login(LoginDto request)
		{
			return _authService.LoginUser(request);
		}

		[HttpPost("verifyToken")]
		public bool VerifyToken([FromBody] VerifyTokenDto prop)
		{
			string token = prop.Token;
			return _authService.ValidateToken(token);
		}

		[HttpDelete("{id}")]
		public int Delete(string id)
		{
			return _authService.DeleteUser(id);
		}

	}
}
