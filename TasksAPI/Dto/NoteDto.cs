using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class NoteDto
	{
		public Guid NoteId { get; set; }
		public string Title { get; set; } = string.Empty;
        public NoteDto()
        {
            
        }
        public NoteDto(Note note)
		{
            NoteId = note.NoteId;
			Title = note.Title;
        }
    }
}
