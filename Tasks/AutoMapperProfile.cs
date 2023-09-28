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
			CreateMap<CustomTask, EditedCustomTaskDto>();
			CreateMap<EditedCustomTaskDto, CustomTask>();

			CreateMap<CustomTaskDto, CustomTask>();
			CreateMap<CustomTaskDto, EditedCustomTaskDto>();

			CreateMap<EditedCustomTaskDto, CustomTaskDto>();
			CreateMap<SubTask, SubTaskDto>();
			CreateMap<SubTaskDto, SubTask>();
		}
	}
}
