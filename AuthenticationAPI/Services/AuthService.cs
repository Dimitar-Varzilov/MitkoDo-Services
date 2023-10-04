using AuthenticationAPI.Data;
using AuthenticationAPI.Dto;
using AuthenticationAPI.Enums;
using AuthenticationAPI.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthenticationAPI.Services
{

	public interface IAuthService
	{
		Task<UserDto?> RegisterUser(RegisterDto user, UserRole userRole = UserRole.MEMBER);
		string LoginUser(LoginDto user);
		bool ValidateToken(string token);
		DateTime GetTokenExpirationDate();
	}
	public class AuthService(IMapper mapper, AuthContext authContext, IConfiguration configuration) : IAuthService
	{
		private readonly IMapper _mapper = mapper;
		private readonly AuthContext _authContext = authContext;
		private readonly string issuer = $"http://localhost:7145";
		private readonly string audience = "http://localhost:5000";
		readonly List<SecurityKey> securityKeys = [new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Secret").Value ?? string.Concat(Enumerable.Repeat(Guid.NewGuid().ToString(), 16))))];
		private readonly DateTime tokenExpirationDate = DateTime.Now.AddDays(30);

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
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
			];

			var cred = new SigningCredentials(securityKeys[0], SecurityAlgorithms.HmacSha512Signature);
			var token = new JwtSecurityToken(claims: claims, expires: expires, signingCredentials: cred, issuer: issuer, audience: audience);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}

		public async Task<UserDto?> RegisterUser(RegisterDto dto, UserRole userRole = UserRole.MEMBER)
		{
			User? user = _authContext.Users.FirstOrDefault(user => user.Email == dto.Email);
			if (user != null)
				return null;

			CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

			User newUser = new()
			{
				UserId = Guid.NewGuid(),
				Email = dto.Email,
				PasswordHash = passwordHash,
				PasswordSalt = passwordSalt,
				Role = userRole
			};

			_authContext.Users.Add(newUser);
			await _authContext.SaveChangesAsync();

			return _mapper.Map<UserDto>(newUser);
		}

		public string LoginUser(LoginDto request)
		{
			User? user = _authContext.Users.FirstOrDefault(user => user.Email == request.Email);
			if (user == null)
				return StatusCodes.Status404NotFound.ToString();

			if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
				return StatusCodes.Status400BadRequest.ToString();

			string token = CreateToken(user, tokenExpirationDate);

			return token;
		}

		public bool ValidateToken(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				bool canRead = tokenHandler.CanReadToken(token);
				if (!canRead)
					return false;
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
				var isValid = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken) ?? throw new SecurityTokenValidationException("Token is not valid");

				return true;
			}
			catch (SecurityTokenValidationException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public DateTime GetTokenExpirationDate()
		{
			return tokenExpirationDate;
		}
	}
}
