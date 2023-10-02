using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{
	public class SubTaskDto
	{
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		//public string[] Photos { get; set; } = [];
		public int PicsRequired { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int NotesRequired { get; set; }
		public bool IsCompleted { get; set; } = false;
	}
}
