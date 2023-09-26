using System.ComponentModel.DataAnnotations;
using Tasks.Models;

namespace Tasks.Dto
{

    public class CustomTaskDto
    {
		[Required]
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public TaskStatus Status { get; set; } = TaskStatus.WaitingToRun;
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime DueDate { get; set; }
	}
}
