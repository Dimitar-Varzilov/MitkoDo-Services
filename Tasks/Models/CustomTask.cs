using System.ComponentModel.DataAnnotations;

namespace Tasks.Models
{
    public interface ITask
	{
		int TaskId { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		TaskStatus Status { get; set; }
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
		public TaskStatus Status { get; set; } = TaskStatus.WaitingToRun;
		[Required]
		public DateTime StartDate { get; set; }
		[Required]
		public DateTime DueDate { get; set; }

		public int SubTaskId { get; set; }
		public virtual ICollection<SubTask> SubTasks { get; set; } = [];

		public int MemberId { get; set; }
		public virtual ICollection<Member> Members { get; set; } = [];
	}
}
