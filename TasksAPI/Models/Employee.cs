using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models
{
	public class Employee
	{
		public int EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;
	}
}
