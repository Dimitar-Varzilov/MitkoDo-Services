using MassTransit;
using Tasks.Models;

namespace Tasks
{
	public class UserConsumer : IConsumer<UserDto>
	{
		public async Task Consume(ConsumeContext<UserDto> context)
		{
			var user = context.Message;
			await Console.Out.WriteLineAsync($"User created: {user.Email}");
		}
	}
}
