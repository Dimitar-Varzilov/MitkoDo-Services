namespace EmployeeWorker.Consumers
{
    using MassTransit;

    public class SubTaskEditedEventConsumerDefinition :
        ConsumerDefinition<SubTaskEditedEventConsumer>
    {
        public SubTaskEditedEventConsumerDefinition()
        {
			ConcurrentMessageLimit = 1;
		}

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubTaskEditedEventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}