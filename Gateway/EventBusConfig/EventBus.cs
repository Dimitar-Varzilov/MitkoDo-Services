using MassTransit;

namespace Gateway.EventBusConfig
{
	public class EventBus(IPublishEndpoint publishEndpoint) : IEventBus
	{
		private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
		public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
		{
			_publishEndpoint.Publish(message, cancellationToken);
			return Task.CompletedTask;
		}
	}
}
