using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Dto
{
	public class AddImagesAndNoteDto
	{
		public List<IFormFile> Images { get; set; } = [];
		[Required(ErrorMessage = "Note is required")]
		[StringLength(maximumLength: 300, MinimumLength = 2, ErrorMessage = "Note must be between 2 and 300 character long")]
		public string Note { get; set; } = string.Empty;
	}
}
