namespace Auth.Models.Users
{
    public interface IBaseUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public interface IUser : IBaseUser
    {
        string Id { get; set; }
    }
    public class User : IUser
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
