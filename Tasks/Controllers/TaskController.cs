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

		[HttpPost]
		public async Task<IActionResult> AddTask(CustomTaskDto task)
		{
			CustomTask newTask = await _taskService.AddTask(task);
			return Ok(newTask);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> EditTask(int id, CustomTaskDto task)
		{
			CustomTask? customTask = await _taskService.EditTask(id, task);
			return customTask == null ? NotFound("Task Not Found") : Ok(customTask);
		}

		[HttpPost("subtask/add/{taskId}")]
		public async Task<ActionResult<SubTaskDto?>> AddSubTask(int taskId, SubTaskDto subTask)
		{
			SubTask? newSubTask = await _taskService.AddSubTask(taskId, subTask);
			return newSubTask == null ? NotFound("Task Not Found") : Ok(newSubTask);
		}


		[HttpPut("subtask/edit/{subTaskId}")]
		public async Task<ActionResult<SubTaskDto?>> EditSubTask(int subTaskId, SubTaskDto subTask)
		{
			SubTask? editedSubTask = await _taskService.EditSubTask(subTaskId, subTask);
			return editedSubTask == null ? NotFound("SubTask Not Found") : Ok(editedSubTask);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTask(int id)
		{
			bool taskDeleted = await _taskService.DeleteTask(id);
			return taskDeleted ? Ok() : NotFound();
		}
	}
}
