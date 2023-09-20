using Microsoft.AspNetCore.Mvc;

namespace Tasks.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TaskController : ControllerBase
	{

		[HttpGet]
		public OkObjectResult Get()
		{
			return Ok("test ok");
		}
	}
}
