namespace EmployeeWorker.Consumers
{
    using MassTransit;

    public class EmployeesRemovedEventConsumerDefinition :
        ConsumerDefinition<EmployeesRemovedEventConsumer>
    {
        public EmployeesRemovedEventConsumerDefinition()
        {
			ConcurrentMessageLimit = 1;
		}
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EmployeesRemovedEventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}