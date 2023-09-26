using AutoMapper;
using Tasks.Data;
using Tasks.Dto;
using Tasks.Models;

namespace Tasks.Services
{

	public interface ITaskService
	{
		CustomTask? GetTaskById(string id);
		Task<int> AddTask(CustomTaskDto task);
		string EditTask(string id, CustomTask task);
		int AddSubTask(SubTask task);
		string EditSubTask(string id, SubTask task);
		int DeleteTask(string id);
	}
	public class TaskService(IMapper mapper, TaskContext taskContext) : ITaskService
	{
		private readonly IMapper _mapper = mapper;
		private readonly TaskContext _taskContext = taskContext;
		private readonly List<CustomTask> _tasks = [.. taskContext.Tasks];
		private readonly List<SubTask> _subTasks = [.. taskContext.SubTasks];

		public int AddSubTask(SubTask task)
		{
			throw new NotImplementedException();
		}

		public async Task<int> AddTask(CustomTaskDto task)
		{
			Console.WriteLine(task.Title);
			_taskContext.Add(_mapper.Map<CustomTask>(task));
			await _taskContext.SaveChangesAsync();
			return (int)TaskStatus.Created;
		}

		public int DeleteTask(string id)
		{
			throw new NotImplementedException();
		}

		public string EditSubTask(string id, SubTask task)
		{
			throw new NotImplementedException();
		}

		public string EditTask(string id, CustomTask task)
		{
			throw new NotImplementedException();
		}

		public CustomTask? GetTaskById(string id)
		{
			throw new NotImplementedException();
		}
	}
}
