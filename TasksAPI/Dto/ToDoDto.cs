using TasksAPI.Enums;

namespace TasksAPI.Dto
{
	public class ToDoDto : CreateToDoDto
	{
		public Guid? TodoId { get; set; }
		public ToDoStatusEnum Status { get; set; } = ToDoStatusEnum.Upcoming;
	}
}
