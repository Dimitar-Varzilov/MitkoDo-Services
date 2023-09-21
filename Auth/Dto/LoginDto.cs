using Auth.Interfaces;
using Auth.Models;

namespace Auth.Dto
{
	public class LoginDto : IBaseUser
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}
}
