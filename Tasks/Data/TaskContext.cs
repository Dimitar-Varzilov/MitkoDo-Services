using Microsoft.EntityFrameworkCore;
using Tasks.Dto;
using Tasks.Models;

namespace Tasks.Data
{
    public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
	{
		public DbSet<CustomTask> Tasks { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<UserDto> Users { get; set; }
	}
}