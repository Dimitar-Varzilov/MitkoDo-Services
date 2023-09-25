using Auth.Data;
using Auth.Dto;
using Auth.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth.Services
{

	public interface IAuthService
	{
		User? GetUserById(string id);
		User? GetUserByEmail(string email);
		int RegisterUser(RegisterDto user);
		string LoginUser(LoginDto user);
		bool ValidateToken(string token);
		string InvalidateToken(string token);

		int DeleteUser(string id);
	}
	public class AuthService(IMapper mapper, AuthContext authContext, IConfiguration configuration) : IAuthService
	{
		private readonly IMapper _mapper = mapper;
		private readonly AuthContext _authContext = authContext;
		private readonly List<User> _users = [.. authContext.Users];
		private readonly string issuer = "http://localhost:5000";
		private readonly string audience = "http://localhost:5000";
		readonly List<SecurityKey> securityKeys = [new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Secret").Value ?? string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 16))))];

		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512();
			passwordSalt = hmac.Key;
			passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
		}

		private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using var hmac = new HMACSHA512(passwordSalt);
			var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			return computedHash.SequenceEqual(passwordHash);
		}

		private string CreateToken(User user, DateTime expires)
		{
			List<Claim> claims =
			[
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.NameIdentifier, user.Id)
			];

			var cred = new SigningCredentials(securityKeys[0], SecurityAlgorithms.HmacSha512Signature);
			var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: cred, issuer: issuer, audience: audience);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}

		private bool ValidateToken(string token, string issuer, string audience, out JwtSecurityToken jwt)
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


		public User? GetUserById(string id)
		{
			return _users.Find(user => user.Id == id);
		}

		public User? GetUserByEmail(string email)
		{
			return _users.Find(user => user.Email == email);
		}

		public int RegisterUser(RegisterDto request)
		{
			var foundUser = GetUserByEmail(request.Email);
			if (foundUser != null)
				return StatusCodes.Status409Conflict;

			CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

			User user = new()
			{
				Email = request.Email,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt
			};

			_authContext.Users.Add(user);
			_authContext.SaveChanges();

			return StatusCodes.Status201Created;
		}

		public string LoginUser(LoginDto request)
		{
			var userFromDb = GetUserByEmail(request.Email);
			if (userFromDb == null)
				return StatusCodes.Status404NotFound.ToString();

			if (!VerifyPasswordHash(request.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
				return StatusCodes.Status400BadRequest.ToString();

			string token = CreateToken(userFromDb, DateTime.UtcNow.AddHours(1));

			return token;
		}

		public bool ValidateToken(string token)
		{
			bool isValid = ValidateToken(token, issuer, audience, out _);
			return isValid;
		}

		public int DeleteUser(string id)
		{
			User? userFound = GetUserById(id);
			if (userFound == null)
			{
				return StatusCodes.Status404NotFound;
			}
			_authContext.Users?.Remove(userFound);
			_authContext.SaveChanges();
			return StatusCodes.Status200OK;
		}

		public string InvalidateToken(string token)
		{
			return CreateToken(new User(), DateTime.UtcNow.AddDays(-1));
		}
	}
}
