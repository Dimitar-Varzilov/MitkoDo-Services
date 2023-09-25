using Auth.Dto;
using Auth.Models;
using Auth.Services;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController(IAuthService authService,IPublishEndpoint publishEndpoint) : ControllerBase
	{
		private readonly IAuthService _authService = authService;
		private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

		[HttpGet("{id}")]
		public IActionResult Get(string id)
		{
			User? userFound = _authService.GetUserById(id);
			if (userFound == null)
			{
				return NotFound();
			}
			return Ok(userFound);
		}

		[HttpPost("register")]
		//public IActionResult Register(RegisterDto request)
		//{
		//	int response = _authService.RegisterUser(request);
		//	if (response == StatusCodes.Status409Conflict) return BadRequest("Email is already registered");
		//	return Ok("User created successfully");
		//}
		public async Task<IActionResult> Register(RegisterDto user)
		{
			await _publishEndpoint.Publish(user);
			return Ok("User created successfully2");
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
		public IActionResult Delete(string id)
		{
			int response = _authService.DeleteUser(id);
			if (response == StatusCodes.Status404NotFound) return NotFound();
			return Ok("User successfully deleted");
		}

	}
}
