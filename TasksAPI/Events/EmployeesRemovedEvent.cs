namespace TasksAPI.Events
{
	public class EmployeesRemovedEvent
	{
		public Guid ToDoId { get; set; }
		public IList<Guid> EmployeeIds { get; set; } = [];
		public EmployeesRemovedEvent()
		{

		}
		public EmployeesRemovedEvent(Guid toDoId, IList<Guid> employeeIds)
		{
			ToDoId = toDoId;
			EmployeeIds = employeeIds;
		}
	}
}
