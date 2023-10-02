using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Dto
{
	public class RegisterDto
	{
		[EmailAddress]
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string ConfirmPassword { get; set; } = null!;
	}
}
