using EmployeeAPI.Models;

namespace EmployeeAPI.Dto
{
	public class ToDoDto
	{
		public Guid ToDoId { get; set; }
		public string Title { get; set; } = string.Empty;

        public ToDoDto(ToDo toDo)
        {
            ToDoId = toDo.ToDoId;
			Title = toDo.Title;
        }
    }
}
