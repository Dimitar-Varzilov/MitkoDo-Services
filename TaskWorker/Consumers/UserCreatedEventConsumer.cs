namespace TaskWorker.Consumers
{
	using AuthenticationAPI.Events;
	using MassTransit;
	using System.Threading.Tasks;
	using TasksAPI.Models;

	public class UserCreatedEventConsumer(TaskContext taskContext) :
		IConsumer<UserCreatedEvent>
	{
		private readonly TaskContext _taskContext = taskContext;
		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			Employee newEmployee = new()
			{
				EmployeeId = context.Message.UserId,
				Name = context.Message.Name,
			};
			_taskContext.Employees.Add(newEmployee);
			await _taskContext.SaveChangesAsync();
		}
	}
}