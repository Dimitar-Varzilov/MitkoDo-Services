namespace TasksAPI.Dto
{
	public class EmployeeToDoDto
	{
		public string EmployeeName { get; set; } = string.Empty;
		public IList<string> TaskTitles { get; set; } = [];
		public IList<string> SubTasks { get; set; } = [];
	}
}
