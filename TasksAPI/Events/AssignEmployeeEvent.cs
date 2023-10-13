namespace TasksAPI.Events
{
	public class AssignEmployeeEvent
	{
		public Guid ToDoId { get; set; }
		public IList<Guid> EmployeeIds { get; set; } = [];
        public AssignEmployeeEvent()
        {
            
        }

        public AssignEmployeeEvent(Guid toDoId, IList<Guid> employeeIds)
        {
            ToDoId = toDoId;
			EmployeeIds = employeeIds;
        }
    }
}
