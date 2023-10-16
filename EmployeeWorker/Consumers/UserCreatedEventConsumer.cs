using AuthenticationAPI.Events;
using EmployeeAPI.Models;
using MassTransit;
using System.Threading.Tasks;


namespace EmployeeWorker.Consumers
{

	public class UserCreatedEventConsumerDefinition :
		ConsumerDefinition<UserCreatedEventConsumer>
	{
		public UserCreatedEventConsumerDefinition()
		{
			EndpointName = "employee.user-created";
		}
		protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<UserCreatedEventConsumer> consumerConfigurator, IRegistrationContext context)
		{
			endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
			endpointConfigurator.UseCircuitBreaker();
		}
	}
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