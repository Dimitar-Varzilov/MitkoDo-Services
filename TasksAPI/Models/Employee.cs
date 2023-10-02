using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models
{
	public class Employee
	{
		[Key]
		public int EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
