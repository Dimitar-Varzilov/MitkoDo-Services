using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Dto
{
	public record RegisterDto
	{
		[Required(ErrorMessage = "Email is required")]
		[EmailAddress(ErrorMessage = "Please provide valid email address")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Name is required")]
		[StringLength(255, ErrorMessage = "Name must be between 4 and 255 characters", MinimumLength = 4)]
		public string Name { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string Password { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string ConfirmPassword { get; set; } = string.Empty;
	}
}
