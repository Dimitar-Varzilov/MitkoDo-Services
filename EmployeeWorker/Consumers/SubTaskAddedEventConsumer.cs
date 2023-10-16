namespace EmployeeWorker.Consumers
{
	using System.Threading.Tasks;
	using EmployeeAPI.Models;
	using MassTransit;
	using TasksAPI.Events;

	public class SubTaskAddedEventConsumerDefinition :
		ConsumerDefinition<SubTaskAddedEventConsumer>
	{
		public SubTaskAddedEventConsumerDefinition()
		{
			EndpointName = "employee.subtask-added";
		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubTaskAddedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
	public class SubTaskAddedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<SubTaskAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<SubTaskAddedEvent> context)
		{
			SubTaskAddedEvent message = context.Message;
			SubTask subTask = new()
			{
				SubTaskId = message.SubTaskId,
				Title = message.Title,
			};
			await _employeeContext.AddAsync(subTask);
			await _employeeContext.SaveChangesAsync();
		}
	}
}