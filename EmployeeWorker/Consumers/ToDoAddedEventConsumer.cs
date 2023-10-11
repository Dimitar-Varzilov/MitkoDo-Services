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

	public class ToDoAddedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<ToDoAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<ToDoAddedEvent> context)
		{
			var employeeIds = context.Message.EmployeeIds;
			var employees = _employeeContext.Employees.Where(e => employeeIds.Contains(e.EmployeeId));
			ToDo newToDo = new()
			{
				ToDoId = context.Message.ToDoId,
				Title = context.Message.Title,
				StartDate = context.Message.StartDate,
				DueDate = context.Message.DueDate
			};
			await Console.Out.WriteLineAsync(employees.First().ToDos.Count.ToString());
			//foreach (var employee in employees)
			//{
			//	IList<ToDo> todos = [.. employee.ToDos];
			//	todos.Add(newToDo);
			//	employee.ToDos = todos;

			//}
			employees.ToList().ForEach(e => e.ToDos.Add(newToDo));
			//_employeeContext.UpdateRange(employees);
			await Console.Out.WriteLineAsync(employees.First().ToDos.Count.ToString());
			await _employeeContext.SaveChangesAsync();
		}
	}
}