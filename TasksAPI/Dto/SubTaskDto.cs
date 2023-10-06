using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class SubTaskDto : CreateSubTaskDto
	{
		public Guid SubTaskId { get; set; }
		public bool IsComplete { get; set; } = false;

		public SubTaskDto() { }

		public SubTaskDto(SubTask subTask)
		{

			SubTaskId = subTask.SubTaskId;
			Title = subTask.Title;
			Description = subTask.Description;
			IsComplete = Utilities.CalculateSubTaskStatus(subTask);
			NotesCountToBeCompleted = subTask.NotesCountToBeCompleted;
			PicturesCountToBeCompleted = subTask.PicturesCountToBeCompleted;
		}
	}
}
