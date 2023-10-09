namespace Gateway.Events
{
	public record UserCreatedEvent(Guid UserId)
	{
		public Guid UserId { get; init; } = UserId;
	}
}