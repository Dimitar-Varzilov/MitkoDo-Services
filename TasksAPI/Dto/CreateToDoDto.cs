using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{

	public class CreateToDoDto
	{
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
	}
}
