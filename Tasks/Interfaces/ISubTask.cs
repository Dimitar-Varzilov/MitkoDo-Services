using System.ComponentModel.DataAnnotations;

namespace Tasks.Interfaces
{
	public interface ISubTask
	{
		[Required]
		string Title { get; set; }
		[Required]
		string Description { get; set; }
		string[] Photos { get; set; }
		string[] Notes { get; set; }
		int PicsRequired { get; set; }
		[MinLength(1)]
		int NotesRequired { get; set; }
		bool IsCompleted { get; set; }
	}
}
