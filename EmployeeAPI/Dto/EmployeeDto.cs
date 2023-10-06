using EmployeeAPI.Models;

namespace EmployeeAPI.Dto
{
	public class EmployeeDto : CreateEmployeeDto
	{
		public Guid EmployeeId { get; set; } = Guid.Empty;
		public bool IsAvailable { get; set; } = false;

		public EmployeeDto() { }

        public EmployeeDto(Employee employee)
        {
            EmployeeId = employee.EmployeeId;
			Name = employee.Name;
			IsAvailable = employee.IsAvailable;
			UserId = employee.UserId;
        }
    }
}
