using System.ComponentModel.DataAnnotations;
using TasksAPI.Enums;

namespace TasksAPI.Models
{
	public class ToDo
	{
		public Guid ToDoId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public ToDoStatusEnum Status
		{
			get => Utilities.CalculateTaskStatus(StartDate, DueDate, SubTasks);
		}
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);

		public IList<SubTask> SubTasks { get; set; } = [];
		public IList<Guid> EmployeeIds { get; set; } = [];

	}
}
