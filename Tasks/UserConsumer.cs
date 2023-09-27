using MassTransit;
using Tasks.Models;

namespace Tasks
{
	public class UserConsumer : IConsumer<User>
	{
		public async Task Consume(ConsumeContext<User> context)
		{
			var user = context.Message;
			await Console.Out.WriteLineAsync($"User created: {user.Email}");
		}
	}
}
