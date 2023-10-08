using Gateway.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController(IBus bus) : ControllerBase
	{
		private readonly IBus _bus = bus;

		[HttpGet(Name = "GetWeatherForecast")]
		public IActionResult Get()
		{
			_bus.Publish(new NewUser
			{
				UserId = "Some Text"
			});
			return Ok();
		}
	}
}
