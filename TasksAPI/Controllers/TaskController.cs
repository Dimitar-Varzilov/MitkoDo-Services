using Microsoft.AspNetCore.Mvc;
using TasksAPI.Dto;
using TasksAPI.Models;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TaskController(ITaskService taskService) : ControllerBase
	{
		private readonly ITaskService _taskService = taskService;

		[HttpPost]
		public async Task<IActionResult> AddTask(CreateToDoDto task)
		{
			ToDo newTask = await _taskService.AddTask(task);
			return Ok(newTask);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> EditTask(Guid id, CreateToDoDto task)
		{
			ToDo? customTask = await _taskService.EditTask(id, task);
			return customTask == null ? NotFound("Task Not Found") : Ok(customTask);
		}

		[HttpPost("subtask/add/{taskId}")]
		public async Task<ActionResult<SubTaskDto?>> AddSubTask(Guid taskId, SubTaskDto subTask)
		{
			SubTaskDto? newSubTask = await _taskService.AddSubTask(taskId, subTask);
			return newSubTask == null ? NotFound("Task Not Found") : Ok(newSubTask);
		}


		[HttpPut("subtask/edit/{subTaskId}")]
		public async Task<ActionResult<SubTaskDto?>> EditSubTask(Guid subTaskId, SubTaskDto subTask)
		{
			SubTaskDto? editedSubTask = await _taskService.EditSubTask(subTaskId, subTask);
			return editedSubTask == null ? NotFound("SubTask Not Found") : Ok(editedSubTask);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTask(Guid id)
		{
			bool taskDeleted = await _taskService.DeleteTask(id);
			return !taskDeleted ? NotFound() : Ok();
		}

		[HttpPost("subtask/addImage/{subTaskId}")]
		public async Task<IActionResult> AddImage(Guid subTaskId, List<IFormFile> images)
		{
			if (images.Count == 0) return BadRequest("No Image Found");
			await _taskService.AddSubTaskImage(subTaskId, images);
			return Ok($"Image{(images.Count > 1 ? "s" : "")} successfully uploaded");
		}
	}
}
