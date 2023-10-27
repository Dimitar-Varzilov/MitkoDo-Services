using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasksAPI.Authorization;
using TasksAPI.Dto;
using TasksAPI.Services;

namespace TasksAPI.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class TaskController(ITaskService taskService) : ControllerBase
	{
		private readonly ITaskService _taskService = taskService;

		[HttpGet]
		[Authorize(Roles = UserRole.MANAGER)]
		public ActionResult<IList<ToDoDto>> GetAllToDos()
		{
			IList<ToDoDto> toDos = _taskService.GetAllToDos();
			return Ok(toDos);
		}

		[HttpGet("{toDoId:guid}")]
		[Authorize(Roles = UserRole.MEMBER)]
		public ActionResult<ToDoDto> GetToDoById(Guid toDoId)
		{
			ToDoDto? toDo = _taskService.GetToDoById(toDoId);
			if (toDo == null) return NotFound("Task Not Found");
			return Ok(toDo);
		}

		[HttpGet("byEmployeeId/{employeeId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public ActionResult<IList<GetAllTaskByEmployeeIdDto>> GetTasksByEmployeeId(Guid employeeId)
		{
			IList<GetAllTaskByEmployeeIdDto> toDos = _taskService.GetAllTasksAndSubTasksByEmployeeId(employeeId);
			return Ok(toDos);
		}

		[HttpGet("byToken")]
		[Authorize(Roles = UserRole.MEMBER)]
		public ActionResult<IList<GetAllTaskByEmployeeIdDto>> GetTasksFromToken()
		{
			Guid? employeeId = Utilities.GetEmployeeId(User);
			if (employeeId == null) return Unauthorized();

			IList<GetAllTaskByEmployeeIdDto> toDos = _taskService.GetAllTasksAndSubTasksByEmployeeId((Guid)employeeId);
			return Ok(toDos);
		}

		[HttpPost]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<ToDoDto>> AddToDo(CreateToDoDto dto)
		{
			ToDoDto newTodo = await _taskService.AddToDoAsync(dto);
			return Ok(newTodo);
		}


		[HttpPut("{toDoId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<int>> EditToDo(Guid toDoId, EditToDoDto editToDoDto)
		{
			int response = await _taskService.EditToDoAsync(toDoId, editToDoDto);
			return StatusCode(response);
		}

		[HttpPost("assignEmployee/{toDoId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<int>> AssignEmployee(Guid toDoId, IList<Guid> employeeIds)
		{
			int response = await _taskService.AssignEmployeesAsync(toDoId, employeeIds);
			return StatusCode(response);
		}

		[HttpPost("subtask/add/{toDoId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<SubTaskDto?>> AddSubTask(Guid toDoId, CreateSubTaskDto createSubTaskDto)
		{
			SubTaskDto? newSubTask = await _taskService.AddSubTaskAsync(toDoId, createSubTaskDto);
			return newSubTask == null ? NotFound("Task Not Found") : Ok(newSubTask);
		}

		[HttpPut("subtask/edit/{subTaskId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<int>> EditSubTask(Guid subTaskId, CreateSubTaskDto createSubTaskDto)
		{
			int response = await _taskService.EditSubTaskAsync(subTaskId, createSubTaskDto);
			return StatusCode(response);
		}

		[HttpPost("removeEmployee/{toDoId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<int>> RemoveEmployee(Guid toDoId, [FromBody] EmployeeIdsDto employeeIdDto)
		{
			int response = await _taskService.RemoveEmployeeAsync(toDoId, employeeIdDto);
			return StatusCode(response);
		}

		[HttpDelete("subtask/delete/{subTaskId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<int>> DeleteSubTask(Guid subTaskId)
		{
			int response = await _taskService.DeleteSubTaskAsync(subTaskId);
			return StatusCode(response);
		}

		[HttpDelete("{toDoId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public async Task<ActionResult<int>> DeleteTask(Guid toDoId)
		{
			int response = await _taskService.DeleteToDoAsync(toDoId);
			return StatusCode(response);
		}

		[HttpPost("subtask/addImage/{subTaskId:guid}")]
		[Authorize(Roles = UserRole.MEMBER)]
		public async Task<ActionResult<int>> AddImageAndNote(Guid subTaskId, AddImagesAndNoteDto addImagesAndNoteDto)
		{
			Guid? employeeId = Utilities.GetEmployeeId(User);
			if (employeeId == null) return Unauthorized();
			int response = await _taskService.AddSubTaskImagesAndNoteAsync(subTaskId, addImagesAndNoteDto, (Guid)employeeId);
			return StatusCode(response);
		}
	}
}
