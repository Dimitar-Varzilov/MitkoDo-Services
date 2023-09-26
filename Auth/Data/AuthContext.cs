using Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Data
{
	public class AuthContext(DbContextOptions<AuthContext> options) : DbContext(options)
	{
		public DbSet<User> Users { get; set; }
	}
}