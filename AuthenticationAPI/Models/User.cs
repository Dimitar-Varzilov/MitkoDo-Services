namespace AuthenticationAPI.Models
{
	public class User
	{
		public Guid UserId { get; set; } = Guid.Empty;
		public string Email { get; set; } = string.Empty;
		public byte[] PasswordHash { get; set; } = [];
		public byte[] PasswordSalt { get; set; } = [];
	}

}
