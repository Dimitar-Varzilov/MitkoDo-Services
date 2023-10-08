using System;

namespace EmployeeWorker.Models
{
	public class Note
	{
		public Guid NoteId { get; set; }
		public string Title { get; set; } = string.Empty;
	}
}
