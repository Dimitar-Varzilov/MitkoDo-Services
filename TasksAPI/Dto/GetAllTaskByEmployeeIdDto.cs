using TasksAPI.Enums;
using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class GetAllTaskByEmployeeIdDto
	{
		public Guid TodoId { get; set; }
		public string Title { get; set; } = "";
		public string Description { get; set; } = "";
		public DateTime StartDate { get; set; }
		public DateTime DueDate { get; set; }
		public bool IsComplete { get; set; }
		public ToDoStatusEnum Status { get; set; } = ToDoStatusEnum.Upcoming;
		public ICollection<SubTaskDto> SubTasks { get; set; } = [];

		public GetAllTaskByEmployeeIdDto() { }
		public GetAllTaskByEmployeeIdDto(ToDo t)
		{
			TodoId = t.ToDoId;
			Title = t.Title;
			Description = t.Description;
			StartDate = t.StartDate;
			DueDate = t.DueDate;
			Status = Utilities.CalculateTaskStatus(t);
			IsComplete = Utilities.IsTaskCompleted(t);
			SubTasks = t.SubTasks.Select(s => new SubTaskDto()
			{
				SubTaskId = s.SubTaskId,
				Title = s.Title,
				Description = s.Description,
				IsComplete = Utilities.CalculateSubTaskStatus(s)
			}).ToList();

		}
	}
}
