using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{
	public class EmployeeIdsDto
	{
		[Required]
		public ICollection<Guid> EmployeeIds { get; set; } = [];
	}
}
