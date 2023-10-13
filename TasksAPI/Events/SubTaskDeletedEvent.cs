namespace TasksAPI.Events
{
	public class SubTaskDeletedEvent
	{
		public Guid SubTaskId { get; set; }
        public SubTaskDeletedEvent()
        {
            
        }
        public SubTaskDeletedEvent(Guid subTaskId)
		{
			SubTaskId = subTaskId;
		}
	}
}
