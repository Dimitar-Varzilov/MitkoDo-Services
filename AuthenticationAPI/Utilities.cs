using Microsoft.Extensions.Primitives;
using System.IdentityModel.Tokens.Jwt;

namespace AuthenticationAPI
{
	public class Utilities
	{
		public static string? GetJwtTokenFromCookie(HttpRequest httpRequest)
		{
			bool tryGetValue = httpRequest.Cookies.TryGetValue("token", out string? token);
			if (!tryGetValue || token == null)
				return null;
			return token;
		}

		public static string? GetJwtTokenFromHeader(HttpRequest httpRequest)
		{
			StringValues header = httpRequest.Headers.Authorization;
			if (header.Count == 0)
				return null;
			return header[0]?.Split(" ")[1];
		}

	}
}
