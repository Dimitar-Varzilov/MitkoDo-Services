namespace EmployeeAPI.Models
{
	public class Picture
	{
		public Guid PictureId { get; set; } = Guid.Empty;
		public string Path { get; set; } = string.Empty;
	}
}
