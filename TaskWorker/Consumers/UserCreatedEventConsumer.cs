namespace TaskWorker.Consumers
{
	using AuthenticationAPI.Events;
	using MassTransit;
	using System.Threading.Tasks;
	using TasksAPI.Models;

	public class UserCreatedEventConsumerDefinition :
		ConsumerDefinition<UserCreatedEventConsumer>
	{
		public UserCreatedEventConsumerDefinition()
		{
			EndpointName = "task.user-created";
		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}

	public class UserCreatedEventConsumer(TaskContext taskContext) :
		IConsumer<UserCreatedEvent>
	{
		private readonly TaskContext _taskContext = taskContext;
		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			UserCreatedEvent message = context.Message;
			Employee employee = await _taskContext.Employees.FindAsync(message.UserId);
			if (employee != null)
				return;

			Employee newEmployee = new()
			{
				EmployeeId = message.UserId,
				Name = message.Name,
			};
			_taskContext.Employees.Add(newEmployee);
			await _taskContext.SaveChangesAsync();
		}
	}
}