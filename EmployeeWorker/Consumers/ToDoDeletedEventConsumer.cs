namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class ToDoDeletedEventConsumerDefinition :
		ConsumerDefinition<ToDoDeletedEventConsumer>
	{
		public ToDoDeletedEventConsumerDefinition()
		{
			EndpointName = "employee.todo-deleted";
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ToDoDeletedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
			endpointConfigurator.UseCircuitBreaker();
		}
	}

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