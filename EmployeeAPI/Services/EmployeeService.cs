using EmployeeAPI.Data;
using EmployeeAPI.Dto;
using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Services
{

	public interface IEmployeeService
	{
		IList<EmployeeDto> GetAllEmployees();
		EmployeeDto? GetEmployeeById(Guid employeeId);
		EmployeeDetailsDto? GetEmployeeDetails(Guid employeeId);
		bool? IsEmployeeAvailable(Guid employeeId);
	}
	public class EmployeeService(EmployeeContext employeeContext) : IEmployeeService
	{
		private readonly EmployeeContext _employeeContext = employeeContext;

		public IList<EmployeeDto> GetAllEmployees()
		{
			var employees = _employeeContext.Employees
				.AsNoTracking()
				.Include(e => e.ToDos);
			return [.. employees.Select(e => new EmployeeDto(e))];
		}

		public EmployeeDto? GetEmployeeById(Guid employeeId)
		{
			Employee? employee = _employeeContext.Employees
				.AsNoTracking()
				.FirstOrDefault(x => x.EmployeeId == employeeId);

			if (employee == null) return null;

			return new EmployeeDto(employee);
		}

		public EmployeeDetailsDto? GetEmployeeDetails(Guid employeeId)
		{
			Employee? employee = _employeeContext.Employees
				.AsNoTracking()
				.FirstOrDefault(x => x.EmployeeId == employeeId);
			if (employee == null) return null;

			return new EmployeeDetailsDto(employee);
		}

		public bool? IsEmployeeAvailable(Guid employeeId)
		{
			Employee? employee = _employeeContext.Employees
				.FirstOrDefault(x => x.EmployeeId == employeeId);
			if (employee == null) return null;

			return employee.IsAvailable;
		}
	}
}
