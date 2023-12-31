﻿namespace TasksAPI.Models
{
	public class Picture
	{
		public Guid PictureId { get; set; } = Guid.Empty;
		public string Path { get; set; } = string.Empty;
		public Guid SubTaskId { get; set; } = Guid.Empty;
		public SubTask SubTask { get; set; } = new SubTask();
		public Guid UploadedBy { get; set; } = Guid.Empty;
	}
}
