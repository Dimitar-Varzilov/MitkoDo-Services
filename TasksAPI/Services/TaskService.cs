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
		IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId);
		Task<ToDoDto> AddToDo(CreateToDoDto createToDoDto);
		Task<int> EditToDo(Guid toDoId, CreateToDoDto task);
		Task<int> AssignEmployees(Guid toDoId, IList<Guid> employeeIds);
		Task<SubTaskDto?> AddSubTask(Guid subTaskId, CreateSubTaskDto subTask);
		Task<int> EditSubTask(Guid subTaskId, CreateSubTaskDto subTask);
		Task<int> RemoveEmployee(Guid toDoId, EmployeeIdDto employeeId);
		Task<int> DeleteSubTask(Guid subTaskId);
		Task<int> DeleteTask(Guid taskId);
		Task<int> AddSubTaskImagesAndNote(Guid subTaskId, AddImagesAndNoteDto dto);
	}
	public class TaskService(TaskContext taskContext, IWebHostEnvironment hostingEnvironment, IBus bus) : ITaskService
	{
		private readonly TaskContext _taskContext = taskContext;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;
		private readonly IBus _bus = bus;

		public IList<ToDoDto> GetAllToDos()
		{
			var toDos = _taskContext.ToDos.Include(t => t.Employees).Include(t => t.SubTasks).ThenInclude(t => t.Pictures).Include(t => t.SubTasks).ThenInclude(t => t.Notes);

			return [.. toDos.Select(t => new ToDoDto(t))];
		}

		public IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId)
		{
			var toDos = _taskContext.ToDos.Where(t => t.Employees.Any(e => e.EmployeeId == employeeId)).Include(t => t.SubTasks).ThenInclude(t => t.Pictures).Include(t => t.SubTasks).ThenInclude(t => t.Notes);

			return [.. toDos.Select(t => new GetAllTaskByEmployeeIdDto(t))];
		}


		public async Task<ToDoDto> AddToDo(CreateToDoDto createToDoDto)
		{
			ToDo newToDo = new()
			{
				ToDoId = Guid.NewGuid(),
				Title = createToDoDto.Title,
				Description = createToDoDto.Description,
				StartDate = createToDoDto.StartDate,
				DueDate = createToDoDto.DueDate,
			};
			IList<Employee> employees = [.. _taskContext.Employees.Where(employee => createToDoDto.EmployeeIds.Contains(employee.EmployeeId))];
			newToDo.Employees = employees;

			_taskContext.Add(newToDo);
			await _taskContext.SaveChangesAsync();

			ToDoAddedEvent toDoAddedEvent = new()
			{
				ToDoId = newToDo.ToDoId,
				Title = newToDo.Title,
				StartDate = newToDo.StartDate,
				DueDate = newToDo.DueDate,
				EmployeeIds = newToDo.Employees.Select(employee => employee.EmployeeId).ToList()
			};

			await _bus.Publish(toDoAddedEvent);

			return new ToDoDto(newToDo);
		}

		public async Task<int> EditToDo(Guid taskId, CreateToDoDto createToDoDto)
		{
			try
			{
				ToDo? todo = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == taskId);
				if (todo == null) return StatusCodes.Status404NotFound;

				_taskContext.Entry(todo).CurrentValues.SetValues(createToDoDto);

				_taskContext.Update(todo);
				await _taskContext.SaveChangesAsync();
				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}

		}

		public async Task<int> AssignEmployees(Guid toDoId, IList<Guid> employeeIds)
		{
			try
			{
				ToDo? todo = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == toDoId);
				if (todo == null) return StatusCodes.Status404NotFound;

				IList<Employee?> employee = [.. _taskContext.Employees.Where(employee => employeeIds.Any(id => id == employee.EmployeeId))];
				if (employee == null) return StatusCodes.Status404NotFound;

				var combinedEmployees = todo.Employees.Union(employee);
				todo.Employees = [.. combinedEmployees];
				_taskContext.Update(todo);
				await _taskContext.SaveChangesAsync();
				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<SubTaskDto?> AddSubTask(Guid taskId, CreateSubTaskDto createSubTaskDto)
		{
			if (createSubTaskDto.NotesCountToBeCompleted == 0) return null;
			ToDo? todo = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == taskId);
			if (todo == null) return null;
			if (!Utilities.IsTaskActive(todo)) return null;

			SubTask newSubTask = new()
			{
				Title = createSubTaskDto.Title,
				Description = createSubTaskDto.Description,
				PicturesCountToBeCompleted = createSubTaskDto.PicturesCountToBeCompleted,
				NotesCountToBeCompleted = createSubTaskDto.NotesCountToBeCompleted,
			};
			todo.SubTasks.Add(newSubTask);
			await _taskContext.SaveChangesAsync();
			SubTaskDto subTaskDto1 = new(newSubTask);

			return subTaskDto1;
		}

		public async Task<int> EditSubTask(Guid subTaskId, CreateSubTaskDto createSubTaskDto)
		{
			try
			{
				var query = _taskContext.ToDos.Where(toDo => toDo.StartDate < DateTime.Now && toDo.DueDate > DateTime.Now && toDo.SubTasks.Any(subTask => subTask.SubTaskId == subTaskId)).ToList();
				if (query.Count == 0) return StatusCodes.Status400BadRequest;

				SubTask subtaskToEdit = query.First().SubTasks.First();
				_taskContext.Entry(subtaskToEdit).CurrentValues.SetValues(createSubTaskDto);
				await _taskContext.SaveChangesAsync();

				return StatusCodes.Status200OK;
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return StatusCodes.Status500InternalServerError;
			}

		}

		public async Task<int> AddSubTaskImagesAndNote(Guid subTaskId, AddImagesAndNoteDto dto)
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

				subTask.Notes.Add(new Note() { Title = dto.Note });
				await _taskContext.SaveChangesAsync();
				return StatusCodes.Status201Created;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}

		}

		public async Task<int> RemoveEmployee(Guid toDoId, EmployeeIdDto dto)
		{
			try
			{
				var toDos = _taskContext.ToDos.Where(task => task.ToDoId == toDoId).Include(todo => todo.Employees.Where(employee => employee.EmployeeId == dto.EmployeeId)).ToList();
				if (toDos == null) return StatusCodes.Status400BadRequest;

				ToDo toDo = toDos.First();
				if (toDo.Employees.Count == 0) return StatusCodes.Status404NotFound;
				toDo.Employees.Remove(toDo.Employees.First());
				await _taskContext.SaveChangesAsync();
				return StatusCodes.Status200OK;
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<int> DeleteTask(Guid id)
		{
			try
			{
				ToDo? taskFound = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == id);
				if (taskFound == null) return StatusCodes.Status404NotFound;

				_taskContext.Remove(taskFound);
				await _taskContext.SaveChangesAsync();
				return StatusCodes.Status200OK;

			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				return StatusCodes.Status500InternalServerError;
			}
		}

		public async Task<int> DeleteSubTask(Guid subTaskId)
		{
			try
			{
				SubTask? subTask = _taskContext.SubTasks.FirstOrDefault(subTask => subTask.SubTaskId == subTaskId);
				if (subTask == null) return StatusCodes.Status404NotFound;

				_taskContext.Remove(subTask);
				await _taskContext.SaveChangesAsync();
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
