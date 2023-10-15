using EmployeeAPI.Models;

namespace EmployeeAPI.Dto
{
	public class EmployeeDetailsDto
	{
		public Guid EmployeeId { get; set; } = Guid.Empty;
		public string Name { get; set; } = string.Empty;

        public EmployeeDetailsDto()
        {
            
        }

        public EmployeeDetailsDto(Employee employee)
        {
			EmployeeId = employee.EmployeeId;
            Name = employee.Name;
        }
    }
}
