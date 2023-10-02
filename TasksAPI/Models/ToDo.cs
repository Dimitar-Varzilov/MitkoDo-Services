using TasksAPI.Enums;

namespace TasksAPI.Models
{
	public class ToDo
	{
		public Guid ToDoId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public ToDoStatusEnum Status
		{
			get => Utilities.CalculateTaskStatus(StartDate, DueDate, SubTasks);
		}
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);

		public IList<SubTask> SubTasks { get; set; } = [];
		public IList<Employee> EmployeeIds { get; set; } = [];

	}
}
