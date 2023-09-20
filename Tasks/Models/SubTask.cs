using System.ComponentModel.DataAnnotations;
using Tasks.Interfaces;

namespace Tasks.Models
{
	public class SubTask : ISubTask, IId
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Title { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public string[] Photos { get; set; } = [];
		public string[] Notes { get; set; } = [];
		public int PicsRequired { get; set; }
		[AllowedValues(int.MaxValue > 1)]
		public int NotesRequired { get; set; }
		public bool IsCompleted { get; set; } = false;
	}
}
