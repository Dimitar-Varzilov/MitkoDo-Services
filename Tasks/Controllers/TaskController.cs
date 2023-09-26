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

		[HttpGet("{id}")]
		public IActionResult GetTaskById(int id)
		{
			CustomTask? foundTask = _taskService.GetTaskById(id);
			return foundTask == null ? NotFound("Task Not Found") : Ok(foundTask);
		}

		[HttpPost("addTask")]
		public async Task<IActionResult> AddTask(CustomTaskDto task)
		{
			CustomTask newTask = await _taskService.AddTask(task);
			return Ok(newTask);
		}

		[HttpPost("addSubtask/{taskId}")]
		public async Task<ActionResult<SubTask?>> AddSubTask(int taskId, AddSubTaskDto subTask)
		{
			SubTask? newTask = await _taskService.AddSubTask(taskId, subTask);
			return newTask == null ? NotFound("Task Not Found") : Ok(newTask);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditTask(int id, CustomTask task)
		{
			CustomTask? customTask = await _taskService.EditTask(id, task);
			return customTask == null ? NotFound("Task Not Found") : Ok(customTask);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTask(int id)
		{
			bool taskDeleted = await _taskService.DeleteTask(id);
			return taskDeleted ? Ok() : NotFound();
		}
	}
}
