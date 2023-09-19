using System.ComponentModel.DataAnnotations;

namespace Auth.Models.Users
{
    public class User : IUser
    {
        [Key]
        public string? Id { get; set; } = string.Empty;
        [Required]
        public string email { get; set; } = string.Empty;
        [Required]
        public string password { get; set; } = string.Empty;
		public string[] tasks { get; set; } = [];
    }
}