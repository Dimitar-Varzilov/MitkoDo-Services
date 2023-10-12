using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeWorker
{
	public class EmployeeContext(DbContextOptions<EmployeeContext> options) : DbContext(options)
	{
		public DbSet<Employee> Employees { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<ToDo> ToDos { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Note> Notes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Employee>(builder =>
			{
				builder.HasKey(p => p.EmployeeId);
				builder.Property(p => p.Name).IsRequired();
				builder.HasMany(p => p.ToDos).WithMany(p => p.Employees);
				builder.HasMany(p => p.SubTasks).WithOne(p => p.Employee).HasForeignKey(p => p.EmployeeId);
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
				builder.HasOne(p => p.Employee).WithMany(p => p.SubTasks).HasForeignKey(p => p.EmployeeId);
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