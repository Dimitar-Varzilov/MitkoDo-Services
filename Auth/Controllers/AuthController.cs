using Auth.Dto;
using Auth.Models.Users;
using Auth.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class AuthController(IAuthService authService, IConfiguration configuration) : ControllerBase
	{
		private readonly IAuthService AuthService = authService;
		private readonly string issuer = "http://localhost:5000";
		private readonly string audience = "http://localhost:5000";
		readonly List<SecurityKey> securityKeys = [new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Secret").Value ?? "MitkoDo secret key is something veeeery long hsjidhfksdhfldiskhfliksdhgfiksdhfkihsdikufhsdikuhfsdkhfjksdhgf"))];
		User user { get; set; } = new();
		private readonly IConfiguration _configuration = configuration;

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
		public object Register(RegisterDto request)
		{
			if (request.Password != request.ConfirmPassword)
				return BadRequest("Passwords do not match");
			if (AuthService.GetUserByEmail(request.Email) != null)
				return BadRequest("User already exists");

			CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
			user.Email = request.Email;
			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;
			Program.users.Add(user);

			return Ok(true);
		}

		[HttpPost("login")]
		public object Login(LoginDto request)
		{
			User? userFromDb = AuthService.GetUserByEmail(request.Email);
			if (userFromDb == null)
				return BadRequest("User not found");

			user = userFromDb;

			if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
				return BadRequest("Wrong password");

			string token = CreateToken(user);

			return Ok(token);
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}
		private string CreateToken(User user)
		{
			List<Claim> claims =
			[
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			];

			var cred = new SigningCredentials(securityKeys[0], SecurityAlgorithms.HmacSha512Signature);
			var token = new JwtSecurityToken(
								   claims: claims,
								   expires: DateTime.UtcNow.AddDays(1),
								   signingCredentials: cred,
								   issuer: issuer,
								   audience: audience
   );
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}

		[HttpPost("verifyToken")]
		public bool VerifyToken(string token)
		{

			bool isValid = ValidateToken(token, issuer, audience, out JwtSecurityToken jwt);
			return isValid;
		}

		private bool ValidateToken(
  string token,
  string issuer,
  string audience,

 out JwtSecurityToken jwt
)
		{
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = issuer,
				ValidateAudience = true,
				ValidAudience = audience,
				ValidateIssuerSigningKey = true,
				IssuerSigningKeys = securityKeys,
				ValidateLifetime = true,

			};

			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var isValid = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken) ?? throw new SecurityTokenValidationException("Token is not valid");
				jwt = (JwtSecurityToken)validatedToken;

				return true;
			}
			catch (SecurityTokenValidationException ex)
			{
				jwt = new JwtSecurityToken();
				Console.WriteLine(ex.Message);
				return false;
			}
		}

	}
}
