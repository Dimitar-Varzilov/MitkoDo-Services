namespace TasksAPI.Models
{
	public class SubTask
	{
		public Guid SubTaskId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int PicturesCountToBeCompleted { get; set; } = 0;
		public int NotesCountToBeCompleted { get; set; } = 1;
		public bool IsCompleted 
		{ 
			get => Notes.Count >= NotesCountToBeCompleted && Pictures.Count >= PicturesCountToBeCompleted;
		} 
		public Guid ToDoId { get; set; }
		public ToDo Todo { get; set; } = new ToDo();

		public IList<Note> Notes { get; set; } = [];
		public IList<Picture> Pictures { get; set; } = [];
	}
}
