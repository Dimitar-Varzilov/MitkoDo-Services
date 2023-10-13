namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class ToDoDeletedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<ToDoDeletedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<ToDoDeletedEvent> context)
		{
			ToDoDeletedEvent message = context.Message;
			ToDo toDoToDelete = _employeeContext.ToDos.FirstOrDefault(task => task.ToDoId == message.ToDoId);
			if (toDoToDelete == null) return;

			_employeeContext.Remove(toDoToDelete);
			await _employeeContext.SaveChangesAsync();
		}
	}
}