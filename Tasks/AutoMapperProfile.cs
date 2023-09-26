using AutoMapper;
using Tasks.Dto;
using Tasks.Models;

namespace Tasks
{
	public class AutoMapperProfile:Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<CustomTask, CustomTaskDto>();
			CreateMap<CustomTaskDto, CustomTask>();
			CreateMap<SubTask, SubTaskDto>();
			CreateMap<SubTaskDto, SubTask>();
			CreateMap<AddSubTaskDto, SubTask>();
			CreateMap<SubTask, AddSubTaskDto>();
		}
	}
}
