using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.Models
{
	public class User
	{
		[Key]
		public int UserId { get; set; }
		public string Email { get; set; } = null!;

		public int MemberId { get; set; }
		public virtual Member Member { get; set; } = new Member();
	}
}
