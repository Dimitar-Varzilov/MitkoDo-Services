using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWorker
{
	public class EmployeeContext(DbContextOptions<EmployeeContext> options) : DbContext(options)
	{
		public DbSet<Employee> Employees { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<ToDo> ToDos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Employee>(builder =>
			{
				builder.HasKey(p => p.EmployeeId);
				builder.Property(p => p.Name).IsRequired();
				builder.HasMany(p => p.ToDos).WithMany(p => p.Employees);
				builder.HasMany(p => p.SubTasks).WithMany(p => p.Employees);

			});

			modelBuilder.Entity<Note>(builder =>
			{
				builder.HasKey(p => p.NoteId);
				builder.Property(p => p.Title).IsRequired();
				builder.HasOne(p => p.SubTask).WithMany(p => p.Notes).HasForeignKey(p => p.SubTaskId);
			});

			modelBuilder.Entity<Picture>(builder =>
			{
				builder.HasKey(p => p.PictureId);
				builder.Property(p => p.Path).IsRequired();
				builder.HasOne(p => p.SubTask).WithMany(p => p.Pictures).HasForeignKey(p => p.SubTaskId);
			});

			modelBuilder.Entity<SubTask>(builder =>
			{
				builder.HasKey(p => p.SubTaskId);
				builder.Property(p => p.Title).IsRequired();
				builder.HasMany(p => p.Employees).WithMany(p => p.SubTasks);
				builder.HasMany(p => p.Pictures).WithOne(p => p.SubTask).HasForeignKey(p => p.SubTaskId);
				builder.HasMany(p => p.Notes).WithOne(p => p.SubTask).HasForeignKey(p => p.SubTaskId);
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