using EmployeeAPI.Models;

namespace EmployeeAPI.Dto
{
	public class ToDoDto(ToDo toDo)
	{
		public Guid ToDoId { get; set; } = toDo.ToDoId;
		public string Title { get; set; } = toDo.Title;
	}
}
