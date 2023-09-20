using Auth.Interfaces;

namespace Auth.Dto
{
	public class RegisterDto : IBaseUser
	{
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public string ConfirmPassword { get; set; } = null!;
	}
}
