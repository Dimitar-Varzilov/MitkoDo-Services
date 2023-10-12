using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{
	public class AddImagesAndNoteDto
	{
		public List<IFormFile> Images { get; set; } = [];
		[Required]
		public string Note { get; set; } = string.Empty;
		public Guid EmployeeId { get; set; }
	}
}
