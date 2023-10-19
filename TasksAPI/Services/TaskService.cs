using MassTransit;
using Microsoft.EntityFrameworkCore;
using TasksAPI.Data;
using TasksAPI.Dto;
using TasksAPI.Events;
using TasksAPI.Models;

namespace TasksAPI.Services
{
	public interface ITaskService
	{
		IList<ToDoDto> GetAllToDos();
		ToDoDto? GetToDoById(Guid toDoId);
		IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId);
		Task<ToDoDto> AddToDoAsync(CreateToDoDto createToDoDto);
		Task<int> EditToDoAsync(Guid toDoId, EditToDoDto task);
		Task<int> AssignEmployeesAsync(Guid toDoId, IList<Guid> employeeIds);
		Task<SubTaskDto?> AddSubTaskAsync(Guid subTaskId, CreateSubTaskDto subTask);
		Task<int> EditSubTaskAsync(Guid subTaskId, CreateSubTaskDto subTask);
		Task<int> RemoveEmployeeAsync(Guid toDoId, EmployeeIdsDto employeeId);
		Task<int> DeleteSubTaskAsync(Guid subTaskId);
		Task<int> DeleteToDoAsync(Guid taskId);
		Task<int> AddSubTaskImagesAndNoteAsync(Guid subTaskId, AddImagesAndNoteDto dto, Guid employeeId);
	}
	public class TaskService(TaskContext taskContext, IWebHostEnvironment hostingEnvironment, IBus bus) : ITaskService
	{
		private readonly TaskContext _taskContext = taskContext;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
		private readonly IBus _bus = bus;

		public IList<ToDoDto> GetAllToDos()
		{
			var toDos = _taskContext.ToDos
				.AsNoTracking()
				.Include(t => t.Employees)
				.Include(t => t.SubTasks)
					.ThenInclude(t => t.Pictures)
				.Include(t => t.SubTasks)
					.ThenInclude(t => t.Notes);

			return [.. toDos.Select(t => new ToDoDto(t))];
		}

		public ToDoDto? GetToDoById(Guid toDoId)
		{
			ToDo? toDo = _taskContext.ToDos
				.AsNoTracking()
				.Where(task => task.ToDoId == toDoId)
				.Include(t => t.Employees)
				.Include(t => t.SubTasks)
					.ThenInclude(t => t.Pictures)
				.Include(t => t.SubTasks)
					.ThenInclude(t => t.Notes)
				.FirstOrDefault();

			if (toDo == null) return null;

			return new ToDoDto(toDo);
		}

		public IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId)
		{
			IQueryable<ToDo> toDos = _taskContext.ToDos
				.AsNoTracking()
				.Where(t => t.Employees
				.Any(e => e.EmployeeId == employeeId))
				.Include(t => t.Employees)
				.Include(t => t.SubTasks)
					.ThenInclude(t => t.Pictures)
				.Include(t => t.SubTasks)
					.ThenInclude(t => t.Notes);

			if (!toDos.Any()) return [];

			return [.. toDos.Select(t => new GetAllTaskByEmployeeIdDto(t))];
		}


		public async Task<ToDoDto> AddToDoAsync(CreateToDoDto createToDoDto)
		{
			ToDo newToDo = new()
			{
				ToDoId = Guid.NewGuid(),
				Title = createToDoDto.Title,
				Description = createToDoDto.Description,
				StartDate = createToDoDto.StartDate,
				DueDate = createToDoDto.DueDate,
			};
			IList<Employee> employees =
				[.. _taskContext.Employees
				.Where(employee => createToDoDto.EmployeeIds
				.Contains(employee.EmployeeId))
				];
			newToDo.Employees = employees;

			_taskContext.Add(newToDo);
			await _taskContext.SaveChangesAsync();

			await _bus.Publish(new ToDoAddedEvent(newToDo));

			return new ToDoDto(newToDo);
		}

		public async Task<int> EditToDoAsync(Guid taskId, EditToDoDto editToDoDto)
		{
			try
			{
				ToDo? toDo = _taskContext.ToDos
					.FirstOrDefault(task => task.ToDoId == taskId);
				if (toDo == null) return StatusCodes.Status404NotFound;

				toDo.Title = editToDoDto.Title;
				toDo.Description = editToDoDto.Description;
				toDo.StartDate = editToDoDto.StartDate;
				toDo.DueDate = editToDoDto.DueDate;

				_taskContext.Update(toDo);
				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new ToDoEditedEvent(toDo));

				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}

		}

		public async Task<int> AssignEmployeesAsync(Guid toDoId, IList<Guid> employeeIds)
		{
			try
			{
				ToDo? todo = _taskContext.ToDos.FirstOrDefault(toDo => toDo.ToDoId == toDoId);
				if (todo == null) return StatusCodes.Status404NotFound;

				IList<Employee?> employeesToAssign = [.. _taskContext.Employees
					.Where(employee =>
						employeeIds.Any(id => id == employee.EmployeeId) && 
						!employee.ToDos.Any(t => t.ToDoId == toDoId))];
				if (employeesToAssign.Count == 0) return StatusCodes.Status404NotFound;

				todo.Employees =[.. todo.Employees.Union(employeesToAssign)];

				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new EmployeeAssignedEvent(toDoId, employeeIds));

				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<SubTaskDto?> AddSubTaskAsync(Guid toDoId, CreateSubTaskDto createSubTaskDto)
		{
			if (createSubTaskDto.NotesCountToBeCompleted == 0) return null;
			ToDo? todo = _taskContext.ToDos
				.FirstOrDefault(task => task.ToDoId == toDoId);
			if (todo == null) return null;
			if (!Utilities.IsTaskActive(todo)) return null;

			SubTask newSubTask = new()
			{
				SubTaskId = Guid.NewGuid(),
				Title = createSubTaskDto.Title,
				Description = createSubTaskDto.Description,
				PicturesCountToBeCompleted = createSubTaskDto.PicturesCountToBeCompleted,
				NotesCountToBeCompleted = createSubTaskDto.NotesCountToBeCompleted,
				Todo = todo
			};
			_taskContext.Add(newSubTask);

			await _bus.Publish(new SubTaskAddedEvent(newSubTask));

			await _taskContext.SaveChangesAsync();

			return new SubTaskDto(newSubTask);
		}

		public async Task<int> EditSubTaskAsync(Guid subTaskId, CreateSubTaskDto createSubTaskDto)
		{
			try
			{
				IList<ToDo> query = [.. _taskContext.ToDos
					.Where(toDo => 
						toDo.StartDate < DateTime.Now 
						&& toDo.DueDate > DateTime.Now 
						&& toDo.SubTasks.Any(subTask => subTask.SubTaskId == subTaskId))
					.Include(t => t.SubTasks.Where(s => s.SubTaskId == subTaskId))];
					
				if (query.Count == 0) return StatusCodes.Status400BadRequest;

				SubTask subTaskToEdit = query.First().SubTasks.First();
				_taskContext.Entry(subTaskToEdit).CurrentValues.SetValues(createSubTaskDto);
				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new SubTaskEditedEvent(subTaskToEdit));

				return StatusCodes.Status200OK;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return StatusCodes.Status500InternalServerError;
			}

		}

		public async Task<int> AddSubTaskImagesAndNoteAsync(Guid subTaskId, AddImagesAndNoteDto dto, Guid employeeId)
		{
			try
			{
				SubTask? subTask = _taskContext.SubTasks.FirstOrDefault(subTask => subTask.SubTaskId == subTaskId);
				if (subTask == null) return StatusCodes.Status400BadRequest;

				if (dto.Images.Count > 0)
				{
					string directory = Path.Combine(_hostingEnvironment.ContentRootPath, "Images");
					foreach (var image in dto.Images)
					{
						string filePath = Path.Combine(directory, image.FileName);
						FileStream fileStream = new(filePath, FileMode.Create);
						await image.CopyToAsync(fileStream);
						subTask.Pictures.Add(new Picture() { Path = filePath });
					}
				}

				subTask.Notes.Add(new() { Title = dto.Note });
				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new PictureAndNoteAddedEvent(subTask, dto, employeeId));

				return StatusCodes.Status201Created;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<int> RemoveEmployeeAsync(Guid toDoId, EmployeeIdsDto employeeIdsDto)
		{
			try
			{
				var toDos = _taskContext.ToDos.Where(task => task.ToDoId == toDoId).Include(todo => todo.Employees.Where(employee => employeeIdsDto.EmployeeIds.Any(id => employee.EmployeeId == id) && employee.ToDos.Any(t => t.ToDoId == toDoId))).ToList();
				if (toDos.Count == 0) return StatusCodes.Status400BadRequest;

				ToDo toDo = toDos.First();
				if (toDo.Employees.Count == 0) return StatusCodes.Status404NotFound;

				IList<Guid> excludedEmployeesIds = [.. toDo.Employees.Select(employee => employee.EmployeeId)];

				toDo.Employees = [];

				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new EmployeesRemovedEvent(toDo.ToDoId, excludedEmployeesIds));

				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<int> DeleteToDoAsync(Guid id)
		{
			try
			{
				ToDo? toDoToDelete = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == id);
				if (toDoToDelete == null) return StatusCodes.Status404NotFound;

				_taskContext.Remove(toDoToDelete);
				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new ToDoDeletedEvent(toDoToDelete.ToDoId));

				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<int> DeleteSubTaskAsync(Guid subTaskId)
		{
			try
			{
				SubTask? subTask = _taskContext.SubTasks.FirstOrDefault(subTask => subTask.SubTaskId == subTaskId);
				if (subTask == null) return StatusCodes.Status404NotFound;

				_taskContext.Remove(subTask);
				await _taskContext.SaveChangesAsync();

				await _bus.Publish(new SubTaskDeletedEvent(subTask.SubTaskId));

				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}
	}
}
