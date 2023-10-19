namespace TasksAPI.Models
{
	public class Note
	{
		public Guid NoteId { get; set; } = Guid.Empty;
		public string Title { get; set; } = string.Empty;
		public Guid SubTaskId { get; set; } = Guid.Empty;
		public SubTask SubTask { get; set; } = new SubTask();
		public Guid UploadedBy { get; set; } = Guid.Empty;
	}
}
