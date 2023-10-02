using Microsoft.EntityFrameworkCore;
using TasksAPI.Models;

namespace TasksAPI.Data
{
	public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
	{
		public DbSet<ToDo> Tasks { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<Note> Notes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Employee>(builder =>
			{
				builder.HasKey(p => p.EmployeeId);
				builder.Property(p => p.Name).IsRequired();
			});

			modelBuilder.Entity<Note>(builder =>
			{
				builder.HasKey(p => p.NoteId);
				builder.Property(p => p.Title).IsRequired();
			});

			modelBuilder.Entity<Picture>(builder =>
			{
				builder.HasKey(p => p.PictureId);
				builder.Property(p => p.Path).IsRequired();
			});

			modelBuilder.Entity<SubTask>(builder =>
			{
				builder.HasKey(p => p.SubTaskId);
				builder.Property(p => p.Title).IsRequired();
				builder.Property(p => p.Description).IsRequired();
				builder.Property(p => p.NotesCountToBeCompleted).IsRequired().HasDefaultValue(1);
			});

			modelBuilder.Entity<ToDo>(builder =>
			{
				builder.HasKey(p => p.ToDoId);
				builder.Property(p => p.Title).IsRequired();
				builder.Property(p => p.Description).IsRequired();
				builder.Property(p => p.StartDate).IsRequired();
				builder.Property(p => p.DueDate).IsRequired();
			});

		}
	}
}