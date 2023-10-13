using Microsoft.EntityFrameworkCore;
using TasksAPI.Models;

namespace TasksAPI.Data
{
	public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
	{
		public DbSet<ToDo> ToDos { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Employee> Employees { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<ToDo>(builder =>
			{
				builder.HasKey(p => p.ToDoId);
				builder.Property(p => p.Title).IsRequired();
				builder.Property(p => p.Description).IsRequired();
				builder.Property(p => p.StartDate).IsRequired();
				builder.Property(p => p.DueDate).IsRequired();
				builder.HasMany(p => p.SubTasks).WithOne(p => p.Todo).HasForeignKey(p => p.ToDoId);
				builder.HasMany(p => p.Employees).WithMany(p => p.ToDos);
			});

			modelBuilder.Entity<SubTask>(builder =>
			{
				builder.HasKey(p => p.SubTaskId);
				builder.Property(p => p.Title).IsRequired();
				builder.Property(p => p.Description).IsRequired();
				builder.Property(p => p.NotesCountToBeCompleted).IsRequired().HasDefaultValue(1);
				builder.Property(p => p.PicturesCountToBeCompleted).IsRequired().HasDefaultValue(0);
				builder.HasOne(p => p.Todo).WithMany(p => p.SubTasks).HasForeignKey(p => p.ToDoId).IsRequired();
				builder.Navigation(p => p.Pictures).AutoInclude();
				builder.Navigation(p => p.Notes).AutoInclude();
			});

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
		}
	}
}