using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class ToDoDto
	{
		public Guid TodoId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime StartDate { get; set; } = DateTime.Now;
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
