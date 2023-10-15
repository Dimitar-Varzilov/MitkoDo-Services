using EmployeeAPI.Authorization;
using EmployeeAPI.Dto;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeAPI.Controllers
{
	[ApiController]
	[Authorize]
	[Route("[controller]")]
	public class EmployeeController(IEmployeeService employeeService) : ControllerBase
	{
		private readonly IEmployeeService _employeeService = employeeService;

		[HttpGet]
		[Authorize(Roles = UserRole.MANAGER)]
		public ActionResult<IList<EmployeeDto>> GetAllEmployees()
		{
			IList<EmployeeDto> employees = _employeeService.GetAllEmployees();
			return Ok(employees);
		}

		[HttpGet("{employeeId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public ActionResult<EmployeeDto> GetEmployeeById(Guid employeeId)
		{
			EmployeeDto? employee = _employeeService.GetEmployeeById(employeeId);
			return employee == null ? BadRequest() : Ok(employee);
		}

		[HttpGet("employeeDetailsFromToken")]
		[Authorize(Roles = UserRole.MEMBER)]
		public ActionResult<EmployeeDetailsDto> GetEmployeeDetailsFromToken()
		{
			string? guid = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (guid == null)
			{
				return BadRequest();
			}
			bool success = Guid.TryParse(guid, out Guid employeeId);
			if (!success)
			{
				return BadRequest();
			};
			EmployeeDetailsDto? employee = _employeeService.GetEmployeeDetails(employeeId);
			return employee == null ? BadRequest() : Ok(employee);
		}

		[HttpGet("employeeDetails/{employeeId:guid}")]
		[Authorize(Roles = UserRole.MANAGER)]
		public ActionResult<EmployeeDetailsDto> GetEmployeeDetails(Guid employeeId)
		{
			EmployeeDetailsDto? employee = _employeeService.GetEmployeeDetails(employeeId);
			return employee == null ? BadRequest() : Ok(employee);
		}


		[HttpGet("isAvailable/{employeeId:guid}")]
		public IActionResult IsEmployeeAvailable(Guid employeeId)
		{
			bool? employee = _employeeService.IsEmployeeAvailable(employeeId);
			return employee == null ? BadRequest("Employee Not Found") : Ok(employee);
		}
	}
}
