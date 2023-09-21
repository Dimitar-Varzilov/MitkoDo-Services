using Microsoft.AspNetCore.Mvc;
using Tasks.Models;

namespace Tasks.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TaskController : ControllerBase
	{

		[HttpGet]
		public OkObjectResult GetAllTask()
		{
			return Ok(Program.Tasks);
		}
	}
}
