using AuthenticationAPI.Dto;
using AuthenticationAPI.Enums;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController(IAuthService authService) : ControllerBase
	{
		private readonly IAuthService _authService = authService;

		[HttpPost("register/employee")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto request)
		{
			if (request.Password != request.ConfirmPassword) return BadRequest("Passwords do not match");
			UserDto? createdUser = await _authService.RegisterUser(request);
			return (createdUser == null)
				? BadRequest("Email is already registered")
				: Ok(createdUser);
		}

		[HttpPost("register/manager")]
		public async Task<ActionResult<UserDto>> RegisterManager(RegisterDto request)
		{
			if (request.Password != request.ConfirmPassword) return BadRequest("Passwords do not match");
			UserDto? createdUser = await _authService.RegisterUser(request, UserRole.MANAGER);
			return (createdUser == null)
				? BadRequest("Email is already registered")
				: Ok(createdUser);
		}

		[HttpPost("login")]
		public IActionResult Login(LoginDto request)
		{
			string token = _authService.LoginUser(request);
			if (token == StatusCodes.Status404NotFound.ToString()) return BadRequest("User not found");
			if (token == StatusCodes.Status400BadRequest.ToString()) return BadRequest("Invalid password");
			return Ok(token);
		}

		[HttpPost("verifyToken")]
		public IActionResult ValidateToken([FromBody] ValidateTokenDto prop)
		{
			string token = prop.Token;
			return _authService.ValidateToken(token) ? Ok() : BadRequest();
		}

		[HttpPost("logout")]
		public IActionResult Logout(ValidateTokenDto prop)
		{
			string token = prop.Token;
			return Ok(_authService.InvalidateToken(token));
		}
	}
}
