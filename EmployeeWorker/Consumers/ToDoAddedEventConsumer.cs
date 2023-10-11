namespace EmployeeWorker.Consumers
{
	using EmployeeWorker.Contracts;
	using EmployeeWorker.Events;
	using MassTransit;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	public class ToDoAddedEventConsumer(EmployeeContext employeeContext) :
		IConsumer<ToDoAddedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public Task Consume(ConsumeContext<ToDoAddedEvent> context)
		{
			IList<Guid> employeeIds = context.Message.EmployeeIds;
			var employees = _employeeContext.Employees.Where(e => employeeIds.Contains(e.EmployeeId));
			employees.ToList().ForEach(e => e.ToDos.Add(new ToDo()
			{
				ToDoId = context.Message.ToDoId,
				Title = context.Message.Title,
				StartDate = context.Message.StartDate,
				DueDate = context.Message.DueDate
			}
			));
			_employeeContext.SaveChanges();
			return Task.CompletedTask;
		}
	}
}