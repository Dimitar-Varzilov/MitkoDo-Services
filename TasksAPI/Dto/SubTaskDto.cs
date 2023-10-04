namespace TasksAPI.Dto
{
	public class SubTaskDto
	{
		public Guid SubTaskId { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public bool IsComplete { get; set; } = false;
	}
}
