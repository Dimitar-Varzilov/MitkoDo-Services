namespace Auth.Models.Users
{
	public interface IUser
	{
		string? Id { get; set; }
		string email { get; set; }
		string password { get; set; }
		string[] tasks { get; set; }
	}
}
