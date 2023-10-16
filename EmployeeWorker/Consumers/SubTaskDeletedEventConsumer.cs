namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class SubTaskDeletedEventConsumerDefinition :
		ConsumerDefinition<SubTaskDeletedEventConsumer>
	{
		public SubTaskDeletedEventConsumerDefinition()
		{
			EndpointName = "employee.subtask-deleted";
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<SubTaskDeletedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
	public class SubTaskDeletedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<SubTaskDeletedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<SubTaskDeletedEvent> context)
		{
			try
			{
				SubTaskDeletedEvent message = context.Message;
				SubTask subTask = _employeeContext.SubTasks.FirstOrDefault(subTask => subTask.SubTaskId == message.SubTaskId);
				if (subTask == null) return;

				_employeeContext.Remove(subTask);
				await _employeeContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				await Console.Out.WriteLineAsync(e.Message);
			}
		}
	}
}