namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class EmployeeAssignedEventConsumerDefinition :
		ConsumerDefinition<EmployeeAssignedEventConsumer>
	{
        public EmployeeAssignedEventConsumerDefinition()
        {
			ConcurrentMessageLimit = 1;
		}

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EmployeeAssignedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}