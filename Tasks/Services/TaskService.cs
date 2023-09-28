using AutoMapper;
using Microsoft.Extensions.Hosting.Internal;
using Tasks.Data;
using Tasks.Dto;
using Tasks.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Tasks.Services
{

	public interface ITaskService
	{
		Task<CustomTask> AddTask(CustomTaskDto task);
		//Task<EditedCustomTaskDto?> EditTask(int taskId, CustomTaskDto task);
		Task<CustomTask?> EditTask(int taskId, CustomTaskDto task);
		Task<SubTaskDto?> AddSubTask(int subTaskId, SubTaskDto subTask);
		Task<SubTaskDto?> EditSubTask(int subTaskId, SubTaskDto subTask);
		Task<bool> DeleteTask(int taskId);
		Task<bool> AddSubTaskImage(int subTaskId, List<IFormFile> images);
	}
	public class TaskService(IMapper mapper, TaskContext taskContext, IWebHostEnvironment hostingEnvironment) : ITaskService
	{
		private readonly IMapper _mapper = mapper;
		private readonly TaskContext _taskContext = taskContext;
		private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

		CustomTask? GetTaskById(int id)
		{
			try
			{
				return _taskContext.Tasks.First(task => task.TaskId == id);
			}
			catch (Exception)
			{
				return null;
			}
		}

		SubTask? GetSubTaskById(int subTaskId)
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

		public async Task<CustomTask> AddTask(CustomTaskDto task)
		{
			CustomTask newTask = _mapper.Map<CustomTaskDto, CustomTask>(task);
			_taskContext.Add(newTask);
			await _taskContext.SaveChangesAsync();
			return newTask;
		}

		public async Task<CustomTask?> EditTask(int taskId, CustomTaskDto task)
		{
			CustomTask? taskFound = GetTaskById(taskId);
			if (taskFound == null)
			{
				return null;
			}
			_taskContext.Entry(taskFound).CurrentValues.SetValues(task);
			await _taskContext.SaveChangesAsync();
			return taskFound;
		}

		public async Task<SubTaskDto?> AddSubTask(int taskId, SubTaskDto subTaskDto)
		{
			if (subTaskDto.NotesRequired == 0) return null;
			CustomTask? customTask = GetTaskById(taskId);
			if (customTask == null) return null;
			if (!Utilities.IsTaskActive(customTask)) return null;

			SubTask newSubTask = new()
			{
				Title = subTaskDto.Title,
				Description = subTaskDto.Description,
				IsCompleted = false,
				PicsRequired = subTaskDto.PicsRequired,
				NotesRequired = subTaskDto.NotesRequired,
			};
			customTask.SubTasks.Add(newSubTask);
			customTask.Status = Utilities.CalculateTaskStatus(customTask);
			await _taskContext.SaveChangesAsync();
			SubTaskDto subTaskDto1 = _mapper.Map<SubTask, SubTaskDto>(newSubTask);
			return subTaskDto1;
		}

		public async Task<SubTaskDto?> EditSubTask(int subTaskId, SubTaskDto subTask)
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

		public async Task<bool> DeleteTask(int id)
		{
			CustomTask? taskFound = GetTaskById(id);
			if (taskFound == null)
			{
				return false;
			}
			_taskContext.Remove(taskFound);
			await _taskContext.SaveChangesAsync();
			return true;
		}

		public async Task<bool> AddSubTaskImage(int subTaskId, List<IFormFile> images)
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
	}
}
