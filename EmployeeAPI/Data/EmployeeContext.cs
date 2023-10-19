using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Data
{
	public class EmployeeContext(DbContextOptions<EmployeeContext> options) : DbContext(options)
	{
		public DbSet<Employee> Employees { get; set; }
		public DbSet<ToDo> ToDos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			modelBuilder.Entity<Employee>(builder =>
			{
				builder.HasKey(p => p.EmployeeId);
				builder.Property(p => p.Name).IsRequired();
				builder.HasMany(p => p.ToDos).WithMany(p => p.Employees);
			});

			modelBuilder.Entity<ToDo>(builder =>
			{
				builder.HasKey(p => p.ToDoId);
				builder.Property(p => p.Title).IsRequired();
				builder.Property(p => p.StartDate).IsRequired();
				builder.Property(p => p.DueDate).IsRequired();
				builder.HasMany(p => p.Employees).WithMany(p => p.ToDos);
			});
		}
	}
}