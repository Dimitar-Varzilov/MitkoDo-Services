using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Dto
{
	public record RegisterDto
	{
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
