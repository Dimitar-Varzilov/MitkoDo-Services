using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Dto
{
	public record RegisterDto
	{
		[EmailAddress]
		public string Email { get; set; } = string.Empty;
		[Required(ErrorMessage ="")]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string Password { get; set; } = string.Empty;
		[Required(ErrorMessage = "Password is required")]
		[StringLength(255, ErrorMessage = "Must be between 8 and 255 characters", MinimumLength = 8)]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
