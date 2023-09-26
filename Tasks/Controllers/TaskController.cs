using Microsoft.AspNetCore.Mvc;
using Tasks.Dto;
using Tasks.Models;
using Tasks.Services;

namespace Tasks.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TaskController(ITaskService taskService) : ControllerBase
	{
		private readonly ITaskService _taskService = taskService;

		[HttpGet]
		public OkObjectResult GetAllTask()
		{
			return Ok("Got all users");
		}

		[HttpGet("{id}")]
		public OkObjectResult GetTaskById(int id)
		{
			return Ok($"Got user with id {id}");
		}

		[HttpPost]
		public Task<int> AddTask(CustomTaskDto task)
		{
			return _taskService.AddTask(task);
		}
	}
}
