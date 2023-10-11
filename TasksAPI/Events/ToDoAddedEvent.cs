namespace TasksAPI.Events
{
	public class ToDoAddedEvent
	{
		public Guid ToDoId { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime DueDate { get; set; }
		public IList<Guid> EmployeeIds { get; set; } = [];
	}
}
