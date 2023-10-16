namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class ToDoAddedEventConsumerDefinition :
		ConsumerDefinition<ToDoAddedEventConsumer>
	{
		public ToDoAddedEventConsumerDefinition()
		{
			EndpointName = "employee.todo-added";
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<ToDoAddedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}

	public class ToDoAddedEventConsumer(EmployeeContext employeeContext) :
	IConsumer<ToDoAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<ToDoAddedEvent> context)
		{

			ToDoAddedEvent message = context.Message;
			List<Employee> employees = [.. _employeeContext.Employees.Where(e => message.EmployeeIds.Contains(e.EmployeeId))];
			ToDo newToDo = new()
			{
				ToDoId = message.ToDoId,
				Title = message.Title,
				StartDate = message.StartDate,
				DueDate = message.DueDate,
				Employees = employees

			};
			await _employeeContext.AddAsync(newToDo);
			await _employeeContext.SaveChangesAsync();
		}
	}
}