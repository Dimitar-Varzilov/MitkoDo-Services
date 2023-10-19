using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{
	public class CreateSubTaskDto
	{
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		public int PicturesCountToBeCompleted { get; set; }
		[Required]
		[Range(1, int.MaxValue)]
		public int NotesCountToBeCompleted { get; set; }
	}
}
