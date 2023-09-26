using AutoMapper;
using Tasks.Data;
using Tasks.Dto;
using Tasks.Models;

namespace Tasks.Services
{

	public interface ITaskService
	{
		CustomTask? GetTaskById(int id);
		Task<CustomTask> AddTask(CustomTaskDto task);
		Task<CustomTask?> EditTask(int id, CustomTask task);
		Task<SubTask?> AddSubTask(int taskId, AddSubTaskDto task);
		Task<SubTask> EditSubTask(int id, SubTask task);
		Task<bool> DeleteTask(int id);
	}
	public class TaskService(IMapper mapper, TaskContext taskContext) : ITaskService
	{
		private readonly IMapper _mapper = mapper;
		private readonly TaskContext _taskContext = taskContext;

		public async Task<SubTask?> AddSubTask(int taskId, AddSubTaskDto task)
		{
			CustomTask? customTask = GetTaskById(taskId);
			if (customTask == null)
			{
				return null;
			}
			SubTask newSubTask = _mapper.Map<SubTask>(task);
			_taskContext.Add(newSubTask);
			await _taskContext.SaveChangesAsync();
			return newSubTask;
		}

		public async Task<CustomTask> AddTask(CustomTaskDto task)
		{
			CustomTask newTask = _mapper.Map<CustomTask>(task);
			_taskContext.Add(newTask);
			await _taskContext.SaveChangesAsync();
			return newTask;
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

		public async Task<SubTask?> EditSubTask(int id, SubTask task)
		{
			return null;

		}

		public async Task<CustomTask?> EditTask(int id, CustomTask task)
		{
			CustomTask? taskFound = GetTaskById(id);
			if (taskFound == null)
			{
				return null;
			}
			CustomTask newTask = _mapper.Map<CustomTask>(task);
			_taskContext.Entry(taskFound).CurrentValues.SetValues(newTask);
			await _taskContext.SaveChangesAsync();
			return newTask;
		}

		public CustomTask? GetTaskById(int id)
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
	}
}
