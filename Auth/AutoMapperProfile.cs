using Auth.Dto;
using Auth.Models;
using AutoMapper;

namespace Auth
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
