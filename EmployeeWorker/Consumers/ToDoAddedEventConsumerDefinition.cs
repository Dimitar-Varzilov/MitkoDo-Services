namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class ToDoAddedEventConsumerDefinition :
		ConsumerDefinition<ToDoAddedEventConsumer>
	{
		public ToDoAddedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;

		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator configurator, IConsumerConfigurator<ToDoAddedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			configurator.UseMessageRetry(r => r.Intervals(100, 1000, 2000, 5000));
		}
	}
}