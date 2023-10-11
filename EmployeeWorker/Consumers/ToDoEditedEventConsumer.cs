namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;
	public class ToDoEditedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<ToDoEditedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<ToDoEditedEvent> context)
		{
			ToDoEditedEvent message = context.Message;

			var employees = _employeeContext.Employees.Where(e => e.ToDos.Any(t => t.ToDoId == message.ToDoId));

			ToDo toDoToEdit = employees.First().ToDos.First();

			_employeeContext.Entry(toDoToEdit).CurrentValues.SetValues(message);

			await _employeeContext.SaveChangesAsync();
		}
	}
}
