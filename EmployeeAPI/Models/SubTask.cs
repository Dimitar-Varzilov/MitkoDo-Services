namespace EmployeeAPI.Models
{
	public class SubTask
	{
		public Guid SubTaskId { get; set; } = Guid.Empty;
		public string Title { get; set; } = string.Empty;
		public IList<Note> Notes { get; set; } = [];
		public IList<Picture> Pictures { get; set; } = [];
	}
}
