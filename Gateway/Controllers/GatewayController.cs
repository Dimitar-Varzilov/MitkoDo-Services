using Gateway.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GatewayController(IBus bus) : ControllerBase
	{
		private readonly IBus _bus = bus;

		[HttpPost]
		public async Task<IActionResult> Get([FromBody] Guid userId)
		{
			Uri uri = new("rabbitmq://localhost/employee-worker");
			await _bus.GetSendEndpoint(uri).Result.Send(new NewEmployee
			{
				UserId = userId,
			});
			return Ok();
		}
	}
}
