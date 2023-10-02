using AutoMapper;
using EmployeeAPI.Dto;
using EmployeeAPI.Models;

namespace EmployeeAPI
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<EmployeeDto, Employee>();
			CreateMap<Employee, EmployeeDto>();
		}
	}
}
