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

		[HttpGet("byEmployeeId/{employeeId:guid}")]
		public ActionResult<IList<GetAllTaskByEmployeeIdDto>> GetTasksByEmployeeId(Guid employeeId)
		{
			IList<GetAllTaskByEmployeeIdDto> tasks = _taskService.GetAllTasksAndSubTasksByEmployeeId(employeeId);
			return Ok(tasks);
		}

		[HttpPost]
		public async Task<ActionResult<ToDoDto>> AddTask(CreateToDoDto task)
		{
			ToDoDto newTask = await _taskService.AddTask(task);
			return Ok(newTask);
		}


		[HttpPut("{id}")]
		public async Task<ActionResult<ToDo?>> EditTask(Guid id, CreateToDoDto task)
		{
			ToDo? customTask = await _taskService.EditTask(id, task);
			return customTask == null ? NotFound("Task Not Found") : Ok(customTask);
		}

		[HttpPost("subtask/add/{taskId}")]
		public async Task<ActionResult<CreateSubTaskDto?>> AddSubTask(Guid taskId, CreateSubTaskDto subTask)
		{
			SubTaskDto? newSubTask = await _taskService.AddSubTask(taskId, subTask);
			return newSubTask == null ? NotFound("Task Not Found") : Ok(newSubTask);
		}


		[HttpPut("subtask/edit/{subTaskId}")]
		public async Task<ActionResult<SubTaskDto?>> EditSubTask(Guid subTaskId, CreateSubTaskDto subTask)
		{
			SubTaskDto? editedSubTask = await _taskService.EditSubTask(subTaskId, subTask);
			return editedSubTask == null ? NotFound("SubTask Not Found") : Ok(editedSubTask);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> DeleteTask(Guid id)
		{
			bool taskDeleted = await _taskService.DeleteTask(id);
			return !taskDeleted ? NotFound() : Ok();
		}

		[HttpPost("subtask/addImage/{subTaskId}")]
		public async Task<ActionResult<string>> AddImage(Guid subTaskId, List<IFormFile> images, string note)
		{
            await Console.Out.WriteLineAsync(note);
            if (images.Count == 0) return BadRequest("No Image Found");
			await _taskService.AddSubTaskImage(subTaskId, images);
			return Ok($"Image{(images.Count > 1 ? "s" : "")} successfully uploaded");
		}
	}
}
