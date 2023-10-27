namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;


	public class EmployeeAssignedEventConsumerDefinition :
		ConsumerDefinition<EmployeeAssignedEventConsumer>
	{

		public EmployeeAssignedEventConsumerDefinition()
		{
			EndpointName = "employee.employee-assigned";
		}

		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EmployeeAssignedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}

	public class EmployeeAssignedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<EmployeeAssignedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<EmployeeAssignedEvent> context)
		{
			try
			{
				EmployeeAssignedEvent message = context.Message;
				if (message.ToDoId == Guid.Empty || message.EmployeeIds.Count == 0) return;

				List<Employee> employees = [.. _employeeContext.Employees.Where(e => message.EmployeeIds.Contains(e.EmployeeId))];
				ToDo toDo = _employeeContext.ToDos.FirstOrDefault(t => t.ToDoId == message.ToDoId);
				toDo.Employees = employees;

				_employeeContext.Update(toDo);
				var res = await _employeeContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				await Console.Out.WriteLineAsync(e.Message);
			}

		}
	}
}