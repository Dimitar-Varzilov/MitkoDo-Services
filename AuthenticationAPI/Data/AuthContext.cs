using AuthenticationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Data
{
	public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>(builder =>
			{
				builder.HasKey(p => p.UserId);
				builder.Property(p => p.Email).IsRequired();
			});
		}
	}
}