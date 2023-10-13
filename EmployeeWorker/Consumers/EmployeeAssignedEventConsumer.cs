namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class EmployeeAssignedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<EmployeeAssignedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<EmployeeAssignedEvent> context)
		{
			try
			{
				EmployeeAssignedEvent message = context.Message;
				List<Employee> employees = [.. _employeeContext.Employees.Where(e => message.EmployeeIds.Contains(e.EmployeeId))];
				ToDo toDo = _employeeContext.ToDos.FirstOrDefault(t => t.ToDoId == message.ToDoId);

				employees.ForEach(e => e.ToDos.Add(toDo));
			 await	_employeeContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				await Console.Out.WriteLineAsync(e.Message);
			}

		}
	}
}