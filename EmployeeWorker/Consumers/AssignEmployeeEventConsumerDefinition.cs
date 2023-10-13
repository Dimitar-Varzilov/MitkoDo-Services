namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class AssignEmployeeEventConsumerDefinition :
		ConsumerDefinition<AssignEmployeeEventConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AssignEmployeeEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}