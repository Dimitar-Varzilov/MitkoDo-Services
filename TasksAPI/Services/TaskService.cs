using TasksAPI.Data;
using TasksAPI.Dto;
using TasksAPI.Models;

namespace TasksAPI.Services
{

	public interface ITaskService
	{
		IList<SubTask> GetAllSubTaskByTasksIds(IList<Guid> taskIds);
		IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId);
		Task<ToDoDto> AddTask(CreateToDoDto task);
		Task<ToDo?> EditTask(Guid taskId, CreateToDoDto task);
		Task<SubTaskDto?> AddSubTask(Guid subTaskId, CreateSubTaskDto subTask);
		Task<SubTaskDto?> EditSubTask(Guid subTaskId, CreateSubTaskDto subTask);
		Task<bool> DeleteTask(Guid taskId);
		Task<bool> AddSubTaskImage(Guid subTaskId, List<IFormFile> images);
	}
	public class TaskService(TaskContext taskContext, IWebHostEnvironment hostingEnvironment) : ITaskService
	{
		private readonly TaskContext _taskContext = taskContext;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

		public IList<SubTask> GetAllSubTaskByTasksIds(IList<Guid> taskIds)
		{
			var subtasks = _taskContext.SubTasks.Where(s => taskIds.Any(t => t == s.ToDoId)).ToList();
			return subtasks;
		}

		public IList<GetAllTaskByEmployeeIdDto> GetAllTasksAndSubTasksByEmployeeId(Guid employeeId)
		{
			var toDos = _taskContext.Tasks.Where(t => t.Employees.Any(e => e.EmployeeId == employeeId));
			var taskIds = toDos.Select(t => t.ToDoId).ToList();
			var subtasks = GetAllSubTaskByTasksIds(taskIds);

			IList<GetAllTaskByEmployeeIdDto> toDoDto = toDos.Select(t => new GetAllTaskByEmployeeIdDto(t)).ToList();

			return toDoDto;
		}


		public async Task<ToDoDto> AddTask(CreateToDoDto createToDoDto)
		{
			ToDo toDo = new()
			{
				ToDoId = Guid.NewGuid(),
				Title = createToDoDto.Title,
				Description = createToDoDto.Description,
				StartDate = createToDoDto.StartDate,
				DueDate = createToDoDto.DueDate,
			};
			IList<Employee> employees = [.. _taskContext.Employees.Where(employee => createToDoDto.EmployeeIds.Contains(employee.EmployeeId))];
			toDo.Employees = employees;

			_taskContext.Add(toDo);
			await _taskContext.SaveChangesAsync();

			ToDoDto toDoDto = new()
			{
				TodoId = toDo.ToDoId,
				Title = toDo.Title,
				Description = toDo.Description,
				StartDate = toDo.StartDate,
				DueDate = toDo.DueDate,
				Status = Utilities.CalculateTaskStatus(toDo),
				SubTasks = [.. toDo.SubTasks.Select(subTask => new SubTaskDto()
				{
					SubTaskId = subTask.SubTaskId,
					Title = subTask.Title,
					Description = subTask.Description,
					IsComplete = Utilities.CalculateSubTaskStatus(subTask),
				})]
			};
			return toDoDto;
		}
		
		public async Task<ToDo?> EditTask(Guid taskId, CreateToDoDto createToDoDto)
		{
			ToDo? todo = _taskContext.Tasks.FirstOrDefault(task => task.ToDoId == taskId);
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
			ToDo? todo = _taskContext.Tasks.FirstOrDefault(task => task.ToDoId == taskId);
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

		public async Task<bool> AddSubTaskImage(Guid subTaskId, List<IFormFile> images)
		{
			try
			{
				SubTask? subTask = _taskContext.SubTasks.First(subTask => subTask.SubTaskId == subTaskId);
				if (subTask == null) return false;
				string directory = Path.Combine(_hostingEnvironment.ContentRootPath, "Images");

				foreach (var image in images)
				{
					string filePath = Path.Combine(directory, image.FileName);
					FileStream fileStream = new(filePath, FileMode.Create);
					await image.CopyToAsync(fileStream);
					subTask.Pictures.Add(new Picture() { Path = filePath });
				}
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
			ToDo? taskFound = _taskContext.Tasks.FirstOrDefault(task => task.ToDoId == id);
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
