using Microsoft.AspNetCore.Mvc;
using TasksAPI.Dto;
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
			ToDoDto newTodo = await _taskService.AddToDo(dto);
			return Ok(newTodo);
		}


		[HttpPut("{toDoId:guid}")]
		public async Task<ActionResult<int>> EditToDo(Guid toDoId, CreateToDoDto createSubTaskDto)
		{
			int response = await _taskService.EditToDo(toDoId, createSubTaskDto);
			return StatusCode(response);
		}

		[HttpPost("assignEmployee/{toDoId:guid}")]
		public async Task<ActionResult<int>> AssignEmployee(Guid toDoId, IList<Guid> employeeIds)
		{
			int response = await _taskService.AssignEmployees(toDoId, employeeIds);
			return StatusCode(response);
		}

		[HttpPost("subtask/add/{toDoId:guid}")]
		public async Task<ActionResult<SubTaskDto?>> AddSubTask(Guid toDoId, CreateSubTaskDto createSubTaskDto)
		{
			SubTaskDto? newSubTask = await _taskService.AddSubTask(toDoId, createSubTaskDto);
			return newSubTask == null ? NotFound("Task Not Found") : Ok(newSubTask);
		}

		[HttpPut("subtask/edit/{subTaskId:guid}")]
		public async Task<ActionResult<int>> EditSubTask(Guid subTaskId, CreateSubTaskDto createSubTaskDto)
		{
			int response = await _taskService.EditSubTask(subTaskId, createSubTaskDto);
			return StatusCode(response);
		}

		[HttpPost("removeEmployee/{toDoId:guid}")]
		public async Task<ActionResult<int>> RemoveEmployee(Guid toDoId, EmployeeIdsDto employeeIdDto)
		{
			int response = await _taskService.RemoveEmployee(toDoId, employeeIdDto);
			return StatusCode(response);
		}

		[HttpDelete("subtask/delete/{subTaskId:guid}")]
		public async Task<ActionResult<int>> DeleteSubTask(Guid subTaskId)
		{
			int response = await _taskService.DeleteSubTask(subTaskId);
			return StatusCode(response);
		}

		[HttpDelete("{toDoId:guid}")]
		public async Task<ActionResult<int>> DeleteTask(Guid toDoId)
		{
			int response = await _taskService.DeleteToDo(toDoId);
			return StatusCode(response);
		}

		[HttpPost("subtask/addImage/{subTaskId:guid}")]
		public async Task<ActionResult<int>> AddImageAndNote(Guid subTaskId, AddImagesAndNoteDto addImagesAndNoteDto)
		{
			int response = await _taskService.AddSubTaskImagesAndNote(subTaskId, addImagesAndNoteDto);
			return StatusCode(response);
		}
	}
}
