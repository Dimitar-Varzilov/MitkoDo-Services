namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class AssignEmployeeEventConsumerConsumerDefinition :
		ConsumerDefinition<AssignEmployeeEventConsumerConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AssignEmployeeEventConsumerConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}