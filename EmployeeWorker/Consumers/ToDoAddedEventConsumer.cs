namespace EmployeeWorker.Consumers
{
	using EmployeeAPI.Models;
	using MassTransit;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using TasksAPI.Events;

	public class ToDoAddedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<ToDoAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public Task Consume(ConsumeContext<ToDoAddedEvent> context)
		{

			ToDoAddedEvent message = context.Message;
			List<Employee> employees = [.. _employeeContext.Employees.Where(e => message.EmployeeIds.Contains(e.EmployeeId))];
			ToDo newToDo = new()
			{
				ToDoId = message.ToDoId,
				Title = message.Title,
				StartDate = message.StartDate,
				DueDate = message.DueDate
			};
			_employeeContext.ToDos.Add(newToDo);
			employees.ForEach(e => e.ToDos.Add(newToDo));
			_employeeContext.SaveChanges();
			return Task.CompletedTask;
		}
	}
}