using System;
using System.Collections.Generic;
namespace EmployeeWorker.Models
{
	public class SubTask
	{
		public Guid SubTaskId { get; set; }
		public string Title { get; set; } = string.Empty;
		public IList<Note> Notes { get; set; } = new List<Note>();
		public IList<Picture> Pictures { get; set; } = new List<Picture>();
	}
}
