using AuthenticationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Data
{
	public class AuthContext : DbContext
	{
		public DbSet<User> Users { get; set; }


        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
			Database.Migrate();
		}

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