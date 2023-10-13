namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class AssignEmployeeEventConsumerDefinition :
		ConsumerDefinition<AssignEmployeeEventConsumer>
	{
        public AssignEmployeeEventConsumerDefinition()
        {
			ConcurrentMessageLimit = 1;
		}

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<AssignEmployeeEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}