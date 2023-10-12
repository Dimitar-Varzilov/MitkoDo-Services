using TasksAPI.Models;

namespace TasksAPI.Events
{
	public class ToDoAddedEvent
	{
		public Guid ToDoId { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime DueDate { get; set; }
		public IList<Guid> EmployeeIds { get; set; } = [];

		public ToDoAddedEvent()
		{

		}
		public ToDoAddedEvent(ToDo toDo)
		{
			ToDoId = toDo.ToDoId;
			Title = toDo.Title;
			StartDate = toDo.StartDate;
			DueDate = toDo.DueDate;
			EmployeeIds = toDo.Employees.Select(employee => employee.EmployeeId).ToList();
		}
	}
}
