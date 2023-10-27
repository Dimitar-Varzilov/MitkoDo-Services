namespace TasksAPI.Events
{
	public class ToDoDeletedEvent
	{
		public Guid ToDoId { get; set; }
		public ToDoDeletedEvent()
		{

		}
		public ToDoDeletedEvent(Guid toDoId)
		{
			ToDoId = toDoId;
		}
	}
}
