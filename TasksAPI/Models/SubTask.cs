using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models
{
	public class SubTask
	{
		[Key]
		public Guid SubTaskId { get; set; }
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		public int PicturesCountToBeCompleted { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int NotesCountToBeCompleted { get; set; }
		public bool IsCompleted { get; set; } = false;

		public int TaskId { get; set; }
		public ToDo Task { get; set; } = new ToDo();

		public ICollection<Note> Notes { get; set; } = [];
		public ICollection<Picture> Pictures { get; set; } = [];
	}
}
