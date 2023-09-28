using System.ComponentModel.DataAnnotations;
using Tasks.Enums;

namespace Tasks.Models
{
	public interface ITask
	{
		int TaskId { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		CustomTaskStatus Status { get; }
		DateTime StartDate { get; set; }
		DateTime DueDate { get; set; }
		ICollection<SubTask> SubTasks { get; set; }
	}
	public class CustomTask : ITask
	{
		[Key]
		public int TaskId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		[Required]
		public CustomTaskStatus Status
		{
			get
			{
				CustomTaskStatus customTaskStatus = CustomTaskStatus.Upcoming;
				return customTaskStatus;
			}
		}
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);

		public ICollection<SubTask> SubTasks { get; set; } = [];

	}
}
