using AutoMapper;
using Tasks.Data;
using Tasks.Dto;
using Tasks.Models;

namespace Tasks.Services
{

	public interface ITaskService
	{
		Task<CustomTask> AddTask(CustomTaskDto task);
		//Task<EditedCustomTaskDto?> EditTask(int taskId, CustomTaskDto task);
		Task<CustomTask?> EditTask(int taskId, CustomTaskDto task);
		Task<SubTask?> AddSubTask(int subTaskId, SubTaskDto subTask);
		Task<SubTask?> EditSubTask(int subTaskId, SubTaskDto subTask);
		Task<bool> DeleteTask(int taskId);
		CustomTask? GetTaskById(int taskId);
		SubTask? GetSubTaskById(int subTaskId);
	}
	public class TaskService(IMapper mapper, TaskContext taskContext) : ITaskService
	{
		private readonly IMapper _mapper = mapper;
		private readonly TaskContext _taskContext = taskContext;

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

		public async Task<SubTask?> AddSubTask(int taskId, SubTaskDto subTaskDto)
		{
			CustomTask? customTask = GetTaskById(taskId);
			if (customTask == null)
			{
				return null;
			}
			SubTask newSubTask = _mapper.Map<SubTask>(subTaskDto);
			newSubTask.TaskId = customTask.TaskId;
			customTask.SubTasks.Add(newSubTask);
			customTask.Status = Utilities.CalculateTaskStatus(customTask);
			await _taskContext.SaveChangesAsync();
			return newSubTask;
		}

		public async Task<SubTask?> EditSubTask(int subTaskId, SubTaskDto subTask)
		{
			SubTask? subTaskFound = GetSubTaskById(subTaskId);
			if (subTaskFound == null)
			{
				return null;
			}
			_taskContext.Entry(subTaskFound).CurrentValues.SetValues(subTask);
			await _taskContext.SaveChangesAsync();
			return subTaskFound;
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

		public SubTask? GetSubTaskById(int subTaskId)
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
	}
}
