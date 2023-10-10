namespace EmployeeAPI.Models
{
	public class Employee
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsAvailable => !ToDos.Any(todo => todo.IsActive == true);
		public ICollection<ToDo> ToDos { get; set; } = [];
		public ICollection<SubTask> SubTasks { get; set; } = [];
	}
}
