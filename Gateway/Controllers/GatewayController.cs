using Gateway.EventBusConfig;
using Gateway.Events;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GatewayController(IEventBus eventBus) : ControllerBase
	{
		private readonly IEventBus _eventBus = eventBus;

		[HttpPost]
		public IActionResult Get([FromBody] Guid userId)
		{
			_eventBus.PublishAsync(new UserCreatedEvent(userId));
			return Ok();
		}
	}
}
