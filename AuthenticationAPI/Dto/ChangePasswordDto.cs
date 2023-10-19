using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPI.Dto
{
	public class ChangePasswordDto
	{
		[Required]
		[EmailAddress(ErrorMessage = "Please provide valid email address")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string OldPassword { get; set; } = null!;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string NewPassword { get; set; } = null!;

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
		public string ConfirmNewPassword { get; set; } = null!;
	}
}
