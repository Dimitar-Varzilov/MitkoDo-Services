using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{
	public class CreateToDoDto
	{
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
		[Required]
		public IList<Guid> EmployeeIds { get; set; } = [];
	}
}
