using AuthenticationAPI.Data;
using AuthenticationAPI.Dto;
using AuthenticationAPI.Enums;
using AuthenticationAPI.Events;
using AuthenticationAPI.Models;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthenticationAPI.Services
{

	public interface IAuthService
	{
		Task<UserDto?> RegisterUser(RegisterDto registerDto, string userRole = UserRole.MEMBER);
		string LoginUser(LoginDto user);
		Task<int> ChangePassword(ChangePasswordDto changePasswordDto);
		bool ValidateToken(string token);
	}
	public class AuthService(AuthContext authContext, IConfiguration configuration, IBus bus) : IAuthService
	{
		private readonly AuthContext _authContext = authContext;
		private readonly string issuer = configuration.GetSection("JWT:ValidIssuer").Value!;
		private readonly string audience = configuration.GetSection("JWT:ValidAudience").Value!;
		readonly List<SecurityKey> securityKeys = [new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value!))];
		private readonly DateTime tokenExpirationDate = DateTime.Now.AddHours(3);
		private readonly IBus _bus = bus;

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
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim(ClaimTypes.Role, user.Role),
			];
			if (user.Role == UserRole.MANAGER)
				claims.Add(new Claim(ClaimTypes.Role, UserRole.MEMBER));

			var cred = new SigningCredentials(securityKeys[0], SecurityAlgorithms.HmacSha512Signature);
			var token = new JwtSecurityToken(issuer, audience, claims, DateTime.Now, expires, signingCredentials: cred);
			var jwt = new JwtSecurityTokenHandler().WriteToken(token);
			return jwt;
		}

		public async Task<UserDto?> RegisterUser(RegisterDto dto, string userRole = UserRole.MEMBER)
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

			User generatedUser = _authContext.Users.Add(newUser).Entity;
			await _authContext.SaveChangesAsync();
			if (userRole == UserRole.MEMBER)
				await _bus.Publish(new UserCreatedEvent(newUser.UserId, dto.Name));

			return new UserDto(newUser);
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
				ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
				return true;
			}
			catch (SecurityTokenValidationException ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}

		public async Task<int> ChangePassword(ChangePasswordDto changePasswordDto)
		{
			try
			{
				User? user = _authContext.Users.FirstOrDefault(user => user.Email == changePasswordDto.Email);
				if (user == null)
					return StatusCodes.Status404NotFound;

				if (!VerifyPasswordHash(changePasswordDto.OldPassword, user.PasswordHash, user.PasswordSalt) || changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
					return StatusCodes.Status400BadRequest;

				CreatePasswordHash(changePasswordDto.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);

				user.PasswordHash = passwordHash;
				user.PasswordSalt = passwordSalt;

				_authContext.Users.Update(user);
				await _authContext.SaveChangesAsync();
				return StatusCodes.Status200OK;
			}
			catch (Exception)
			{
				return StatusCodes.Status500InternalServerError;
			}
			
		}
	}
}
