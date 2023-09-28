using System.ComponentModel.DataAnnotations;
using Tasks.Enums;

namespace Tasks.Models
{
	public interface ITask
	{
		int TaskId { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		CustomTaskStatus Status { get; set; }
		DateTime StartDate { get; set; }
		DateTime DueDate { get; set; }
		ICollection<SubTask> SubTasks { get; set; }
		ICollection<Member> Members { get; set; }
	}
	public class CustomTask : ITask
	{
		[Key]
		public int TaskId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public CustomTaskStatus Status { get; set; } = CustomTaskStatus.Upcoming;
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);

		public ICollection<SubTask> SubTasks { get; set; } = [];

		public ICollection<Member> Members { get; set; } = [];
	}
}
