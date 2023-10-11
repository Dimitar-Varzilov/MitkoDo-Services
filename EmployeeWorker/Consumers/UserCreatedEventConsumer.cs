using System.Threading.Tasks;
using MassTransit;
using System;
using AuthenticationAPI.Events;
using EmployeeAPI.Models;


namespace EmployeeWorker.Consumers
{
	public class UserCreatedEventConsumer(EmployeeContext employeeContext) :
	IConsumer<UserCreatedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
			Console.WriteLine("Employee was created with ID: {0}", context.Message.UserId);
			Employee newEmployee = new()
			{
				EmployeeId = context.Message.UserId,
				Name = context.Message.Name,
			};
			_employeeContext.Employees.Add(newEmployee);
			await _employeeContext.SaveChangesAsync();
		}
	}
}