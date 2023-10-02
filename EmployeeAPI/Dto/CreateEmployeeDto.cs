namespace EmployeeAPI.Dto
{
	public class CreateEmployeeDto
	{
		public Guid UserId { get; set; } = Guid.Empty;
		public string Name { get; set; } = string.Empty;
	}
}
