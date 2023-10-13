namespace EmployeeWorker.Consumers
{
    using MassTransit;

    public class SubTaskDeletedEventConsumerDefinition :
        ConsumerDefinition<SubTaskDeletedEventConsumer>
    {
		public SubTaskDeletedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;
		}

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubTaskDeletedEventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}