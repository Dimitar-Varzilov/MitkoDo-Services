namespace Tasks.Models
{
	public class Picture
	{
		public int PictureId { get; set; }
		public string Path { get; set; } = string.Empty;
		public int SubTaskId { get; set; }
		public SubTask SubTask { get; set; } = new SubTask();
	}
}
