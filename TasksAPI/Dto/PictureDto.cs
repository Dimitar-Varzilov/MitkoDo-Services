using System.Text.Json.Serialization;
using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class PictureDto
	{
		public Guid PictureId { get; set; }
		public string Path { get; set; } = string.Empty;
		[JsonConstructor]
		public PictureDto()
		{

		}
		public PictureDto(Picture picture)
		{
			PictureId = picture.PictureId;
			Path = picture.Path;
		}
	}
}
