using System.Text.Json.Serialization;
using TasksAPI.Models;

namespace TasksAPI.Events
{
	public class SubTaskAddedEvent
	{
		public Guid SubTaskId { get; set; } = Guid.Empty;
		public string Title { get; set; } = string.Empty;
		public SubTaskAddedEvent()
		{

		}

		public SubTaskAddedEvent(SubTask subTaskToAdd)
		{
			SubTaskId = subTaskToAdd.SubTaskId;
			Title = subTaskToAdd.Title;
		}
	}
}
