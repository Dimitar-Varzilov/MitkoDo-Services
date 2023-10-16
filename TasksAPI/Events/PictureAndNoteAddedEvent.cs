using System.Text.Json.Serialization;
using TasksAPI.Dto;
using TasksAPI.Models;

namespace TasksAPI.Events
{
	public class PictureAndNoteAddedEvent
	{
		public Guid SubTaskId { get; set; }
		public string SubTaskTitle { get; set; } = string.Empty;
		public Guid EmployeeId { get; set; }
		public IList<PictureDto> Pictures { get; set; } = [];
		public NoteDto Note { get; set; } = new();

		[JsonConstructor]
		private PictureAndNoteAddedEvent()
		{

		}

		public PictureAndNoteAddedEvent(SubTask subTask, AddImagesAndNoteDto addImagesAndNoteDto, Guid employeeId)
		{
			SubTaskId = subTask.SubTaskId;
			SubTaskTitle = subTask.Title;
			EmployeeId = employeeId;
			Pictures = [.. subTask.Pictures.Select(p => new PictureDto(p))];
			Note = new NoteDto(subTask.Notes.First());
		}
	}
}
