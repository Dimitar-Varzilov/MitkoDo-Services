using AuthenticationAPI.Events;
using EmployeeAPI.Models;
using MassTransit;
using System.Threading.Tasks;


namespace EmployeeWorker.Consumers
{
	public class UserCreatedEventConsumer(EmployeeContext employeeContext) :
	IConsumer<UserCreatedEvent>
	{
		private readonly EmployeeContext _employeeContext = employeeContext;
		public async Task Consume(ConsumeContext<UserCreatedEvent> context)
		{
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