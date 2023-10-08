using System;

namespace EmployeeWorker.Models
{
	public class Picture
	{
		public Guid PictureId { get; set; }
		public string Path { get; set; } = string.Empty;
	}
}
