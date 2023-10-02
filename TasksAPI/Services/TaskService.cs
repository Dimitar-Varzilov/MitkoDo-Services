using AutoMapper;
using TasksAPI.Data;
using TasksAPI.Dto;
using TasksAPI.Models;

namespace TasksAPI.Services
{

	public interface ITaskService
	{
		Task<ToDo> AddTask(CreateToDoDto task);
		Task<ToDo?> EditTask(Guid taskId, CreateToDoDto task);
		Task<SubTaskDto?> AddSubTask(Guid subTaskId, SubTaskDto subTask);
		Task<SubTaskDto?> EditSubTask(Guid subTaskId, SubTaskDto subTask);
		Task<bool> DeleteTask(Guid taskId);
		Task<bool> AddSubTaskImage(Guid subTaskId, List<IFormFile> images);
	}
	public class TaskService(IMapper mapper, TaskContext taskContext, IWebHostEnvironment hostingEnvironment) : ITaskService
	{
		private readonly IMapper _mapper = mapper;
		private readonly TaskContext _taskContext = taskContext;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

		ToDo? GetTaskById(Guid id)
		{
			try
			{
				return _taskContext.Tasks.First(task => task.ToDoId == id);
			}
			catch (Exception)
			{
				return null;
			}
		}

		SubTask? GetSubTaskById(Guid subTaskId)
		{
			try
			{
				return _taskContext.SubTasks.First(subTask => subTask.SubTaskId == subTaskId);
				//return _taskContext.Tasks.Where(task => task.SubTasks.Any(subTask => subTask.SubTaskId == subTaskId)).SelectMany(task => task.SubTasks).First(subTask => subTask.SubTaskId == subTaskId);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<ToDo> AddTask(CreateToDoDto task)
		{
			ToDo toDo = _mapper.Map<CreateToDoDto, ToDo>(task);
			toDo.ToDoId = Guid.NewGuid();
			_taskContext.Add(toDo);
			await _taskContext.SaveChangesAsync();
			return toDo;
		}
		//TODO
		public async Task<ToDo?> EditTask(Guid taskId, CreateToDoDto dto)
		{
			ToDo? todo = GetTaskById(taskId);
			if (todo == null)
			{
				return null;
			}
			todo.Title = dto.Title;
			todo.Description = dto.Description;
			todo.StartDate = dto.StartDate;
			todo.DueDate = dto.DueDate;
			_taskContext.Update(todo);
			await _taskContext.SaveChangesAsync();
			return todo;
		}

		public async Task<SubTaskDto?> AddSubTask(Guid taskId, SubTaskDto subTaskDto)
		{
			if (subTaskDto.NotesRequired == 0) return null;
			ToDo? customTask = GetTaskById(taskId);
			if (customTask == null) return null;
			if (!Utilities.IsTaskActive(customTask)) return null;

			SubTask newSubTask = new()
			{
				Title = subTaskDto.Title,
				Description = subTaskDto.Description,
				IsCompleted = false,
				PicturesCountToBeCompleted = subTaskDto.PicsRequired,
				NotesCountToBeCompleted = subTaskDto.NotesRequired,
			};
			customTask.SubTasks.Add(newSubTask);
			await _taskContext.SaveChangesAsync();
			SubTaskDto subTaskDto1 = _mapper.Map<SubTask, SubTaskDto>(newSubTask);
			return subTaskDto1;
		}

		public async Task<SubTaskDto?> EditSubTask(Guid subTaskId, SubTaskDto subTask)
		{
			SubTask? subTaskFound = GetSubTaskById(subTaskId);
			if (subTaskFound == null)
			{
				return null;
			}
			subTask.IsCompleted = Utilities.CalculateSubTaskStatus(subTaskFound);
			_taskContext.Entry(subTaskFound).CurrentValues.SetValues(subTask);
			await _taskContext.SaveChangesAsync();
			SubTaskDto subTaskDto = _mapper.Map<SubTask, SubTaskDto>(subTaskFound);
			return subTaskDto;
		}

		public async Task<bool> AddSubTaskImage(Guid subTaskId, List<IFormFile> images)
		{
			try
			{
				SubTask? subTask = GetSubTaskById(subTaskId);
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
			ToDo? taskFound = GetTaskById(id);
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
