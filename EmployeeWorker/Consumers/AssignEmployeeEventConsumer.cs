namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class AssignEmployeeEventConsumer(EmployeeContext employeeContext) :
		IConsumer<AssignEmployeeEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<AssignEmployeeEvent> context)
		{
			try
			{
				AssignEmployeeEvent message = context.Message;
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