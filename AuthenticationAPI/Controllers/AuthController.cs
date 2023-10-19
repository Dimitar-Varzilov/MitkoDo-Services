using AuthenticationAPI.Dto;
using AuthenticationAPI.Enums;
using AuthenticationAPI.Services;
using Microsoft.AspNetCore.Authorization;
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
		public ActionResult<string> Login(LoginDto request)
		{
			string responseToken = _authService.LoginUser(request);
			if (responseToken == StatusCodes.Status400BadRequest.ToString()) return BadRequest("Invalid email or password");
			return Ok(responseToken);
		}

		[Authorize]
		[HttpPost("verifyTokenFromHeader")]
		public IActionResult ValidateToken()
		{
			string? token = Utilities.GetJwtToken(Request);
			if (token == null)
				return BadRequest("Invalid token or error getting token");
			return _authService.ValidateToken(token) ? Ok() : BadRequest();
		}

		[Authorize]
		[HttpPost("changePassword")]
		public async Task<ActionResult<int>> ChangeUserPassword(ChangePasswordDto changePasswordDto)
		{
			if (changePasswordDto == null)
				return BadRequest("Invalid token or error getting token");
			int result = await _authService.ChangePassword(changePasswordDto);
			return StatusCode(result);
		}
	}
}
