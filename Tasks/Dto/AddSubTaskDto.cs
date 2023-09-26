using System.ComponentModel.DataAnnotations;

namespace Tasks.Dto
{
	public class AddSubTaskDto
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
