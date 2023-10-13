namespace EmployeeWorker.Consumers
{
    using MassTransit;

    public class ToDoDeletedEventConsumerDefinition :
        ConsumerDefinition<ToDoDeletedEventConsumer>
    {
		public ToDoDeletedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;
		}

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ToDoDeletedEventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}