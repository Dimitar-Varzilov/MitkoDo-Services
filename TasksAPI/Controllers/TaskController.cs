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

		[HttpGet]
		public ActionResult<IList<ToDoDto>> GetAllToDos()
		{
			IList<ToDoDto> toDos = _taskService.GetAllToDos();
			return Ok(toDos);
		}

		[HttpGet("byEmployeeId/{employeeId:guid}")]
		public ActionResult<IList<GetAllTaskByEmployeeIdDto>> GetTasksByEmployeeId(Guid employeeId)
		{
			IList<GetAllTaskByEmployeeIdDto> toDos = _taskService.GetAllTasksAndSubTasksByEmployeeId(employeeId);
			return Ok(toDos);
		}

		[HttpPost]
		public async Task<ActionResult<ToDoDto>> AddToDo(CreateToDoDto dto)
		{
			ToDoDto newTodo = await _taskService.AddTask(dto);
			return Ok(newTodo);
		}


		[HttpPut("{toDoId:guid}")]
		public async Task<ActionResult<ToDo?>> EditToDo(Guid toDoId, CreateToDoDto createSubTaskDto)
		{
			ToDo? editedTodo = await _taskService.EditTask(toDoId, createSubTaskDto);
			return editedTodo == null ? NotFound("Task Not Found") : Ok(editedTodo);
		}

		[HttpPost("subtask/add/{taskId:guid}")]
		public async Task<ActionResult<CreateSubTaskDto?>> AddSubTask(Guid taskId, CreateSubTaskDto createSubTaskDto)
		{
			SubTaskDto? newSubTask = await _taskService.AddSubTask(taskId, createSubTaskDto);
			return newSubTask == null ? NotFound("Task Not Found") : Ok(newSubTask);
		}


		[HttpPut("subtask/edit/{subTaskId:guid}")]
		public async Task<ActionResult<SubTaskDto?>> EditSubTask(Guid subTaskId, CreateSubTaskDto createSubTaskDto)
		{
			SubTaskDto? editedSubTask = await _taskService.EditSubTask(subTaskId, createSubTaskDto);
			return editedSubTask == null ? NotFound("SubTask Not Found") : Ok(editedSubTask);
		}

		[HttpDelete("{toDoId:guid}")]
		public async Task<ActionResult<bool>> DeleteTask(Guid toDoId)
		{
			bool taskDeleted = await _taskService.DeleteTask(toDoId);
			return !taskDeleted ? NotFound() : Ok();
		}

		[HttpPost("subtask/addImage/{subTaskId:guid}")]
		public async Task<ActionResult<string>> AddImageAndNote(Guid subTaskId, AddImagesAndNoteDto addImagesAndNoteDto)
		{
			bool success = await _taskService.AddSubTaskImagesAndNote(subTaskId, addImagesAndNoteDto);
			return !success ? BadRequest("Something went wrong") : Ok($"Image{(addImagesAndNoteDto.Images.Count > 1 ? "s" : "")} and/or notes successfully uploaded");
		}
	}
}
