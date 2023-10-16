using EmployeeAPI.Models;

namespace EmployeeAPI.Dto
{
	public class EmployeeDto
	{
		public Guid EmployeeId { get; set; } = Guid.Empty;
		public string Name { get; set; } = string.Empty;
		public bool IsAvailable { get; set; } = false;
		public IList<ToDoDto> ToDos { get; set; } = [];

		public EmployeeDto() { }

		public EmployeeDto(Employee employee)
		{
			EmployeeId = employee.EmployeeId;
			Name = employee.Name;
			IsAvailable = employee.IsAvailable;
			ToDos = [.. employee.ToDos
				.Select(x => new ToDoDto(x))];
		}
	}
}
