using EmployeeAPI.Models;

namespace EmployeeAPI.Dto
{
	public class EmployeeDetailsDto
	{
		public string Name { get; set; } = string.Empty;

        public EmployeeDetailsDto()
        {
            
        }

        public EmployeeDetailsDto(Employee employee)
        {
            Name = employee.Name;
        }
    }
}
