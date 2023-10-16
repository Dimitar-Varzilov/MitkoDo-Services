namespace EmployeeAPI.Models
{
	public class Employee
	{
		public Guid EmployeeId { get; set; } = Guid.Empty;
		public string Name { get; set; } = string.Empty;
		public bool IsAvailable => !ToDos.Any(todo => todo.IsActive == true);
		public IList<ToDo> ToDos { get; set; } = [];
		public IList<Note> Notes { get; set; } = [];
		public IList<Picture> Pictures { get; set; } = [];
	}
}
