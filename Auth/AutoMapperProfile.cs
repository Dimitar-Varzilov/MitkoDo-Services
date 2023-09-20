using Auth.Dto;
using Auth.Models.Users;
using AutoMapper;

namespace AddressBookAPI
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, RegisterDto>();
			CreateMap<RegisterDto, User>();
		}
	}
}
