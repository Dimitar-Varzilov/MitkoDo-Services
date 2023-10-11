namespace AuthenticationAPI.Events
{
	public record UserCreatedEvent(Guid UserId, string Name)
	{
		public Guid UserId { get; init; } = UserId;
		public string Name { get; init; } = Name;
	}
}