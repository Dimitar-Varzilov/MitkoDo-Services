namespace TasksAPI.Models
{
	public class Employee
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;
		public IList<ToDo> Tasks { get; set; } = [];
	}
}
