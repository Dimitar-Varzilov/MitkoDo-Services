using Tasks.Enums;
using Tasks.Interfaces;

namespace Tasks.Models
{
	public class Member :IMember, IId
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public Guid[] Tasks { get; set; } = [];
		public MemberRole Role { get; set; }
		public Guid UserId { get; set; }
	}
}
