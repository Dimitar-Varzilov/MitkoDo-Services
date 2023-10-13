namespace EmployeeWorker.Consumers
{
	using MassTransit;

	public class PictureAndNoteAddedEventConsumerDefinition :
		ConsumerDefinition<PictureAndNoteAddedEventConsumer>
	{
		public PictureAndNoteAddedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<PictureAndNoteAddedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
}