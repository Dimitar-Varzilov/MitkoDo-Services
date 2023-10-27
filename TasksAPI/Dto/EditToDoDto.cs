using System.ComponentModel.DataAnnotations;
using TasksAPI.Models;

namespace TasksAPI.Dto
{
	public class EditToDoDto
	{
		[Required]
		public string Title { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public DateTime StartDate { get; set; } = DateTime.Now;
		[Required]
		public DateTime DueDate { get; set; } = DateTime.Now.AddDays(1);

		public EditToDoDto()
		{

		}

		public EditToDoDto(ToDo toDo)
		{
			Title = toDo.Title;
			Description = toDo.Description;
			StartDate = toDo.StartDate;
			DueDate = toDo.DueDate;
		}
	}
}
