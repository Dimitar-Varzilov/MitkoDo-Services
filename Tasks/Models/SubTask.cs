using System.ComponentModel.DataAnnotations;

namespace Tasks.Models
{
    public interface ISubTask
	{
		int SubTaskId { get; set; }
		string Title { get; set; }
		string Description { get; set; }
		int PicsRequired { get; set; }
		int NotesRequired { get; set; }
		bool IsCompleted { get; set; }
		ICollection<Note> Notes { get; set; }
		ICollection<Picture> Pictures{ get; set; }
	}
	public class SubTask : ISubTask
	{
		[Key]
		public int SubTaskId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		public int PicsRequired { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int NotesRequired { get; set; }
		public bool IsCompleted { get; set; } = false;

		public int TaskId { get; set; }
		public CustomTask Task { get; set; } = new CustomTask();

		public ICollection<Note> Notes { get; set; } = [];
		public ICollection<Picture> Pictures { get; set; } = [];
	}
}
