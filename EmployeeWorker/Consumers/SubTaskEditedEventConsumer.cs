namespace EmployeeWorker.Consumers
{
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using EmployeeAPI.Models;
	using MassTransit;
	using TasksAPI.Events;

	public class SubTaskEditedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<SubTaskEditedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<SubTaskEditedEvent> context)
		{
			try
			{
				SubTaskEditedEvent message = context.Message;
				SubTask subTask = _employeeContext.SubTasks.FirstOrDefault(s => s.SubTaskId == message.SubTaskId);
				if (subTask == null) return;

				subTask.Title = message.Title;
				await _employeeContext.SaveChangesAsync();
			}
			catch (Exception exception)
			{
				await Console.Out.WriteLineAsync(exception.Message);
			}

		}
	}
}