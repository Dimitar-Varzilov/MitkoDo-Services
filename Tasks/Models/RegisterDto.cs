namespace Tasks.Models
{
	public class RegisterDto
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string ConfirmPassword { get; set; } = null!;

	}
}
