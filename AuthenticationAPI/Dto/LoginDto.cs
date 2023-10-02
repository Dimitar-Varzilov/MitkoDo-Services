using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Dto
{
	public class LoginDto
	{
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
