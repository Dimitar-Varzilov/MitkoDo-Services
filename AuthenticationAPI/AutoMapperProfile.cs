using AuthenticationAPI.Dto;
using AuthenticationAPI.Models;
using AutoMapper;

namespace AuthenticationAPI
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, RegisterDto>();
			CreateMap<RegisterDto, User>();
			CreateMap<LoginDto, User>();
			CreateMap<User, LoginDto>();
		}
	}
}
