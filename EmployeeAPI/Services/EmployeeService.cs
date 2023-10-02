using AutoMapper;
using EmployeeAPI.Data;
using EmployeeAPI.Dto;
using EmployeeAPI.Models;

namespace EmployeeAPI.Services
{

	public interface IEmployeeService
	{
		EmployeeDto? GetEmployeeById(Guid employeeId);
		Task<EmployeeDto?> AddEmployee(CreateEmployeeDto UserID);
		bool? IsEmployeeAvailable(Guid employeeId);
	}
	public class EmployeeService(IMapper mapper, EmployeeContext employeeContext) : IEmployeeService
	{
		private readonly IMapper _mapper = mapper;
		private readonly EmployeeContext _employeeContext = employeeContext;

		Employee? FindEmployeeById(Guid inputEmployeeId)
		{
			try
			{
				
				return _employeeContext.Employees.First(employee => employee.EmployeeId == inputEmployeeId);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return null;
			}
		}

		Employee? FindEmployeeByUserId(Guid userId)
		{
			try
			{
				return _employeeContext.Employees.First(employee => employee.UserId == userId);
			}
			catch (Exception)
			{
				return null;
			}
		}


		public EmployeeDto? GetEmployeeById(Guid employeeId)
		{
			//bool success = Guid.TryParse(employeeId.ToString(), out Guid newGuid);
			//if (!success) return null;
		//	Employee? employee = FindEmployeeById(employeeId);


			var e = _employeeContext.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);

			if (e == null)
			{
				return null;
			}
			return _mapper.Map<EmployeeDto>(e);
		}

		public async Task<EmployeeDto?> AddEmployee(CreateEmployeeDto dto)
		{
			bool success = Guid.TryParse(dto.UserId.ToString(), out Guid newGuid);
			if (!success) return null;
			Employee? employee = FindEmployeeByUserId(newGuid);
			if (employee != null) return null;

			Employee newEmployee = new()
			{
				EmployeeId = Guid.NewGuid(),
				UserId = dto.UserId,
				Name = dto.Name,
			};
			Console.Clear();
			await Console.Out.WriteLineAsync(newEmployee.EmployeeId.ToString());
			Employee generatedEmployee = _employeeContext.Add(newEmployee).Entity;
			await _employeeContext.SaveChangesAsync();
			return _mapper.Map<EmployeeDto>(generatedEmployee);
		}

		public bool? IsEmployeeAvailable(Guid employeeId)
		{
			Employee? employee = FindEmployeeById(employeeId);
			if (employee == null) return null;

			return true;//employee.IsAvailable;
		}

	}


}
