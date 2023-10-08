namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class employeeworkerConsumerDefinition :
		ConsumerDefinition<EmployeeWorkerConsumer>
	{
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EmployeeWorkerConsumer> consumerConfigurator)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}