namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class UserCreatedEventDefinition :
		ConsumerDefinition<UserCreatedEventConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}