using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth
{
    public class AuthContext : DbContext
	{
		public AuthContext(DbContextOptions<AuthContext> options) : base(options)
		{
		}

		public DbSet<User>? Users { get; set; }
	}
}