namespace EmployeeWorker.Consumers
{
	using MassTransit;

	internal class ToDoEditedEventConsumerDefinition :
		ConsumerDefinition<ToDoEditedEventConsumer>
	{
		public ToDoEditedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;

		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator configurator, IConsumerConfigurator<ToDoEditedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			configurator.UseMessageRetry(r => r.Intervals(100, 1000, 2000, 5000));
		}
	}

}
