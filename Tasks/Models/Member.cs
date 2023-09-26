using System.ComponentModel.DataAnnotations;
using Tasks.Enums;

namespace Tasks.Models
{
    public class Member
	{
		[Key]
		public int MemberId { get; set; }
		public MemberRole Role { get; set; }

		public int TaskId { get; set; }
		public virtual ICollection<CustomTask> Tasks { get; set; } = [];

		public virtual UserDto User { get; set; } = new UserDto();
	}
}
