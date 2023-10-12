namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class UserCreatedEventConsumerDefinition : ConsumerDefinition<UserCreatedEventConsumer>
	{
		public UserCreatedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;
		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}