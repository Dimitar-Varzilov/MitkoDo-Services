namespace EmployeeWorker.Consumers
{
    using MassTransit;

    public class EmployeesRemovedEventConsumerDefinition :
        ConsumerDefinition<EmployeesRemovedEventConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EmployeesRemovedEventConsumer> consumerConfigurator, IRegistrationContext context)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
        }
    }
}