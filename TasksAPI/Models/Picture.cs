namespace TasksAPI.Models
{
	public class Picture
	{
		public Guid PictureId { get; set; }
		public string Path { get; set; } = string.Empty;
		public Guid SubTaskId { get; set; }
		public SubTask SubTask { get; set; } = new SubTask();
	}
}
