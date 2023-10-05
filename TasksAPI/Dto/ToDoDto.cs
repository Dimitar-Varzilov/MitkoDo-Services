using System.ComponentModel.DataAnnotations;
using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class ToDoDto
	{
		public Guid TodoId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
		public IList<SubTaskDto> SubTasks { get; set; } = [];
		public IList<EmployeeDto> Employees { get; set; } = [];

		public ToDoDto() { }

		public ToDoDto(ToDo toDo)
		{
			TodoId = toDo.ToDoId;
			Title = toDo.Title;
			Description = toDo.Description;
			StartDate = toDo.StartDate;
			DueDate = toDo.DueDate;
			SubTasks = [.. toDo.SubTasks.Select(subTask => new SubTaskDto(subTask))];
			Employees = [.. toDo.Employees.Select(employee => new EmployeeDto(employee))];

		}
	}
}
