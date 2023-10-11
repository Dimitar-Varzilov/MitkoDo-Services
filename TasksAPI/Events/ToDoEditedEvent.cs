namespace TasksAPI.Events
{
	public class ToDoEditedEvent
	{
		public Guid ToDoId { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime DueDate { get; set; }
	}
}
