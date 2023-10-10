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

		public bool? IsEmployeeAvailable(Guid employeeId)
		{
			Employee? employee = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);
			if (employee == null) return null;

			return employee.IsAvailable;
		}

	}


}
