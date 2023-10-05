namespace TasksAPI.Models
{
	public class Note
	{
		public Guid NoteId { get; set; }
		public string Title { get; set; } = string.Empty;
		public Guid SubTaskId { get; set; }
		public SubTask SubTask { get; set; } = new SubTask();
	}
}
