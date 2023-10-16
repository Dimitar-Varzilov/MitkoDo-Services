namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class EmployeesRemovedEventConsumerDefinition :
		ConsumerDefinition<EmployeesRemovedEventConsumer>
	{
		public EmployeesRemovedEventConsumerDefinition()
		{
			ConcurrentMessageLimit = 1;
			EndpointName = "employee.employees-removed";
		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<EmployeesRemovedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
		}
	}
	public class EmployeesRemovedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<EmployeesRemovedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<EmployeesRemovedEvent> context)
		{
			try
			{
				EmployeesRemovedEvent message = context.Message;
				List<Employee> employees = [.. _employeeContext.Employees.Where(e => message.EmployeeIds.Contains(e.EmployeeId)).Include(employee => employee.ToDos.Where(toDo => toDo.ToDoId == message.ToDoId && toDo.Employees.Any(e => message.EmployeeIds.Contains(e.EmployeeId))))];
				if (employees.Count == 0) return;

				employees.ForEach(e => e.ToDos = []);

				await _employeeContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
                await Console.Out.WriteLineAsync(e.Message);
            }
		}
	}
}