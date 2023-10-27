using AuthenticationAPI.Models;

namespace AuthenticationAPI.Dto
{
	public class UserDto(User user)
	{
		public Guid UserId { get; set; } = user.UserId;
		public string Email { get; set; } = user.Email;
	}
}
