using Auth.Dto;
using Auth.Models.Users;
using Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService AuthService = authService;

        [HttpGet("{id}")]
        public object Get(string id)
        {

            User? userFinded = AuthService.GetUserById(id);
            if (userFinded == null)
            {
                return NotFound();
            }
            return Ok(userFinded);
        }

        [HttpPost("register")]
        public object Register(RegisterDto user)
        {
            User? result = AuthService.Register(user);

            if (result == null) return BadRequest(new { message = "User with that email already exists" });
            return Ok(result);
        }

        [HttpPost("login")]
        public object Login(LoginDto user)
        {
            User? result = AuthService.Login(user);

            if (result == null) return BadRequest(new { message = "Email or password is incorrect" });
            return Ok(result);
        }   
    }
}
