namespace EmployeeWorker.Consumers
{
	using System.Threading.Tasks;
	using MassTransit;
	using System;
	using EmployeeWorker.Contracts;

	public class EmployeeWorkerConsumer :
		IConsumer<NewUser>
	{
		public Task Consume(ConsumeContext<NewUser> context)
		{
			
			Console.WriteLine(context.Message.UserId);
			return Task.CompletedTask;
		}
	}
}