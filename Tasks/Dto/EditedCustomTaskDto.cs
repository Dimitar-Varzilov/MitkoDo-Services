using Tasks.Enums;

namespace Tasks.Dto
{
	public class EditedCustomTaskDto : CustomTaskDto
	{
		public CustomTaskStatus Status { get; set; } = CustomTaskStatus.Upcoming;
	}
}
