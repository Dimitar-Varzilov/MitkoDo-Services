using Auth.Models;
using AutoFixture;

namespace Auth.Data
{
	public static class Seeder
	{
		// This is purely for a demo, don't anything like this in a real application!
		public static void Seed(this AuthContext authContext)
		{
			if (authContext.Users != null && !authContext.Users.Any())
			{
				Fixture fixture = new();
				List<User> users = fixture.CreateMany<User>(100).ToList();
				authContext.AddRange(users);
				authContext.SaveChanges();
			}
		}
	}
}