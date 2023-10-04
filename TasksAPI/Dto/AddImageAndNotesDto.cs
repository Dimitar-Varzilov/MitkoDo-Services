namespace TasksAPI.Dto
{
	public class AddImageAndNotesDto
	{
		public List<IFormFile> Images { get; set; } = [];
		public string Note { get; set; } = string.Empty;
	}
}
