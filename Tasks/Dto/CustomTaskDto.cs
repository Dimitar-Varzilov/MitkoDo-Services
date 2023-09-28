using System.ComponentModel.DataAnnotations;
using Tasks.Enums;
using Tasks.Models;

namespace Tasks.Dto
{

	public class CustomTaskDto
	{
		[Required]
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
	}
}
