namespace TasksAPI.Models
{
	public class Employee
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;
		public Guid ToDoId { get; set; }
		public ToDo ToDo { get; set; } = new ToDo();
	}
}
