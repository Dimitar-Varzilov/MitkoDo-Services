using AutoMapper;
using EmployeeAPI.Data;
using EmployeeAPI.Dto;
using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{

	public interface IEmployeeService
	{
		IList<EmployeeDto> GetAllEmployees();
		EmployeeDto? GetEmployeeById(Guid employeeId);
		Task<EmployeeDto?> AddEmployee(CreateEmployeeDto UserID);
		bool? IsEmployeeAvailable(Guid employeeId);
	}
	public class EmployeeService(EmployeeContext employeeContext) : IEmployeeService
	{
		private readonly EmployeeContext _employeeContext = employeeContext;

		public IList<EmployeeDto> GetAllEmployees()
		{
			return _employeeContext.Employees.Select(x => new EmployeeDto(x)).ToList();
		}

		public EmployeeDto? GetEmployeeById(Guid employeeId)
		{
			Employee? employee = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

			if (employee == null) return null;

			return new EmployeeDto(employee);
		}

		public async Task<EmployeeDto?> AddEmployee(CreateEmployeeDto dto)
		{

			Employee? employee = _employeeContext.Employees.FirstOrDefault(x => x.UserId == dto.UserId);
			if (employee != null) return null;

			Employee newEmployee = new()
			{
				UserId = dto.UserId,
				Name = dto.Name,
			};
			Employee generatedEmployee = _employeeContext.Add(newEmployee).Entity;
			await _employeeContext.SaveChangesAsync();

			return new EmployeeDto(generatedEmployee);
		}

		public bool? IsEmployeeAvailable(Guid employeeId)
		{
			Employee? employee = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);
			if (employee == null) return null;

			return employee.IsAvailable;
		}

	}


}
