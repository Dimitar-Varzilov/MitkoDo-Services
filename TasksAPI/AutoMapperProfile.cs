using AutoMapper;
using TasksAPI.Dto;
using TasksAPI.Models;

namespace TasksAPI
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<ToDo, CreateToDoDto>();
			CreateMap<ToDo, UpdateToDoDto>();
			CreateMap<UpdateToDoDto, ToDo>();

			CreateMap<CreateToDoDto, ToDo>();
			CreateMap<CreateToDoDto, UpdateToDoDto>();

			CreateMap<UpdateToDoDto, CreateToDoDto>();
			CreateMap<SubTask, SubTaskDto>();
			CreateMap<SubTaskDto, SubTask>();
		}
	}
}
