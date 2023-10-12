namespace TaskWorker.Consumers
{
	using MassTransit;

	public class UserCreatedEventConsumerDefinition :
		ConsumerDefinition<UserCreatedEventConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}