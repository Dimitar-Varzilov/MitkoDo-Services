namespace TasksAPI.Events
{
	public class EmployeeAssignedEvent
	{
		public Guid ToDoId { get; set; }
		public IList<Guid> EmployeeIds { get; set; } = [];
        public EmployeeAssignedEvent()
        {
            
        }

        public EmployeeAssignedEvent(Guid toDoId, IList<Guid> employeeIds)
        {
            ToDoId = toDoId;
			EmployeeIds = employeeIds;
        }
    }
}
