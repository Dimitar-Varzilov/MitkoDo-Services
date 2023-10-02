using AuthenticationAPI.Dto;
using AuthenticationAPI.Models;
using AuthenticationAPI.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController(IAuthService authService, IPublishEndpoint publishEndpoint) : ControllerBase
	{
		private readonly IAuthService _authService = authService;
		private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

		[HttpGet("{id}")]
		public IActionResult Get(Guid id)
		{
			User? userFound = _authService.GetUserById(id);
			return (userFound == null)
				? NotFound()
				: Ok(userFound);
		}

		[HttpPost("register")]
		public IActionResult Register(RegisterDto request)
		{
			UserDto? response = _authService.RegisterUser(request);
			return (response == null)
				? BadRequest("Email is already registered")
				: Ok("User created successfully");
		}

		[HttpPost("login")]
		public IActionResult Login(LoginDto request)
		{
			string response = _authService.LoginUser(request);
			if (response == StatusCodes.Status404NotFound.ToString()) return BadRequest("User not found");
			if (response == StatusCodes.Status400BadRequest.ToString()) return BadRequest("Invalid password");
			return Ok(response);
		}

		[HttpPost("verifyToken")]
		public IActionResult ValidateToken([FromBody] ValidateTokenDto prop)
		{
			string token = prop.Token;
			return _authService.ValidateToken(token) ? Ok() : BadRequest();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(Guid id)
		{
			int response = _authService.DeleteUser(id);
			if (response == StatusCodes.Status404NotFound) return NotFound();
			return Ok("User successfully deleted");
		}

	}
}
