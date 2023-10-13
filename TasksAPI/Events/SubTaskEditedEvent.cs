using TasksAPI.Models;

namespace TasksAPI.Events
{
	public class SubTaskEditedEvent
	{
		public Guid SubTaskId { get; set; } = default!;
		public string Title { get; set; } = default!;

        public SubTaskEditedEvent()
        {
            
        }

        public SubTaskEditedEvent(SubTask subTaskToEdit)
        {
            SubTaskId = subTaskToEdit.SubTaskId;
			Title = subTaskToEdit.Title;
        }
    }
}
