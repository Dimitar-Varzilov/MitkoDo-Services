using Auth.Models.Users;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class AuthController : ControllerBase
	{
		private static readonly List<User> users = new();

		[HttpGet]
		public IEnumerable<User> Get()
		{
			return users;
		}

		[HttpPost]
		public object Register(User user)
		{
			if (users.Any(u => u.email == user.email))
			{
				return BadRequest("User with that email already exists");
			}
			int odd = Math.Abs(DateTime.Now.Millisecond);

			User newUser = new User();
			newUser.email = user.email;
			newUser.password = user.password;
			newUser.tasks = user.tasks;
			newUser.Id = odd.ToString();

			users.Add(newUser);

			return Ok();
		}
	}
}
