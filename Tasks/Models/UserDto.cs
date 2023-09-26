using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.Models
{
	public class UserDto
	{
		[Key]
		[ForeignKey("Member")]
		public int UserId { get; set; }
		public string Email { get; set; } = null!;

		public virtual Member Member { get; set; } = new Member();
	}
}
