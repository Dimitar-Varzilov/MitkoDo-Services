using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tasks.Interfaces;

namespace Tasks.Models
{
	public class CustomTask : ITask, IId
	{
		[Key]
		public Guid Id { get; set; } = Guid.NewGuid();
		[Required]
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public TaskStatus Status { get; set; } = TaskStatus.WaitingToRun;
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime DueDate { get; set; }
		[ForeignKey("UserId")]
		public Guid[] SubTaskId { get; set; } = [];
		[ForeignKey("MemberId")]
		public Guid[] MemberId { get; set; } = [];
	}
}
