using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasks.Interfaces
{
	public interface ITask
	{
		string Title { get; set; }
		string Description { get; set; }
		TaskStatus Status { get; set; }
		DateTime StartDate { get; set; }
		DateTime DueDate { get; set; }
		Guid[] SubTaskId { get; set; }
		Guid[] MemberId { get; set; }
	}
}
