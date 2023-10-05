using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class EmployeeDto
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;

		public EmployeeDto() { }
		public EmployeeDto(Employee employee)
		{
			EmployeeId = employee.EmployeeId;
			Name = employee.Name;
		}
	}
}
