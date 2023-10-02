using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models
{
	public class Note
	{
		[Key]
		public int NoteId { get; set; }
		public string Title { get; set; } = string.Empty;
	}
}
