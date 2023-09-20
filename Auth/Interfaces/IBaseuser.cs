namespace Auth.Interfaces
{
	public interface IBaseUser
	{
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
