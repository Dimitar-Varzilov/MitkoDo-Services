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
			string responseToken = _authService.LoginUser(request);
			if (responseToken == StatusCodes.Status404NotFound.ToString()) return BadRequest("User not found");
			if (responseToken == StatusCodes.Status400BadRequest.ToString()) return BadRequest("Invalid password");
			return Ok(responseToken);
		}
		[HttpPost("verifyToken")]
		public IActionResult ValidateTokenFromCookie()
		{
			string? token = Utilities.GetJwtTokenFromCookie(Request);
			if (token == null)
				return BadRequest("Invalid token or error getting token");
			return _authService.ValidateToken(token) ? Ok() : BadRequest();
		}

		[HttpPost("verifyTokenFromHeader")]
		public IActionResult ValidateTokenFromHeader()
		{
			string? token = Utilities.GetJwtTokenFromHeader(Request);
			if (token == null)
				return BadRequest("Invalid token or error getting token");
			return _authService.ValidateToken(token) ? Ok() : BadRequest();
		}

		[HttpPost("logout")]
		public ActionResult<string> Logout()
		{
			Response.Cookies.Delete("token");
			return Ok("You have been logged out");
		}
	}
}
