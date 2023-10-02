namespace EmployeeAPI.Dto
{
	public class EmployeeDto
	{
		public Guid EmployeeId { get; set; } = Guid.Empty;
		public string Name { get; set; } = string.Empty;
		public bool IsAvailable { get; set; } = false;
	}
}
