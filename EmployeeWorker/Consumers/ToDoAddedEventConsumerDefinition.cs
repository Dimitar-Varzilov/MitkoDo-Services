namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class ToDoAddedEventConsumerDefinition :
		ConsumerDefinition<ToDoAddedEventConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ToDoAddedEventConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}