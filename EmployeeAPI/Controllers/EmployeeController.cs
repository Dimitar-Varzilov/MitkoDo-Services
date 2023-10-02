using EmployeeAPI.Dto;
using EmployeeAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmployeeController(IEmployeeService employeeService) : ControllerBase
	{
		private readonly IEmployeeService _employeeService = employeeService;

		[HttpGet("{employeeId:guid}")]
		public ActionResult<EmployeeDto> GetEmployeeById(Guid employeeId)
		{
			EmployeeDto? employee = _employeeService.GetEmployeeById(employeeId);
			return employee == null ? BadRequest() : Ok(employee);
		}

		[HttpGet("isAvailable/{employeeId:guid}")]
		public IActionResult IsEmployeeAvailable(Guid employeeId)
		{
			bool? employee = _employeeService.IsEmployeeAvailable(employeeId);
			return employee == null ? BadRequest("Employee Not Found") : Ok(employee);
		}

		[HttpPost]
		public async Task<ActionResult<EmployeeDto?>> AddEmployee(CreateEmployeeDto employeeDto)
		{
			EmployeeDto? employeeCreated = await _employeeService.AddEmployee(employeeDto);
			if (employeeCreated == null) return BadRequest();
			return Ok(employeeCreated);
		}
		public class EmployeeDto2
		{
			public string UserId { get; set; } = string.Empty;
			public string Name { get; set; } = string.Empty;
		}

	}
}