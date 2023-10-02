using Microsoft.EntityFrameworkCore;
using TasksAPI.Models;

namespace TasksAPI.Data
{
	public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
	{
		public DbSet<ToDo> Tasks { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<Note> Notes { get; set; }
	}
}