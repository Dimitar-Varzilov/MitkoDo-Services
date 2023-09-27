using AutoMapper;
using Tasks.Data;
using Tasks.Dto;
using Tasks.Models;

namespace Tasks.Services
{

	public interface ITaskService
	{
		Task<CustomTaskDto> AddTask(CustomTaskDto task);
		Task<CustomTaskDto?> EditTask(int taskId, CustomTaskDto task);
		Task<SubTaskDto?> AddSubTask(int subTaskId, SubTaskDto subTask);
		Task<SubTaskDto?> EditSubTask(int subTaskId, SubTaskDto subTask);
		Task<bool> DeleteTask(int taskId);
		CustomTask? GetTaskById(int taskId);
		SubTask? GetSubTaskById(int subTaskId);
	}
	public class TaskService(IMapper mapper, TaskContext taskContext) : ITaskService
	{
		private readonly IMapper _mapper = mapper;
		private readonly TaskContext _taskContext = taskContext;

		public async Task<CustomTaskDto> AddTask(CustomTaskDto task)
		{
			CustomTask newTask = _mapper.Map<CustomTaskDto, CustomTask>(task);
			_taskContext.Add(newTask);
			await _taskContext.SaveChangesAsync();
			return _mapper.Map<CustomTaskDto>(newTask);
		}

		public async Task<CustomTaskDto?> EditTask(int taskId, CustomTaskDto task)
		{
			CustomTask? taskFound = GetTaskById(taskId);
			if (taskFound == null)
			{
				return null;
			}
			CustomTaskDto newTask = _mapper.Map<CustomTaskDto, CustomTaskDto>(task);
			_taskContext.Entry(taskFound).CurrentValues.SetValues(newTask);
			await _taskContext.SaveChangesAsync();
			return _mapper.Map<CustomTaskDto>(newTask);
		}

		public async Task<SubTaskDto?> AddSubTask(int taskId, SubTaskDto subTaskDto)
		{
			CustomTask? customTask = GetTaskById(taskId);
			if (customTask == null)
			{
				return null;
			}
			SubTask newSubTask = _mapper.Map<SubTask>(subTaskDto);
			customTask.SubTasks.Add(newSubTask);
			await _taskContext.SaveChangesAsync();
			return subTaskDto;
		}

		public async Task<SubTaskDto?> EditSubTask(int subTaskId, SubTaskDto subTaskDto)
		{
			SubTask? subTaskFound = GetSubTaskById(subTaskId);
			if (subTaskFound == null)
			{
				return null;
			}
			SubTaskDto newSubTask = _mapper.Map<SubTaskDto>(subTaskDto);
			_taskContext.Entry(subTaskFound).CurrentValues.SetValues(newSubTask);
			await _taskContext.SaveChangesAsync();
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
				return _taskContext.Tasks.Where(task => task.SubTasks.Any(subTask => subTask.SubTaskId == subTaskId)).SelectMany(task => task.SubTasks).First(subTask => subTask.SubTaskId == subTaskId);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
