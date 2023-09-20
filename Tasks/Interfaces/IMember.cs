using Tasks.Enums;

namespace Tasks.Interfaces
{
	public interface IMember
	{
		Guid[] Tasks { get; set; }
		MemberRole Role { get; set; }
		Guid UserId { get; set; }

	}
}
