namespace EmployeeAPI.Models
{
	public class ToDo
	{
		public Guid ToDoId { get; set; }
		public string Title { get; set; } = string.Empty;
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);
		public bool IsActive
		{
			get => Utilities.CalculateTaskActive(StartDate, DueDate);
		}
	}
}
