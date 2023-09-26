using System.ComponentModel.DataAnnotations;
using Tasks.Models;

namespace Tasks.Dto
{
	public class SubTaskDto
	{
		public int? SubTaskId { get; set; }
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

		public int TaskId { get; set; }
		public CustomTask Task { get; set; } = new CustomTask();

		public virtual ICollection<Note> NoteId { get; set; } = [];
	}
}
