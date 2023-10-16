namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	internal class ToDoEditedEventConsumerDefinition :
		ConsumerDefinition<ToDoEditedEventConsumer>
	{
		public ToDoEditedEventConsumerDefinition()
		{
			EndpointName = "employee.todo-edited";
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator configurator, IConsumerConfigurator<ToDoEditedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			configurator.UseMessageRetry(r => r.Intervals(100, 1000, 2000, 5000));
		}
	}
	public class ToDoEditedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<ToDoEditedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<ToDoEditedEvent> context)
		{
			ToDoEditedEvent message = context.Message;

			ToDo toDoToEdit = _employeeContext.ToDos.FirstOrDefault(t => t.ToDoId == message.ToDoId);

			_employeeContext.Entry(toDoToEdit).CurrentValues.SetValues(message);

			await _employeeContext.SaveChangesAsync();
		}
	}
}
