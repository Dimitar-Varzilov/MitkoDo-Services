using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tasks.Models;

namespace Tasks.Data
{
	public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
	{
		public DbSet<CustomTask> Tasks { get; set; }
		public DbSet<SubTask> SubTasks { get; set; }
		public DbSet<Member> Members { get; set; }
		public DbSet<Note> Notes { get; set; }
		public DbSet<User> Users { get; set; }

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			// Get the added or modified entities
			IEnumerable<CustomTask> addedOrModifiedEntities = ChangeTracker.Entries<CustomTask>()
				.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
				.Select(e => e.Entity);

			foreach (var entity in addedOrModifiedEntities)
			{
				entity.Status = Utilities.CalculateTaskStatus(entity);
			}

			IEnumerable<CustomTask?> addedOrEditedSubTasks = ChangeTracker.Entries<SubTask>()
				.Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
				.Select(e => e.Entity.TaskId).Select(id => Tasks.ToList().Find(task=>task.TaskId == id));

			if (addedOrEditedSubTasks.Any())
			{
				foreach (var subTask in addedOrEditedSubTasks)
				{
					if (subTask == null)
					{
						continue;
					}
					subTask.Status = Utilities.CalculateTaskStatus(subTask);
				}

			}


			return await base.SaveChangesAsync(cancellationToken);
		}


	}


}