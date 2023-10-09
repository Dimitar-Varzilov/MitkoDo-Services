using System.Threading.Tasks;
using MassTransit;
using System;
using Gateway.Events;
using EmployeeWorker.Models;


namespace EmployeeWorker.Consumers
{
	public class UserCreatedEventConsumer(EmployeeContext employeeContext) :
	IConsumer<UserCreatedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{

			Console.WriteLine("Employee was created with ID: {0}", context.Message.UserId);
			_employeeContext.Employees.Add(new Employee
			{
				UserId = context.Message.UserId,
				Name = "Mitko"
			});
			await _employeeContext.SaveChangesAsync();
		}
	}
}