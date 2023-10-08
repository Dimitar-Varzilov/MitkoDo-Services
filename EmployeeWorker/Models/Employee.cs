using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeWorker.Models
{
	public class Employee
	{
		public Guid EmployeeId { get; set; }
		public string Name { get; set; } = string.Empty;
		public bool IsAvailable => !ToDos.Any(todo => todo.IsActive == true);
		public ICollection<ToDo> ToDos { get; set; } = [];
		public ICollection<SubTask> SubTasks { get; set; } = [];
		public Guid UserId { get; set; }
	}
}
