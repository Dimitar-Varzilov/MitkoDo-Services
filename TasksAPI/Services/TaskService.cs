using TasksAPI.Data;
using TasksAPI.Dto;
using TasksAPI.Models;

namespace TasksAPI.Services
{
	public interface ITaskService
	{
		IList<ToDoDto> GetAllToDos();
		IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId);
		Task<ToDoDto> AddTask(CreateToDoDto task);
		Task<ToDo?> EditTask(Guid taskId, CreateToDoDto task);
		Task<SubTaskDto?> AddSubTask(Guid subTaskId, CreateSubTaskDto subTask);
		Task<SubTaskDto?> EditSubTask(Guid subTaskId, CreateSubTaskDto subTask);
		Task<bool> DeleteTask(Guid taskId);
		Task<bool> AddSubTaskImagesAndNote(Guid subTaskId, AddImagesAndNoteDto dto);
	}
	public class TaskService(TaskContext taskContext, IWebHostEnvironment hostingEnvironment) : ITaskService
	{
		private readonly TaskContext _taskContext = taskContext;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

		List<Picture> GetAllPicturesBySubTasksIds(IList<Guid> subTaskIds)
		{
			var pictures = _taskContext.Pictures.Where(p => subTaskIds.Any(s => s == p.SubTaskId));
			return [.. pictures];
		}

		List<Note> GetAllNotesBySubTasksIds(IList<Guid> subTaskIds)
		{
			var notes = _taskContext.Notes.Where(n => subTaskIds.Any(s => s == n.SubTaskId));
			return [.. notes];
		}

		List<SubTask> GetAllSubTaskByTasksIds(IList<Guid> taskIds)
		{
			var subtasks = _taskContext.SubTasks.Where(s => taskIds.Any(t => t == s.ToDoId));
			List<Guid> subTaskIds = [.. subtasks.Select(s => s.SubTaskId)];
			GetAllPicturesBySubTasksIds(subTaskIds);
			GetAllNotesBySubTasksIds(subTaskIds);
			return [.. subtasks];
		}

		List<SubTask> GetSubTaskByToDoId(Guid toDoId)
		{
			return [.. _taskContext.SubTasks.Where(s => s.ToDoId == toDoId)];
		}

		List<SubTask> GetAllSubtasks()
		{
			return [.. _taskContext.SubTasks];
		}

		List<Employee> GetAllEmployees()
		{
			return [.. _taskContext.Employees];
		}

		List<Employee> GetEmployeesByTaskId(Guid toDoId)
		{
			return [.. _taskContext.Employees.Where(e => e.ToDoId == toDoId)];
		}

		List<Employee> GetEmployeesByTasksIds(IList<Guid> toDoIds)
		{
			var search = _taskContext.Employees.Where(e => toDoIds.Any(todoId => todoId == e.ToDoId));
			return [.. search];
		}

		List<Picture> GetAllPictures()
		{
			return [.. _taskContext.Pictures];
		}

		List<Note> GetAllNotes()
		{
			return [.. _taskContext.Notes];
		}

		public IList<ToDoDto> GetAllToDos()
		{
			List<ToDo> toDos = [.. _taskContext.ToDos];
			IList<Guid> toDoIds = [.. toDos.Select(t => t.ToDoId)];
			GetAllSubTaskByTasksIds(toDoIds);
			GetEmployeesByTasksIds(toDoIds);

			IList<ToDoDto> toDoDto = [.. toDos.Select(t => new ToDoDto(t))];

			return toDoDto;
		}

		public IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId)
		{
			List<ToDo> toDos = [.. _taskContext.ToDos.Where(t => t.Employees.Any(e => e.EmployeeId == employeeId))];
			List<Guid> taskIds = [.. toDos.Select(t => t.ToDoId)];
			GetAllSubTaskByTasksIds(taskIds);

			IList<GetAllTaskByEmployeeIdDto> toDoDto = [.. toDos.Select(t => new GetAllTaskByEmployeeIdDto(t))];

			return toDoDto;
		}


		public async Task<ToDoDto> AddTask(CreateToDoDto createToDoDto)
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

			ToDoDto toDoDto = new(newToDo);
			return toDoDto;
		}

		public async Task<ToDo?> EditTask(Guid taskId, CreateToDoDto createToDoDto)
		{
			ToDo? todo = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == taskId);
			if (todo == null)
			{
				return null;
			}
			todo.Title = createToDoDto.Title;
			todo.Description = createToDoDto.Description;
			todo.StartDate = createToDoDto.StartDate;
			todo.DueDate = createToDoDto.DueDate;

			_taskContext.Update(todo);
			await _taskContext.SaveChangesAsync();
			return todo;
		}

		public async Task<SubTaskDto?> AddSubTask(Guid taskId, CreateSubTaskDto createSubTaskDto)
		{
			if (createSubTaskDto.NotesCountToBeCompleted == 0) return null;
			ToDo? todo = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == taskId);
			if (todo == null) return null;
			if (!Utilities.IsTaskActive(todo)) return null;
			Console.WriteLine(todo.SubTasks.Count);
			SubTask newSubTask = new()
			{
				Title = createSubTaskDto.Title,
				Description = createSubTaskDto.Description,
				PicturesCountToBeCompleted = createSubTaskDto.PicturesCountToBeCompleted,
				NotesCountToBeCompleted = createSubTaskDto.NotesCountToBeCompleted,
			};
			todo.SubTasks.Add(newSubTask);
			await _taskContext.SaveChangesAsync();
			SubTaskDto subTaskDto1 = new()
			{
				SubTaskId = newSubTask.SubTaskId,
				Title = newSubTask.Title,
				Description = newSubTask.Description,
				IsComplete = Utilities.CalculateSubTaskStatus(newSubTask)

			};

			return subTaskDto1;
		}

		public async Task<SubTaskDto?> EditSubTask(Guid subTaskId, CreateSubTaskDto createSubTaskDto)
		{
			SubTask? subTaskToEdit = _taskContext.SubTasks.FirstOrDefault(subTask => subTask.SubTaskId == subTaskId);
			if (subTaskToEdit == null)
			{
				return null;
			}
			_taskContext.Entry(subTaskToEdit).CurrentValues.SetValues(createSubTaskDto);
			await _taskContext.SaveChangesAsync();
			SubTaskDto subTaskDto = new()
			{
				SubTaskId = subTaskToEdit.SubTaskId,
				Title = createSubTaskDto.Title,
				Description = createSubTaskDto.Description,
				IsComplete = Utilities.CalculateSubTaskStatus(subTaskToEdit),
			};
			return subTaskDto;
		}

		public async Task<bool> AddSubTaskImagesAndNote(Guid subTaskId, AddImagesAndNoteDto dto)
		{
			try
			{
				SubTask? subTask = _taskContext.SubTasks.FirstOrDefault(subTask => subTask.SubTaskId == subTaskId);
				if (subTask == null) return false;

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
				return true;
			}
			catch (Exception)
			{
				return false;
			}

		}

		public async Task<bool> DeleteTask(Guid id)
		{
			ToDo? taskFound = _taskContext.ToDos.FirstOrDefault(task => task.ToDoId == id);
			if (taskFound == null)
			{
				return false;
			}
			_taskContext.Remove(taskFound);
			await _taskContext.SaveChangesAsync();
			return true;
		}
	}
}
