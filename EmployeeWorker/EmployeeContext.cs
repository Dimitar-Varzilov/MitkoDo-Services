using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWorker
{
	public class EmployeeContext(DbContextOptions<EmployeeContext> options) : DbContext(options)
	{
		public DbSet<Employee> Employees { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Employee>(builder =>
			{
				builder.HasKey(p => p.EmployeeId);
				builder.Property(p => p.Name).IsRequired();
				builder.Navigation(p => p.ToDos).AutoInclude();
				builder.HasMany(p => p.ToDos).WithMany(p => p.Employees);
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
			});


			modelBuilder.Entity<ToDo>(builder =>
			{
				builder.HasKey(p => p.ToDoId);
				builder.Property(p => p.Title).IsRequired();
				builder.Property(p => p.StartDate).IsRequired();
				builder.Property(p => p.DueDate).IsRequired();
			});
		}
	}
}