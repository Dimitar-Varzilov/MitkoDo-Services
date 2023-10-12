namespace EmployeeAPI.Models
{
	public class Note
	{
		public Guid NoteId { get; set; } = Guid.Empty;
		public string Title { get; set; } = string.Empty;
	}
}
