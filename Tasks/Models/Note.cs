using System.ComponentModel.DataAnnotations;

namespace Tasks.Models
{
	public interface INote
	{
		int NoteId { get; set; }
		string Title { get; set; }

		int SubTaskId { get; set; }
		SubTask SubTask { get; set; }
	}
	public class Note : INote
	{
		[Key]
		public int NoteId { get; set; }
		public string Title { get; set; } = string.Empty;

		public int SubTaskId { get; set; }
		public SubTask SubTask { get; set; } = new SubTask();
	}
}
